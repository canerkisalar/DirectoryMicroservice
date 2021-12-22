using Directory.Services.Phonebook.Settings.Abstract;

namespace Directory.Services.Phonebook.Settings.Concrete
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PhonebookCollectionName { get; set; }
    }
}
