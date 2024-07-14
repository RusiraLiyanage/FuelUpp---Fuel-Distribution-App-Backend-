using Fuel_App.Models.User;
using MongoDB.Driver;
namespace Fuel_App.Services.User_Service;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _user;

    public UserService(IUserDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _user = database.GetCollection<User>(settings.UserCollectionName);
    }
    public string AddArrivalTime(User user)
    {
        _user.InsertOne(user);
        return $"User: {user.Id} was arrived to the queue";
    }

    public string ExitAfterPump(string id, User user)
    {
        _user.ReplaceOne(user => user.Id == id, user);
        return $"User: {user.Id} was exited after the fuel pumping";
    }

    public string ExitBeforePump(string id, User user)
    {
        _user.ReplaceOne(user => user.Id == id, user);
        return $"User: {user.Id} was exited before the fuel pumping";
    }

    public List<User> Get()
    {
        return _user.Find(user => true).ToList();
    }

    public User Get(string id)
    {
        return _user.Find(user => user.Id == id ).FirstOrDefault();
    }

    public int GetQueueLength(string shed_name, string vehicle_type)
    {
        var result = _user.Find(user => user.PetrolShedName == shed_name && user.VehicleType == vehicle_type && user.DepartTime == "null" && user.ExitTimeBeforePumping == "null").ToList().Count;
        return result;
    }

    public int GetTotalQueueLength(string shed_name)
    {
        var result = _user.Find(user => user.PetrolShedName == shed_name && user.DepartTime == "null" && user.ExitTimeBeforePumping == "null").ToList().Count;
        return result;
    }

    public List<User> GetUsersByFuelShed(string fuel_shed)
    {
        var result = _user.Find(user => user.PetrolShedName == fuel_shed).ToList();
        return result;
    }
}
