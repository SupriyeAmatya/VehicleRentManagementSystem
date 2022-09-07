using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;

namespace NiceApp.Services.VehicleServices
{
    public interface IVehicleService
    {
        IEnumerable<Vehicle> GetVehicle();
       Task AddVehicleAsync(VehicleDTO vehicle);
        Vehicle CompleteDetails(int? id);
        void DeleteVehicle(int userId);
        Vehicle GetVehicleById(int userId);
    }
}
