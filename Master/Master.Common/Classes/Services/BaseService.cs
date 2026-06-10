using AutoMapper;
using Master.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Master.Common.Classes.Services
{
    public abstract class BaseService<TDto, TEntity, TContext>
        where TDto : class
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext _dbContext;
        protected readonly IMapper _mapper;

        protected BaseService(TContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        protected async Task<TDto> GetById(int id, IGenericBaseRepository<TEntity> repo)
        {
            var entity = await repo.GetById(id);
            return _mapper.Map<TDto>(entity);
        }

        protected async Task<TEntity> Create(TDto dto, IGenericBaseRepository<TEntity> repo)
        {
            var entity = _mapper.Map<TEntity>(dto);
            return await repo.Create(entity);
        }

        protected async Task<TEntity> Edit(TDto dto, IGenericBaseRepository<TEntity> repo)
        {
            var entity = _mapper.Map<TEntity>(dto);
            return await repo.Edit(entity);
        }

        protected async Task<int> Delete(int id, IGenericBaseRepository<TEntity> repo)
        {
            return await repo.Delete(id);
        }
    }
}
