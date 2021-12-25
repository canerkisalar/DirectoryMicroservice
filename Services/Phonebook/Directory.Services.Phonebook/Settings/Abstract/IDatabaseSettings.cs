namespace Phonebook.Services.Phonebook.Settings.Abstract
{
    public interface IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PhonebookCollectionName { get; set; }

    }
}
