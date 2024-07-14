namespace Fuel_App.Models.Shed_Owner_Model
{
    public class ShedOwnerDatabaseSettings: IShedOwnerDatabaseSettings
    {
        public string ShedOwnerCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
    }
}
