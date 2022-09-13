using NiceApp.Models.DataModel;

namespace NiceApp.Services.UserhomeService
{
    public interface IUserhomeService
    {
        public List<string> GetAllVehicle();
        public IEnumerable<Vehicle> GetAllVehicleFromType(string VehicleType);
        public IEnumerable<Station> GetAllStation();
        public IEnumerable<Vehicle> GetAllVehicleFromStation(int StationName);
    }
}
