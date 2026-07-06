#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.EFCore.Repositories
{
    public class TnCodesTaxisRepository : GenericBaseRepository<TnCodesTaxis, DmsReferenceContext>, ITnCodesTaxisRepository<TnCodesTaxis>
    {
        public TnCodesTaxisRepository(DmsReferenceContext context) : base(context)
        {
            dbContext = context;
        }
        // Dans TnCodesTaxisRepository
        public async Task<TnCodesTaxis> GetById(string code)
        {
            return await dbContext.TnCodesTaxes
                .FirstOrDefaultAsync(x => x.CodeTaxe == code);
        }

        public async Task<DataSourceResult> GetAllTnCodesTaxis(DataSourceRequest requestModel, string countryCode = null)
        {
            var query = from taxe in dbContext.TnCodesTaxes
                        join country in dbContext.CountryCodes
                            on taxe.CountryId equals country.Id
                        select new DMS.DTO.DTOs.TnCodesTaxisDTO
                        {
                            CodeTaxe = taxe.CodeTaxe,
                            DescriptionTaxe = taxe.DescriptionTaxe,
                            PourcentageTaxe = taxe.PourcentageTaxe,
                            CompteTaxeAchat = taxe.CompteTaxeAchat,
                            CompteTaxeVente = taxe.CompteTaxeVente,
                            Agence = taxe.Agence,
                            Session = taxe.Session,
                            AddNewTime = taxe.AddNewTime,
                            EditTime = taxe.EditTime,
                            IgnoreAccounting = taxe.IgnoreAccounting,
                            TextPrefixOnNewLine = taxe.TextPrefixOnNewLine,
                            SplitOnNewLine = taxe.SplitOnNewLine,
                            Rubrique = taxe.Rubrique,
                            ApplyOnTaxes = taxe.ApplyOnTaxes,
                            Priorite = taxe.Priorite,
                            ToGroup = taxe.ToGroup,
                            AvailableOnFile = taxe.AvailableOnFile,
                            CodeTisCompta = taxe.CodeTisCompta,
                            ToShowOnDetails = taxe.ToShowOnDetails,
                            Abbreviation = taxe.Abbreviation,
                            TypeTax = taxe.TypeTax,
                            TvaTaxTopology = taxe.TvaTaxTopology,
                            WhTaxTopology = taxe.WhTaxTopology,
                            ToBeDeclareOtherThanTax = taxe.ToBeDeclareOtherThanTax,
                            IsDefault = taxe.IsDefault,
                            IsActive = taxe.IsActive,
                            TenantId = taxe.TenantId,
                            CountryId = taxe.CountryId,
                            CountryName = country.CountryName,
                            CountryCode = country.CountryCode1
                        };

            if (!string.IsNullOrEmpty(countryCode))
            {
                query = query.Where(x => x.CountryCode == countryCode);
            }

            return await query.ToDataSourceResultAsync(requestModel);
        }

        public async Task Delete(string code)
        {
            var entity = await dbContext.TnCodesTaxes
                .FirstOrDefaultAsync(x => x.CodeTaxe == code);

            if (entity != null)
            {
                dbContext.TnCodesTaxes.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        // Accounts available for a country, resolved through:
        // PcComptePlanComptableGroupe -> PcPlanComptableGroupe -> PcPlanComptableEntite.CountryId
        public async Task<List<DMS.DTO.DTOs.CompteComptableOptionDTO>> GetComptesByCountry(int countryId)
        {
            var query = from cpg in dbContext.PcComptePlanComptableGroupes
                        join compte in dbContext.PcCompteComptables on cpg.CompteId equals compte.Id
                        join groupe in dbContext.PcPlanComptableGroupes on cpg.PlanComptableGroupeId equals groupe.Id
                        join entite in dbContext.PcPlanComptableEntites on groupe.PlanComptableEntiteId equals entite.Id
                        where entite.CountryId == countryId
                        select new DMS.DTO.DTOs.CompteComptableOptionDTO
                        {
                            Compte = compte.Compte,
                            Designation = compte.Designation
                        };

            return await query
                .Distinct()
                .OrderBy(x => x.Compte)
                .ToListAsync();
        }
    }
}
