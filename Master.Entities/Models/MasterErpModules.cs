#nullable disable
using System;
using System.Collections.Generic;

namespace Master.Entities.Models;

public partial class MasterErpModules
{
    public int Id { get; set; }
    public string ModuleName { get; set; }
    public string IconName { get; set; }
    public string IconStyle { get; set; }
    public string DbPrefix { get; set; }
    public DateTime AddNewTime { get; set; }
    public virtual ICollection<MasterErpModuleClient> MasterErpModuleClient { get; set; } = new List<MasterErpModuleClient>();
}

