﻿using System;

namespace Directory.Services.Phonebook.Dtos.Contact
{
    public class ContactDeleteDto
    {
        public Guid PhonebookId { get; set; }

        public Guid ContactId { get; set; }

    }
}
