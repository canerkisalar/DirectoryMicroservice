using System;

namespace Phonebook.Services.Report.Dtos
{
    public class ReportHeadDto 
    {
        public Guid ReportHeadId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime PreparationDate { get; set; }
        public string Status { get; set; }
    }
}
