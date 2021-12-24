using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory.Core.Domain;

namespace Directory.Core.Messages.DataCapture
{
    public class SnapPhonebookMessageCommand
    {
        public SnapPhonebookMessageCommand()
        {
            Contacts = new List<Contact>();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public List<Contact> Contacts { get; set; }

        public int Modification { get; set; }
    }
    public class Contact : IEntity
    {
        public Guid Id { get; set; }
        public string ContactType { get; set; }
        public string ContactInformation { get; set; }
    }
}
