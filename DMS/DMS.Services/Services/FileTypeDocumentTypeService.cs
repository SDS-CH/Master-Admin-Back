using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using Master.Common.Classes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class FileTypeDocumentTypeService<TEntityDTO, TEntity, TContext>
        : BaseService<TEntityDTO, TEntity, TContext>, IFileTypeDocumentTypeService<TEntityDTO>
        where TEntity : GedDocumentType, new()
        where TEntityDTO : GedDocumentTypeDto
        where TContext : DmsReferenceContext
    {
        private readonly IFileTypeDocumentTypeRepository<GedDocumentType> _repository;

        public FileTypeDocumentTypeService(TContext dbContext, IMapper mapper, IFileTypeDocumentTypeRepository<GedDocumentType> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
        }

        public Task<List<GedDocumentTypeDto>> GetAttachedTypesAsync(string fileTypeCode)
        {
            return _repository.GetAttachedTypesAsync(fileTypeCode);
        }

        public Task<DataSourceResult> SearchTypesAsync(string fileTypeCode, DataSourceRequest request)
        {
            return _repository.SearchAvailableTypesAsync(fileTypeCode, request);
        }

        public async Task<OperationResult> AttachTypesAsync(string fileTypeCode, AttachDocumentTypesDto dto)
        {
            try
            {
                var typeIds = (dto?.TypeIds ?? new List<int>())
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();

                if (typeIds.Count == 0)
                {
                    return new OperationResult(true, "Select at least one document type.");
                }

                var fileType = await _repository.FindFileTypeAsync(fileTypeCode);
                var tenant = fileType?.TenantId
                             ?? await _repository.FindFallbackTenantAsync()
                             ?? Guid.Empty;

                var code = (fileType?.CodeTypeDossier ?? fileTypeCode).Trim();

                var types = await _repository.GetTypesByIdsAsync(typeIds);
                if (types.Count == 0)
                {
                    return new OperationResult(true, "No matching document type found.");
                }

                var added = 0;
                foreach (var type in types)
                {
                    var typeTenant = tenant != Guid.Empty ? tenant : type.TenantId;

                    var categorieType = await _repository.FindCategorieTypeAsync(type.Id, type.TypeCategory);
                    if (categorieType is null)
                    {
                        categorieType = new GedCategorieType
                        {
                            TypesId = type.Id,
                            CategorieId = type.TypeCategory,
                            TenantId = typeTenant
                        };
                        _repository.AddCategorieType(categorieType);
                    }
                    else if (await _repository.MappingExistsAsync(categorieType.Id, code))
                    {
                        continue; // déjà rattaché
                    }

                    var mapping = new GedCategorieTypeTypesDossier
                    {
                        TypeDossierCode = code,
                        TenantId = typeTenant
                    };

                    // GedCategorieType existante : FK directe. Nouvelle : navigation (FK résolue au SaveChanges).
                    if (categorieType.Id != 0)
                    {
                        mapping.CategorieTypeId = categorieType.Id;
                    }
                    else
                    {
                        mapping.CategorieType = categorieType;
                    }

                    _repository.AddMapping(mapping);
                    added++;
                }

                if (added == 0)
                {
                    return new OperationResult(false, "Selected document types are already attached.");
                }

                await _repository.SaveChangesAsync();
                return new OperationResult(false, "Operation terminated successfully.");
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }

        public async Task<OperationResult> DetachTypeAsync(string fileTypeCode, int typeId)
        {
            try
            {
                var mappings = await _repository.GetMappingsForTypeAsync(fileTypeCode, typeId);
                if (mappings.Count == 0)
                {
                    return new OperationResult(true, "Document type mapping not found.");
                }

                _repository.RemoveMappings(mappings);
                await _repository.SaveChangesAsync();
                return new OperationResult(false, "Operation terminated successfully.");
            }
            catch (Exception e)
            {
                return new OperationResult(true, e.Message);
            }
        }
    }
}
