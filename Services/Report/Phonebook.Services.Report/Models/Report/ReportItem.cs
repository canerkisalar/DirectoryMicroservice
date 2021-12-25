using System;
using System.ComponentModel.DataAnnotations.Schema;
using Phonebook.Core.Domain;

namespace Phonebook.Services.Report.Models.Report
{
    public class ReportItem : IEntity
    {
        public Guid ReportItemId { get; set; }
        public string Code { get; set; }
        public Guid ReportHeadId { get; set; }
        [ForeignKey("ReportHeadId")]
        public ReportHead ReportHead { get; set; }
    }
}
