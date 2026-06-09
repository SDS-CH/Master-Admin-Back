using Microsoft.EntityFrameworkCore;

namespace Master.Common.Classes.EFCore
{
    public abstract class GenericRepository<TContext> where TContext : DbContext
    {
        protected TContext dbContext;
        protected TContext defaultDbContext;

        protected GenericRepository(TContext context)
        {
            dbContext = context;
            defaultDbContext = context;
        }

        protected TContext GetContext() => dbContext;

        public void SetDbContext(DbContext ctx)
        {
            if (ctx is TContext typed)
            {
                defaultDbContext = dbContext;
                dbContext = typed;
            }
        }

        public void SetDefaultDbContext()
        {
            if (defaultDbContext != null)
                dbContext = defaultDbContext;
        }
    }
}
