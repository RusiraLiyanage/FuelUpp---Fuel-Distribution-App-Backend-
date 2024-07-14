using Fuel_App.Models.Shed_Owner_Model;
using Fuel_App.Models.User;
using MongoDB.Driver;

namespace Fuel_App.Services.Shed_Owner_Service
{
    public class ShedOwnerService : IShedOwnerService
    {
        private readonly IMongoCollection<ShedOwner> _shedOwner;

        public ShedOwnerService(IShedOwnerDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _shedOwner = database.GetCollection<ShedOwner>(settings.ShedOwnerCollectionName);
        }
        public List<ShedOwner> Get()
        {
            return _shedOwner.Find(shed_owner => true).ToList();
        }

        public string RegisterPetrolShed(ShedOwner shed_owner)
        {
            _shedOwner.InsertOne(shed_owner);
            return $"Petrol Shed: {shed_owner.ShedName} was registerd successfully";
        }

        public ShedOwner RetriewShedDetails(string shed_name)
        {
            return _shedOwner.Find(shed_owner => shed_owner.ShedName == shed_name).FirstOrDefault();
        }

        public string UpdateFuelData(string shed_name, ShedOwner shed_owner)
        {
            _shedOwner.ReplaceOne(shed_owner => shed_owner.ShedName == shed_name, shed_owner);
            return $"Fuel data of the Shed: {shed_owner.ShedName} was updated successfully";
        }

        public string updateQueueLength(string shed_name, int action)
        {
            int[] the_accepted = { 1, -1 };
            if(!the_accepted.Contains(action))
            {
                return $"Please enter only -1 or 1";
            }
            ShedOwner shedOwner = _shedOwner.Find(shed_owner => shed_owner.ShedName == shed_name).FirstOrDefault();
            // incrementing or decrementing the Queue Length of a specific ShedName
            shedOwner.QueueLength += action;
            // updating the object with updated QueueLength value
            _shedOwner.ReplaceOne(shed_owner => shed_owner.ShedName == shed_name, shedOwner);
            if (action == -1)
            {
                return $"Fuel Queue Length of the Shed: {shed_name} was decremented successfully";
            }
            if(action == 1)
            {
                return $"Fuel Queue Length of the Shed: {shed_name} was incremented successfully";
            }
            return "Something went wrong";
        }
    }
}
