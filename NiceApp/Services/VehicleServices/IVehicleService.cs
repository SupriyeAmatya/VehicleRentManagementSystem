using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;

namespace NiceApp.Services.VehicleServices
{
    public interface IVehicleService
    {
        IEnumerable<VehicleImage> GetVehicle();
       Task AddVehicleAsync(VehicleDTO vehicle);
    }
}
