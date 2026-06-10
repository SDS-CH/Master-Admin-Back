namespace Master.DTO.DTOs
{
    public class MasterERPDatabaseRefDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UrlSourceMdf { get; set; }
        public string UrlSourceLdf { get; set; }
        public string UrlDestinationMdf { get; set; }
        public string UrlDestinationLdf { get; set; }
        public int? DbInstanceId { get; set; }
        public string DbInstanceName { get; set; }
    }
}
