#nullable disable
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
using System.Threading.Tasks;

namespace DMS.Services.Services
{
    public class ArticleService<TArticleDTO, TArticle, TContext>
        : BaseService<TArticleDTO, TArticle, TContext>, IArticleService<TArticleDTO>
        where TArticle : TnArticle, new()
        where TArticleDTO : ArticleDto
        where TContext : DmsReferenceContext
    {
        private readonly IArticleRepository<TArticle> _repository;
        private readonly IMapper _mapper;

        public ArticleService(TContext dbContext, IMapper mapper, IArticleRepository<TArticle> repository)
            : base(dbContext, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

       
        public async Task<DataSourceResult> GetAllAsync(DataSourceRequest requestModel)
        {
            return await _repository.GetAllAsync(requestModel);
        }

        
        //public async Task<IEnumerable<ArticleDto>> GetAllAsync()
        //{
        //var entities = await _repository.GetAllAsync();
        //return _mapper.Map<IEnumerable<ArticleDto>>(entities);
        //}

        public async Task<IEnumerable<ArticleDto>> GetByIndustryAsync(int industryId)
        {
            var entities = await _repository.GetByIndustryAsync(industryId);
            return _mapper.Map<IEnumerable<ArticleDto>>(entities);
        }

        public async Task AddAsync(ArticleDto dto)
        {
            var cat = await _repository.GetDefaultCategories();



            var entity = _mapper.Map<TArticle>(dto);
            entity.GroupeArticle = cat.Id;


            await _repository.AddAsync(entity);
        }

        public async Task UpdateAsync(ArticleDto dto)
        {
            var entity = _mapper.Map<TArticle>(dto);
            await _repository.UpdateAsync(entity);
        }
        public async Task<TArticleDTO> GetById(int id)
        {
            return await base.GetById(id, _repository);
        }

        public async Task DeleteAsync(int codeArticle)
        {
            await _repository.DeleteAsync(codeArticle);
        }

        public async Task<IEnumerable<ProductServiceCategoryDto>> GetAllCategoriesAsync()
        {
            var entities = await _repository.GetAllCategoriesAsync();
            return _mapper.Map<IEnumerable<ProductServiceCategoryDto>>(entities);
        }
    }
}