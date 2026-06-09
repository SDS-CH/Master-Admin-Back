#nullable disable
using System;

namespace Master.Entities.Models;

public partial class ErpCountries
{
    public int Id { get; set; }
    public string CountryCode { get; set; }
    public string CountryName { get; set; }
    public string Nationality { get; set; }
    public string CountryFlagUrl { get; set; }
    public string DefaultCurrency { get; set; }
    public string DefaultLanguage { get; set; }
    public string DefaultEquivalenceCurrency { get; set; }
    public bool IsActive { get; set; }
}

