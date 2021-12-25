using System;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Report.Models.Report
{
    public class ReportHead : IEntity
    {
        public Guid ReportHeadId { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime  PreparationDate { get; set; }
        public string Status { get; set; }
        public ReportItem ReportItem { get; set; }

       

    }

    
}
