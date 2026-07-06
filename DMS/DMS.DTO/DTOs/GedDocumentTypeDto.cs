namespace DMS.DTO.DTOs
{
    public class GedDocumentTypeDto
    {
        public int Id { get; set; }
        public string? TypeName { get; set; }
        public int TypeCategory { get; set; }
        public string? UrlTemplate { get; set; }
        public string? TypeNameFr { get; set; }
        public string? TypeNameEn { get; set; }
        public string? TypeNameEs { get; set; }
        public string? TypeNamePt { get; set; }
        public string? CodeTemplate { get; set; }
        public bool? RequiredValidation { get; set; }
        public string? DefaultFolder { get; set; }
        public string? TypeRole { get; set; }
        public bool Visible { get; set; } = true;
        public bool? UseGrCodeRecogizing { get; set; }
        public bool? DownloadOnlyLastVersion { get; set; }
        public bool? NeedsEncryption { get; set; }
        public Guid TenantId { get; set; }
        public int? IndustryId { get; set; }
    }
}
