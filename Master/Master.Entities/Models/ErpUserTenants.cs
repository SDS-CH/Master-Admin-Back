#nullable disable
using System;

namespace Master.Entities.Models;

public partial class ErpUserTenants
{
    public int UserId { get; set; }
    public int TenantId { get; set; }
    public DateOnly AddNewTime { get; set; }
    public bool IsActive { get; set; }
    public bool IsErpUser { get; set; }
    public bool IsDefault { get; set; }
    public virtual ErpTenants Tenant { get; set; }
    public virtual ErpUsers User { get; set; }
}


