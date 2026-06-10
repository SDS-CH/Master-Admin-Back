using System;

namespace Master.DTO
{
    public class MasterERPIndustriesDTO
    {
        public int Id { get; set; }
        public string CodeIndustry { get; set; }
        public string LabelIndustry { get; set; }
        public string DescriptionIndustry { get; set; }
        public int Session { get; set; }
        public DateTime? AddNewTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
