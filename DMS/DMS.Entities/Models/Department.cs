#nullable disable
using System;

namespace DMS.Entities.Models;

public partial class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DepartmentColor { get; set; }
    public DateTime? EditNewTime { get; set; }
    public Guid TenantId { get; set; }
    public int? IndustryId { get; set; }
}
