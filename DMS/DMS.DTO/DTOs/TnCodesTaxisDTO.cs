using System;

namespace DMS.DTO.DTOs
{
    public class TnCodesTaxisDTO
    {
        public string CodeTaxe { get; set; }
        public string DescriptionTaxe { get; set; }
        public double PourcentageTaxe { get; set; }
        public string CompteTaxeAchat { get; set; }
        public string CompteTaxeVente { get; set; }
        public string Agence { get; set; }
        public int Session { get; set; }
        public DateTime AddNewTime { get; set; }
        public DateTime EditTime { get; set; }
        public bool? IgnoreAccounting { get; set; }
        public string TextPrefixOnNewLine { get; set; }
        public bool? SplitOnNewLine { get; set; }
        public string Rubrique { get; set; }
        public bool ApplyOnTaxes { get; set; }
        public int? Priorite { get; set; }
        public bool? ToGroup { get; set; }
        public bool? AvailableOnFile { get; set; }
        public string CodeTisCompta { get; set; }
        public bool? ToShowOnDetails { get; set; }
        public string Abbreviation { get; set; }
        public string TypeTax { get; set; }
        public string TvaTaxTopology { get; set; }
        public string WhTaxTopology { get; set; }
        public bool? ToBeDeclareOtherThanTax { get; set; }
        public bool? IsDefault { get; set; }
        public bool? IsActive { get; set; }
        public Guid TenantId { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public string? CountryCode { get; set; }
    }
}
