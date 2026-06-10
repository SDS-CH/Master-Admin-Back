namespace Master.DTO
{
    public class MasterERPIndustriesClientDTO
    {
        public int Id { get; set; }
        public string IndustryCode { get; set; }
        public int TenantId { get; set; }
    }

    public class MasterERPIndustriesClientExtra : MasterERPIndustriesClientDTO
    {
        public string IndustryLabel { get; set; }
    }
}
