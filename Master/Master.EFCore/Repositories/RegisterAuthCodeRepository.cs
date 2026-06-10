#nullable disable
using Master.Common.Classes.EFCore;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Master.EFCore.Repositories
{
    public class RegisterAuthCodeRepository : GenericBaseRepository<RegisterAuthCodes, ERPMasterContext>, IRegisterAuthCodeRepository<RegisterAuthCodes>
    {
        public RegisterAuthCodeRepository(ERPMasterContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<RegisterAuthCodes> GetByEmail(string email)
        {
            return await dbContext.RegisterAuthCodes
                .Where(r => r.Email.ToLower() == email.ToLower() && r.IsUsed == false)
                .OrderByDescending(r => r.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
