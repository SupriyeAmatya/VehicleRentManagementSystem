using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NiceApp.Data;
using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;

namespace NiceApp.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {

        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehicleService(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IEnumerable<Vehicle> GetVehicle()
        {
            //IList<Vehicle> userList = new List<Vehicle>();
            //var query = from user in _dbContext.Vehicles
            //            select user;
            //var users = query.ToList();
            //foreach (var userData in users)
            //{
            //    userList.Add(new Vehicle()
            //    {
            //        Id = userData.Id,
            //        VehicleName = userData.VehicleName,
            //        PlateNo = userData.PlateNo,
            //        InitialRentPrice = userData.InitialRentPrice,
            //        RentRatePerHr = userData.RentRatePerHr,
            //        Penalty = userData.Penalty,
            //        Availability = userData.Availability,
            //        VehicleType
            //        VehicleKind
            //        WhereStored
            //        Tracker
            //        Vehicleimages
            //    });
            //}
            var mData = _dbContext.VehicleImages
                .Include(d => d.Vehicle).Select(v => v.Vehicle);



            return mData;
        }
        public async Task AddVehicleAsync(VehicleDTO vehicle)
        {



            try
            {

                using var transaction = _dbContext.Database.BeginTransaction();
                var Data = new Vehicle()
                {
                    Id = vehicle.Id,
                    VehicleName = vehicle.VehicleName,
                    PlateNo = vehicle.PlateNo,
                    InitialRentPrice = vehicle.InitialRentPrice,
                    RentRatePerHr = vehicle.RentRatePerHr,
                    Penalty = vehicle.Penalty,
                    Availability = vehicle.Availability,
                    VehicleType = vehicle.VehicleType,
                    VehicleKind = vehicle.VehicleKind,
                    WhereStored = vehicle.WhereStored,
                    Tracker = vehicle.Tracker

                };
                var result = _dbContext.Vehicles.Add(Data);
                _dbContext.SaveChanges();
                var filePaths = await SaveImages(vehicle.VehicleImages, Data);
             
                foreach (string path in filePaths)
                {
                    _dbContext.VehicleImages.Add(new VehicleImage
                    {
                      
                        Vehiclenumber = vehicle.Id,
                        VehicleImage1 = path
                    });
                }
                await _dbContext.SaveChangesAsync();
                if (result is null)
                {
                 
                }
                transaction.Commit();
                //return result.ToString();
                
            }
            catch(Exception ee)
            {
                
            }

     


        }
        private async Task<List<string>> SaveImages(IFormFileCollection files, Vehicle vehicle)
        {

            string uploadFolder = Path.Combine("uploads", $"{vehicle.VehicleName}_{vehicle.PlateNo}");
            string contentPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadFolder);

            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }

            List<string> filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file != null)
                {
                    var InputFileName = Path.GetFileName(file.FileName);

                    // File paths that will be saved to the database.
                    filePaths.Add(Path.Combine(uploadFolder, InputFileName));

                    var ServerSavePath = Path.Combine(contentPath, InputFileName);
                    //Save file to uploads folder  
                    using (Stream fileStream = new FileStream(ServerSavePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return filePaths;
        }
    }
}
