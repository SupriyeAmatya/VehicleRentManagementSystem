using NiceApp.Data;
using NiceApp.Models.DataModel;

namespace NiceApp.Services.UserhomeService
{
    public class UserhomeService: IUserhomeService
    {

        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserhomeService(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Station> GetAllStation()
        {
            var result = _dbContext.Stations;



            return result;
        }

        public List<string> GetAllVehicle()
        {
            

            var result = _dbContext.Vehicles.Select(m => m.VehicleName).Distinct().ToList();
         


            return result;
        }

        public IEnumerable<Vehicle> GetAllVehicleFromStation(int StationName)
        {
            var result = (from v in _dbContext.Vehicles
                         join s in _dbContext.Stations
                         on v.WhereStored equals s.StationName
                         select new Vehicle
                         {
                             Id = s.Id,
                             VehicleName = v.VehicleName
                         }).Where(a=>a.Id == StationName).AsEnumerable();

          


            return result;
        }

        public IEnumerable<Vehicle> GetAllVehicleFromType(string VehicleType)
        {
            var result = _dbContext.Vehicles.Where(a => a.VehicleType == VehicleType);

           


            return result;
        }
    }
}
