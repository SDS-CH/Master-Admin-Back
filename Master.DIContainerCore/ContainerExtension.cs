#nullable disable
using Master.DTO;
using Master.DTO.Config;
using Master.DTO.DTOs;
using Master.DTO.Users;
using Master.EFCore.Repositories;
using Master.EFCore.Repositories.Config;
using Master.EFCore.Repositories.Users;
using Master.Entities.Models;
using Master.Infrastructure.IRepositories;
using Master.Infrastructure.IRepositories.Config;
using Master.Infrastructure.IRepositories.Users;
using Master.Infrastructure.IServices;
using Master.Infrastructure.IServices.Config;
using Master.Infrastructure.IServices.Users;
using Master.Services.Services;
using Master.Services.Services.Config;
using Master.Services.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Master.DIContainerCore
{
    public static class ContainerExtension
    {
        public static void Initialize(IServiceCollection services, string connectionString)
        {
            // DbContext
            services.AddDbContext<ERPMasterContext>(options =>
                options.UseNpgsql(connectionString, o => o.CommandTimeout(180)));

            // Repositories
            services.AddTransient<IUserRepository<MasterAdminUsers>, UserRepository>();
            services.AddTransient<IDmsUserRepository<MasterAdminUsers>, DmsUserRepository>();
            services.AddTransient<IErpTenantRepository<ErpTenants>, ErpTenantRepository>();
            services.AddTransient<IErpUserRepository<ErpUsers>, ErpUserRepository>();
            services.AddTransient<IErpUserTenantRepository<ErpUserTenants>, ErpUserTenantRepository>();
            services.AddTransient<ICurrencyRepository<Currency>, CurrencyRepository>();
            services.AddTransient<IDbInstanceRepository<DbInstance>, DbInstanceRepository>();
            services.AddTransient<IErpCountryRepository<ErpCountries>, ERPCountryRepository>();
            services.AddTransient<IGedDirectoryRepository<GedDirectory>, GeddirectoryRepository>();
            services.AddTransient<IMasterERPDatabaseRefRepository<MasterErpDatabaseRef>, MasterERPDatabaseRefRepository>();
            services.AddTransient<IMasterERPIndustriesRepository<MasterErpIndustries>, MasterERPIndustriesRepository>();
            services.AddTransient<IMasterERPIndustriesClientRepository<MasterErpIndustriesClient>, MasterERPIndustriesClientRepository>();
            services.AddTransient<IMasterErpmodulesRepository<MasterErpModules>, MasterErpmodulesRepository>();
            services.AddTransient<IMasterErpmoduleClientRepository<MasterErpModuleClient>, MasterErpmoduleClientRepository>();
            services.AddTransient<IRegisterAuthCodeRepository<RegisterAuthCodes>, RegisterAuthCodeRepository>();
            services.AddTransient<IMasterErpglobalConfigRepository<MasterErpGlobalConfigs>, MasterErpglobalConfigRepository>();
            services.AddTransient<IMasterErpmailTemplateRepository<MasterErpMailTemplates>, MasterErpmailTemplateRepository>();

            // Services
            services.AddTransient<IUserService<MasterAdminUsersDTO>, UserService<MasterAdminUsersDTO, MasterAdminUsers, ERPMasterContext>>();
            services.AddTransient<IErpTenantService<ErpTenantDTO>, ErpTenantService<ErpTenantDTO, ErpTenants, ERPMasterContext>>();
            services.AddTransient<IErpUserService<ErpUserDTO>, ErpUserService<ErpUserDTO, ErpUsers, ERPMasterContext>>();
            services.AddTransient<IErpUserTenantService<ErpUserTenantDTO>, ErpUserTenantService<ErpUserTenantDTO, ErpUserTenants, ERPMasterContext>>();
            services.AddTransient<ICurrencyService<CurrencyDTO>, CurrencyService<CurrencyDTO, Currency, ERPMasterContext>>();
            services.AddTransient<IDbInstanceService<DbInstanceDTO>, DbInstanceService<DbInstanceDTO, DbInstance, ERPMasterContext>>();
            services.AddTransient<IErpCountryService<ErpCountryDTO>, ERPCountryService<ErpCountryDTO, ErpCountries, ERPMasterContext>>();
            services.AddTransient<IGedDirectoryService<GedDirectoryDTO>, GedDirectoryService<GedDirectoryDTO, GedDirectory, ERPMasterContext>>();
            services.AddTransient<IMasterERPDatabaseRefService<MasterERPDatabaseRefDTO>, MasterERPDatabaseRefService<MasterERPDatabaseRefDTO, MasterErpDatabaseRef, ERPMasterContext>>();
            services.AddTransient<IMasterERPIndustriesService<MasterERPIndustriesDTO>, MasterERPIndustriesService<MasterERPIndustriesDTO, MasterErpIndustries, ERPMasterContext>>();
            services.AddTransient<IMasterERPIndustriesClientService<MasterERPIndustriesClientDTO>, MasterERPIndustriesClientService<MasterERPIndustriesClientDTO, MasterErpIndustriesClient, ERPMasterContext>>();
            services.AddTransient<IMasterErpmodulesService<MasterErpmodulesDTO>, MasterErpmodulesService<MasterErpmodulesDTO, MasterErpModules, ERPMasterContext>>();
            services.AddTransient<IMasterErpmoduleClientService<MasterErpmoduleClientDTO>, MasterErpmoduleClientService<MasterErpmoduleClientDTO, MasterErpModuleClient, ERPMasterContext>>();
            services.AddTransient<IMasterErpglobalConfigService<MasterErpglobalConfigDTO>, MasterErpglobalConfigService<MasterErpglobalConfigDTO, MasterErpGlobalConfigs, ERPMasterContext>>();
            services.AddTransient<IMasterErpmailTemplateService<MasterErpmailTemplateDTO>, MasterErpmailTemplateService<MasterErpmailTemplateDTO, MasterErpMailTemplates, ERPMasterContext>>();
        }
    }
}
