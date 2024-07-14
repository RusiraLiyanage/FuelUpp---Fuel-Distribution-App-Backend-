using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Fuel_App.Models.Shed_Owner_Model
{
    [BsonIgnoreExtraElements]
    public class ShedOwner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("ownerName")]
        public string OwnerName { get; set; } = string.Empty;
        [BsonElement("shedName")]
        public string ShedName { get; set; } = string.Empty;
        [BsonElement("shedLocation")]
        public string ShedLocation { get; set; } = string.Empty;
        [BsonElement("fuelArrivalTime")]
        public string FuelArrivalTime { get; set; } = string.Empty;
        [BsonElement("fuelFinishTime")]
        public string FuelFinishTime { get; set; } = string.Empty;
        [BsonElement("queueLength")]
        public int QueueLength { get; set; }
        [BsonElement("fuelAvaliability")]
        public string FuelAvaliability { get; set; } = string.Empty;
    }
}
