#nullable disable
using DMS.Entities.Models;
using DMS.Infrastructure.IRepositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Master.Common.Classes.EFCore;
using Microsoft.EntityFrameworkCore;
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

        public Task Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
