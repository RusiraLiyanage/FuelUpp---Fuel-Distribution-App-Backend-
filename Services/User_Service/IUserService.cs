using Fuel_App.Models.User;

namespace Fuel_App.Services.User_Service
{
    public interface IUserService
    {
        List<User> Get();
        List<User> GetUsersByFuelShed(string fuel_shed); 
        User Get(string id);
        int GetQueueLength(string shed_name, string vehicle_type);
        int GetTotalQueueLength(string shed_name);
        string AddArrivalTime(User user);
        string ExitBeforePump(string id, User user);
        string ExitAfterPump(string id, User user);
    }
}
