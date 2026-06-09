#nullable disable
using System;
using System.Collections.Generic;

namespace Master.Entities.Models;

public partial class ErpTenants
{
    public int Id { get; set; }
    public Guid TenantId { get; set; }
    public string EntityName { get; set; }
    public DateOnly AddNewTime { get; set; }
    public int? DbInstanceId { get; set; }
    public virtual DbInstance DbInstance { get; set; }
    public virtual ICollection<ErpUserTenants> ErpUserTenants { get; set; } = new List<ErpUserTenants>();
    public virtual ICollection<MasterErpIndustriesClient> MasterErpIndustriesClient { get; set; } = new List<MasterErpIndustriesClient>();
    public virtual ICollection<MasterErpModuleClient> MasterErpModuleClient { get; set; } = new List<MasterErpModuleClient>();
    public virtual TenantAgencyProfile TenantAgencyProfile { get; set; }
}


