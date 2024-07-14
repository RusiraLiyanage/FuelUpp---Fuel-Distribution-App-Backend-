using Fuel_App.Models.Shed_Owner_Model;
using Fuel_App.Models.User;

namespace Fuel_App.Services.Shed_Owner_Service
{
    public interface IShedOwnerService
    {
        List<ShedOwner> Get();
        ShedOwner RetriewShedDetails(string shed_name);
        string RegisterPetrolShed(ShedOwner shed_owner);
        string UpdateFuelData(string shed_name, ShedOwner shed_owner);
        string updateQueueLength(string shed_name, int action);

    }
}
