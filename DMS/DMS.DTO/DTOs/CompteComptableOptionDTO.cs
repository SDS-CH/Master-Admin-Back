namespace DMS.DTO.DTOs
{
    /// <summary>
    /// Lightweight option used to populate the "Compte Achat" / "Compte Vente"
    /// dropdowns in the tax modal, filtered by country.
    /// </summary>
    public class CompteComptableOptionDTO
    {
        public string Compte { get; set; }
        public string Designation { get; set; }
    }
}
