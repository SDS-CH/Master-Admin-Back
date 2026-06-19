#nullable disable
using DMS.DTO.DTOs;
using DMS.Infrastructure.IServices;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService<ArticleDto> _service;

        public ArticleController(IArticleService<ArticleDto> service)
        {
            _service = service;
        }

        // ✅ Kendo
        [HttpPost("getAll")]
        public async Task<IActionResult> GetAllAsync([DataSourceRequest] DataSourceRequest request)
    => Ok(await _service.GetAllAsync(request));

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        // var articles = await _service.GetAllAsync();
        // return Ok(articles);
        //}

        [HttpGet("industry/{industryId}")]
        public async Task<IActionResult> GetByIndustry(int industryId)
        {
            var articles = await _service.GetByIndustryAsync(industryId);
            return Ok(articles);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ArticleDto articleDto)
        {
            articleDto.CodeArticle = 0;
            await _service.AddAsync(articleDto);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ArticleDto articleDto)
        {
            await _service.UpdateAsync(articleDto);
            return Ok();
        }

        [HttpDelete("{codeArticle}")]
        public async Task<IActionResult> Delete(int codeArticle)
        {
            await _service.DeleteAsync(codeArticle);
            return Ok();
        }

        
        [HttpGet("categories/all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _service.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}