using System;

namespace Phonebook.Core.Messages.Report
{
    public class AllPhonebooksMessageCommand
    {
        public Guid ReportId { get; set; }
        public string PhonebookAllData { get; set; }
    }
}
