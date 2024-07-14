using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Fuel_App.Models.User
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("arrivalTime")]
        public string ArrivalTime { get; set; } = string.Empty;
        [BsonElement("vehicleType")]
        public string VehicleType { get; set; } = string.Empty;
        [BsonElement("petrolShedName")]
        public string PetrolShedName { get; set; } = string.Empty;

        [BsonElement("departTime")]
        public string DepartTime { get; set; } = string.Empty;
        [BsonElement("exitTimeBeforePumping")]
        public string ExitTimeBeforePumping { get; set; } = string.Empty;
    }
}
