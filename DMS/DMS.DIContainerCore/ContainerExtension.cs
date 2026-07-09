#nullable disable
using DMS.Application.Services;
using DMS.DTO.DTOs;
using DMS.EFCore.Repositories;
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using DMS.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace DMS.DIContainerCore
{
    public static class ContainerExtension
    {
        public static void Initialize(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DmsReferenceContext>(options =>
                options.UseNpgsql(connectionString, o => o.CommandTimeout(180)));
            // Article
            services.AddTransient<IArticleRepository<TnArticle>, ArticleRepository>();
            services.AddTransient<IArticleService<ArticleDto>,
                ArticleService<ArticleDto, TnArticle, DmsReferenceContext>>();
            // Department
            services.AddTransient<IDepartmentRepository<Department>, DepartmentRepository>();
            services.AddTransient<IDepartmentService<DepartmentDTO>,
                DepartmentService<DepartmentDTO, Department, DmsReferenceContext>>();
            // Translation 
            services.AddTransient<ITranslationRepository, TranslationRepository>();
            services.AddTransient<ITranslationService, TranslationService>();
            services.AddTransient<IDepartmentService<DepartmentDTO>, DepartmentService<DepartmentDTO, Department, DmsReferenceContext>>();
            // FileType
            services.AddTransient<IFileTypeRepository<TnTypesDossier>, FileTypeRepository>();
            services.AddTransient<IFileTypeService<FileTypeDto>, FileTypeService<FileTypeDto, TnTypesDossier, DmsReferenceContext>>();
            // Regime
            services.AddTransient<IRegimeRepository<TnCodesRegime>, RegimeRepository>();
            services.AddTransient<IRegimeService<RegimeDto>, RegimeService<RegimeDto, TnCodesRegime, DmsReferenceContext>>();
            // Activities
            services.AddTransient<IActivityRepository<TnActivite>, ActivityRepository>();
            services.AddTransient<IActivityService<ActivityDto>, ActivityService<ActivityDto, TnActivite, DmsReferenceContext>>();
            // CustomFields
            services.AddTransient<ICustomFieldRepository<TnCodesComplementsDossier>, CustomFieldRepository>();
            services.AddTransient<ICustomFieldService<CustomFieldDto>, CustomFieldService<CustomFieldDto, TnCodesComplementsDossier, DmsReferenceContext>>();

            // GedDocumentCategory
            services.AddTransient<IGedDocumentCategoryRepository<GedDocumentCategory>, GedDocumentCategoryRepository>();
            services.AddTransient<IGedDocumentCategoryService<GedDocumentCategoryDto>,
                GedDocumentCategoryService<GedDocumentCategoryDto, GedDocumentCategory, DmsReferenceContext>>();

            // File Type Milestones
            services.AddTransient<IFileTypeMilestonesRepository<TnCodesEtape>, FileTypeMilestonesRepository>();
            services.AddTransient<IFileTypeMilestonesService<MilestoneStepDto>,
                FileTypeMilestonesService<MilestoneStepDto, TnCodesEtape, DmsReferenceContext>>();

            // File Type Document Types
            services.AddTransient<IFileTypeDocumentTypeRepository<GedDocumentType>, FileTypeDocumentTypeRepository>();
            services.AddTransient<IFileTypeDocumentTypeService<GedDocumentTypeDto>,
                FileTypeDocumentTypeService<GedDocumentTypeDto, GedDocumentType, DmsReferenceContext>>();
            // TnCodesTaxis (VAT / TVA)
            services.AddTransient<ITnCodesTaxisRepository<TnCodesTaxis>, TnCodesTaxisRepository>();
            services.AddTransient<ITnCodesTaxisService<TnCodesTaxisDTO>,
                TnCodesTaxisService<TnCodesTaxisDTO, TnCodesTaxis, DmsReferenceContext>>();
        }
    }
}