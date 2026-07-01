using DMS.DTO.DTOs;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;

namespace DMS.EFCore.Repositories
{
    public class GedDocumentCategoryRepository : GenericBaseRepository<GedDocumentCategory, DmsReferenceContext>, IGedDocumentCategoryRepository<GedDocumentCategory>
    {
        public GedDocumentCategoryRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<List<GedDocumentCategory>> GetCategories()
        {
            return await dbContext.GedDocumentCategories
                .AsNoTracking()
                .OrderBy(c => c.CategoryName)
                .ToListAsync();
        }

        public async Task<IEnumerable<GedDocumentTypeDto>> GetTypes(int categoryId, int? industryId, bool unassignedOnly = false)
        {
            var query = dbContext.GedDocumentTypes
                .AsNoTracking()
                .Where(t => t.TypeCategory == categoryId);

            if (unassignedOnly)
            {
                query = query.Where(t => t.IndustryId == null);
            }
            else if (industryId.HasValue)
            {
                query = query.Where(t => t.IndustryId == industryId.Value);
            }

            return await query
                .OrderBy(t => t.TypeName)
                .Select(t => ToDto(t))
                .ToListAsync();
        }

        public async Task<GedDocumentTypeDto> CreateType(GedDocumentTypeDto dto)
        {
            await ClearUnknownDefaultFolder(dto);

            var type = new GedDocumentType
            {
                TypeName = dto.TypeName,
                TypeCategory = dto.TypeCategory,
                UrlTemplate = dto.UrlTemplate,
                TypeNameFr = dto.TypeNameFr,
                TypeNameEn = dto.TypeNameEn,
                TypeNameEs = dto.TypeNameEs,
                TypeNamePt = dto.TypeNamePt,
                CodeTemplate = dto.CodeTemplate,
                RequiredValidation = dto.RequiredValidation,
                DefaultFolder = dto.DefaultFolder,
                TypeRole = dto.TypeRole,
                Visible = dto.Visible,
                UseGrCodeRecogizing = dto.UseGrCodeRecogizing,
                DownloadOnlyLastVersion = dto.DownloadOnlyLastVersion,
                NeedsEncryption = dto.NeedsEncryption,
                TenantId = dto.TenantId,
                IndustryId = dto.IndustryId
            };

            dbContext.GedDocumentTypes.Add(type);
            await dbContext.SaveChangesAsync();

            return ToDto(type);
        }

        public async Task<GedDocumentTypeDto?> UpdateType(int id, GedDocumentTypeDto dto)
        {
            await ClearUnknownDefaultFolder(dto);

            var type = await dbContext.GedDocumentTypes.FindAsync(id);

            if (type is null)
            {
                return null;
            }

            type.TypeName = dto.TypeName;
            type.TypeCategory = dto.TypeCategory;
            type.UrlTemplate = dto.UrlTemplate;
            type.TypeNameFr = dto.TypeNameFr;
            type.TypeNameEn = dto.TypeNameEn;
            type.TypeNameEs = dto.TypeNameEs;
            type.TypeNamePt = dto.TypeNamePt;
            type.CodeTemplate = dto.CodeTemplate;
            type.RequiredValidation = dto.RequiredValidation;
            type.DefaultFolder = dto.DefaultFolder;
            type.TypeRole = dto.TypeRole;
            type.Visible = dto.Visible;
            type.UseGrCodeRecogizing = dto.UseGrCodeRecogizing;
            type.DownloadOnlyLastVersion = dto.DownloadOnlyLastVersion;
            type.NeedsEncryption = dto.NeedsEncryption;
            type.TenantId = dto.TenantId;
            type.IndustryId = dto.IndustryId;

            await dbContext.SaveChangesAsync();

            return ToDto(type);
        }

        public async Task<bool> DeleteType(int id)
        {
            var type = await dbContext.GedDocumentTypes.FindAsync(id);

            if (type is null)
            {
                return false;
            }

            dbContext.GedDocumentTypes.Remove(type);
            await dbContext.SaveChangesAsync();
            return true;
        }

        private async Task ClearUnknownDefaultFolder(GedDocumentTypeDto dto)
        {
            if (dto.DefaultFolder is null)
            {
                return;
            }

            var exists = await dbContext.GedDossiers
                .AsNoTracking()
                .AnyAsync(d => d.NumDossier == dto.DefaultFolder);

            if (!exists)
            {
                dto.DefaultFolder = null;
            }
        }

        private static GedDocumentTypeDto ToDto(GedDocumentType type)
        {
            return new GedDocumentTypeDto
            {
                Id = type.Id,
                TypeName = type.TypeName,
                TypeCategory = type.TypeCategory,
                UrlTemplate = type.UrlTemplate,
                TypeNameFr = type.TypeNameFr,
                TypeNameEn = type.TypeNameEn,
                TypeNameEs = type.TypeNameEs,
                TypeNamePt = type.TypeNamePt,
                CodeTemplate = type.CodeTemplate,
                RequiredValidation = type.RequiredValidation,
                DefaultFolder = type.DefaultFolder,
                TypeRole = type.TypeRole,
                Visible = type.Visible,
                UseGrCodeRecogizing = type.UseGrCodeRecogizing,
                DownloadOnlyLastVersion = type.DownloadOnlyLastVersion,
                NeedsEncryption = type.NeedsEncryption,
                TenantId = type.TenantId,
                IndustryId = type.IndustryId
            };
        }
    }
}
