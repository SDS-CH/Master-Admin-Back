using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Master.Common.Classes.EFCore
{
    public abstract class GenericBaseRepository<TType, TContext> : GenericRepository<TContext>, IGenericBaseRepository<TType>
        where TType : class
        where TContext : DbContext
    {
        protected GenericBaseRepository(TContext context) : base(context)
        {
        }

        public virtual async Task<TType> GetById(object id)
        {
            try
            {
                var set = dbContext.Set<TType>();
                var entityType = dbContext.Model.FindEntityType(typeof(TType));
                var primaryKey = entityType?.FindPrimaryKey();

                if (primaryKey == null)
                    return await set.FindAsync(id);

                var keyValues = BuildKeyValues(id, primaryKey.Properties);
                if (keyValues != null)
                    return await set.FindAsync(keyValues);

                if (primaryKey.Properties.Count == 1 && TryConvertKeyValue(id, primaryKey.Properties[0].ClrType, out var singleKey))
                    return await set.FindAsync(singleKey);

                if (primaryKey.Properties.Count > 1)
                    return await QueryByPrimaryKeyAsync(set, primaryKey.Properties[0], id);

                return null;
            }
            finally
            {
                SetDefaultDbContext();
            }
        }

        public virtual async Task<List<TType>> GetAll()
        {
            var result = await dbContext.Set<TType>().ToListAsync();
            SetDefaultDbContext();
            return result;
        }

        public virtual async Task<DataSourceResult> GetAll(DataSourceRequest requestModel)
        {
            try
            {
                var groups = requestModel.Groups;
                requestModel.Groups = null;
                var res = await dbContext.Set<TType>().ToDataSourceResultAsync(requestModel);
                requestModel.Groups = groups;
                return res;
            }
            catch
            {
                return null;
            }
        }

        public virtual async Task<TType> Create(TType obj)
        {
            GetContext().Entry(obj).State = EntityState.Added;
            try
            {
                await GetContext().SaveChangesAsync();
            }
            catch (Exception e) when (e is DbUpdateException)
            {
                throw e.InnerException ?? e;
            }
            SetDefaultDbContext();
            return obj;
        }

        public virtual async Task<TType> Edit(TType obj)
        {
            try
            {
                var set = dbContext.Set<TType>();
                var entry = dbContext.Entry(obj);
                if (entry.State == EntityState.Detached)
                {
                    var entityType = dbContext.Model.FindEntityType(typeof(TType));
                    var key = entityType?.FindPrimaryKey();
                    if (key == null)
                    {
                        set.Attach(obj);
                        entry = dbContext.Entry(obj);
                    }
                    else
                    {
                        var local = set.Local.FirstOrDefault(localEntity =>
                        {
                            var localEntry = dbContext.Entry(localEntity);
                            foreach (var keyProp in key.Properties)
                            {
                                var localValue = localEntry.Property(keyProp.Name).CurrentValue;
                                var newValue = entry.Property(keyProp.Name).CurrentValue;
                                if (!Equals(localValue, newValue)) return false;
                            }
                            return true;
                        });

                        if (local != null)
                        {
                            dbContext.Entry(local).CurrentValues.SetValues(obj);
                            entry = dbContext.Entry(local);
                        }
                        else
                        {
                            set.Attach(obj);
                            entry = dbContext.Entry(obj);
                        }
                    }
                }
                entry.State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e) when (e is DbUpdateException)
            {
                throw e.InnerException ?? e;
            }
            SetDefaultDbContext();
            return obj;
        }

        public virtual async Task<int> Delete(object id)
        {
            var entity = await GetById(id);
            if (entity != null)
                GetContext().Set<TType>().Remove(entity);
            SetDefaultDbContext();
            return await GetContext().SaveChangesAsync();
        }

        public virtual async Task<List<TType>> GenericGet(Expression<Func<TType, bool>> filter = null)
        {
            IQueryable<TType> query = dbContext.Set<TType>();
            if (filter != null)
                query = query.Where(filter);
            SetDefaultDbContext();
            return await query.ToListAsync();
        }

        public virtual async Task<DataSourceResult> GenericGetDataSourceResult(DataSourceRequest request, Expression<Func<TType, bool>> filter = null)
        {
            IQueryable<TType> query = dbContext.Set<TType>();
            if (filter != null)
                query = query.Where(filter);
            SetDefaultDbContext();
            return await query.ToDataSourceResultAsync(request);
        }

        public virtual async Task<TType> GenericGetFirstOrDefaultAsync(Expression<Func<TType, bool>> filter = null)
        {
            IQueryable<TType> query = dbContext.Set<TType>();
            if (filter != null)
                query = query.Where(filter);
            SetDefaultDbContext();
            try
            {
                return await query.FirstOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        private object[] BuildKeyValues(object id, IReadOnlyList<IProperty> keyProperties)
        {
            if (id == null || keyProperties == null || keyProperties.Count == 0) return null;

            if (id is object[] array)
            {
                if (array.Length != keyProperties.Count) return null;
                var normalized = new object[array.Length];
                for (var i = 0; i < array.Length; i++)
                {
                    if (!TryConvertKeyValue(array[i], keyProperties[i].ClrType, out var converted)) return null;
                    normalized[i] = converted;
                }
                return normalized;
            }

            if (keyProperties.Count == 1 && TryConvertKeyValue(id, keyProperties[0].ClrType, out var single))
                return new[] { single };

            return null;
        }

        private async Task<TType> QueryByPrimaryKeyAsync(IQueryable<TType> set, IProperty keyProperty, object idValue)
        {
            if (!TryConvertKeyValue(idValue, keyProperty.ClrType, out var converted)) return null;
            var parameter = Expression.Parameter(typeof(TType), "entity");
            var propertyAccess = Expression.Property(parameter, keyProperty.Name);
            var constant = Expression.Constant(converted, keyProperty.ClrType);
            var equality = Expression.Equal(propertyAccess, constant);
            var predicate = Expression.Lambda<Func<TType, bool>>(equality, parameter);
            return await set.FirstOrDefaultAsync(predicate);
        }

        private static bool TryConvertKeyValue(object value, Type targetType, out object converted)
        {
            converted = null;
            var nonNullable = Nullable.GetUnderlyingType(targetType) ?? targetType;
            if (value == null) return !nonNullable.IsValueType;
            if (nonNullable.IsInstanceOfType(value)) { converted = value; return true; }
            try
            {
                if (nonNullable == typeof(Guid))
                {
                    if (value is string guidText && Guid.TryParse(guidText, out var guid)) { converted = guid; return true; }
                    return false;
                }
                converted = Convert.ChangeType(value, nonNullable);
                return true;
            }
            catch { return false; }
        }
    }
}
