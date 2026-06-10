#nullable disable
using System;

namespace Master.Entities.Models;

public partial class MasterErpGlobalConfigs
{
    public int Id { get; set; }
    public string EnvironmentName { get; set; }
    public string TemporaryPhysicalPath { get; set; }
    public string VirtualDirectoryName { get; set; }
    public string ZipLibraryPath { get; set; }
    public string GlobalSmtpAccount { get; set; }
    public string GlobalSmtpUser { get; set; }
    public string GlobalSmtpPasswordHash { get; set; }
    public int GlobalSmtpPort { get; set; }
    public string GlobalDfsUserId { get; set; }
    public string GlobalDfsUserPassword { get; set; }
    public string GlobalDfsUserName { get; set; }
    public int? GlobalDfsUserRole { get; set; }
    public string GlobalDfsUserAddress { get; set; }
    public string GlobalDfsUserDomainName { get; set; }
    public string GlobalDfsUserGroupName { get; set; }
    public string GlobalDfsPath { get; set; }
    public int? GlobalDfsPort { get; set; }
    public string GlobalDfsMainUrl { get; set; }
    public string DmsPhysicalPath { get; set; }
    public string DmsVirtualDirectoryName { get; set; }
    public bool? RequirePasswordComplexity { get; set; }
    public int? MinPasswordLength { get; set; }
    public bool? RequireUpperCase { get; set; }
    public bool? RequireLowerCase { get; set; }
    public bool? RequireDigits { get; set; }
    public bool? RequireSpecialCharacters { get; set; }
    public bool? HasResetPasswordEveryXDays { get; set; }
    public int? ResetPasswordEveryXDays { get; set; }
    public bool? HasM2f { get; set; }
    public bool? M2fAlwaysAsk { get; set; }
    public int? M2fFrequencyXDays { get; set; }
    public bool? M2fUserCanControl { get; set; }
    public int? M2fActiveTime { get; set; }
    public int? M2fNumberOfTries { get; set; }
    public int? AccessTokenExpiration { get; set; }
    public int? NumberOfRegenerateCode { get; set; }
    public string FrontDirectoryPath { get; set; }
    public DateTime AddNewTime { get; set; }
    public bool? HasCaptcha { get; set; }
    public int? ShowCaptchaLoginAfterX { get; set; }
}

