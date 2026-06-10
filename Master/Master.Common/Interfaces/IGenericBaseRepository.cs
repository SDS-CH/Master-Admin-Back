using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Master.Common.Interfaces
{
    public interface IGenericBaseRepository<TType> where TType : class
    {
        Task<TType> GetById(object id);
        Task<List<TType>> GetAll();
        Task<DataSourceResult> GetAll(DataSourceRequest request);
        Task<TType> Create(TType obj);
        Task<TType> Edit(TType obj);
        Task<int> Delete(object id);
        Task<List<TType>> GenericGet(Expression<Func<TType, bool>> filter = null);
        Task<DataSourceResult> GenericGetDataSourceResult(DataSourceRequest request, Expression<Func<TType, bool>> filter = null);
        Task<TType> GenericGetFirstOrDefaultAsync(Expression<Func<TType, bool>> filter = null);
    }
}
