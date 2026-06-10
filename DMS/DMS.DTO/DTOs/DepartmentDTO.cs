namespace DMS.DTO.DTOs
{
    public class DepartmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentColor { get; set; }
        public DateTime? EditNewTime { get; set; }
        public Guid TenantId { get; set; }
        public int? IndustryId { get; set; }
    }
}
