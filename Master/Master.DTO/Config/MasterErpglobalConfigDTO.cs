namespace Master.DTO.Config
{
    public class MasterErpglobalConfigDTO
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
        public bool? RequirePasswordComplexity { get; set; }
        public int? MinPasswordLength { get; set; }
        public bool? RequireUpperCase { get; set; }
        public bool? RequireLowerCase { get; set; }
        public bool? RequireDigits { get; set; }
        public bool? RequireSpecialCharacters { get; set; }
        public bool? HasResetPasswordEveryXdays { get; set; }
        public int? ResetPasswordEveryXdays { get; set; }
        public bool? HasM2f { get; set; }
        public bool? M2falwaysAsk { get; set; }
        public int? M2ffrequencyXdays { get; set; }
        public bool? M2fuserCanControl { get; set; }
        public int? M2factiveTime { get; set; }
        public int? M2fnumberOfTries { get; set; }
        public int? AccessTokenExpiration { get; set; }
        public int? NumberOfRegenerateCode { get; set; }
        public string FrontDirectoryPath { get; set; }
        public bool? HasCaptcha { get; set; }
        public int? ShowCaptchaLoginAfterX { get; set; }
    }
}
