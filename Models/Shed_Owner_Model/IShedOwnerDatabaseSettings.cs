namespace Fuel_App.Models.Shed_Owner_Model
{
    public interface IShedOwnerDatabaseSettings
    {
        string ShedOwnerCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
