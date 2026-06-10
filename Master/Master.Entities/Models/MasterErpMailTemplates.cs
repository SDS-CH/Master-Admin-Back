#nullable disable
using System;

namespace Master.Entities.Models;

public partial class MasterErpMailTemplates
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Label { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public string MailFrom { get; set; }
    public string Signature { get; set; }
    public string ToEmail { get; set; }
    public DateTime AddNewTime { get; set; }
}

