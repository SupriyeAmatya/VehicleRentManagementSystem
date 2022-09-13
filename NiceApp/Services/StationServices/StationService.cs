using Microsoft.EntityFrameworkCore;
using NiceApp.Data;
using NiceApp.Models.DataModel;

namespace NiceApp.Services.StationServices
{
    public class StationService:IStationService
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StationService(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IEnumerable<Station> GetVehicle()
        {
            var mData = _dbContext.Stations;



            return mData;
        }

    }
}
