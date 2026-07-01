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
            services.AddTransient<IDepartmentRepository<Department>, DepartmentRepository>();            services.AddTransient<IDepartmentService<DepartmentDTO>,
                DepartmentService<DepartmentDTO, Department, DmsReferenceContext>>();

            // Translation 
            services.AddTransient<ITranslationRepository, TranslationRepository>();
            services.AddTransient<ITranslationService, TranslationService>();
            services.AddTransient<IDepartmentService<DepartmentDTO>, DepartmentService<DepartmentDTO, Department, DmsReferenceContext>>();    
            
            services.AddTransient<IFileTypeRepository<TnTypesDossier>, FileTypeRepository>();
            services.AddTransient<IFileTypeService<FileTypeDto>, FileTypeService<FileTypeDto, TnTypesDossier, DmsReferenceContext>>();

            // Activity
            services.AddTransient<IActivityRepository<TnActivite>, ActivityRepository>();
            services.AddTransient<IActivityService<ActivityDto>,
                ActivityService<ActivityDto, TnActivite, DmsReferenceContext>>();

            // GedDocumentCategory
            services.AddTransient<IGedDocumentCategoryRepository<GedDocumentCategory>, GedDocumentCategoryRepository>();
            services.AddTransient<IGedDocumentCategoryService<GedDocumentCategoryDto>,
                GedDocumentCategoryService<GedDocumentCategoryDto, GedDocumentCategory, DmsReferenceContext>>();
        }
    }
}