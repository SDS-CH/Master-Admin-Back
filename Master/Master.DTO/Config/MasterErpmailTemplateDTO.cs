namespace Master.DTO.Config
{
    public class MasterErpmailTemplateDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailFrom { get; set; }
        public string Signature { get; set; }
        public string ToEmail { get; set; }
    }
}
