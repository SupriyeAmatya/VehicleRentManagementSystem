﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NiceApp.Data;
using NiceApp.DTO;
using NiceApp.Models.DataModel;
using NiceApp.Models.DTO;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;

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
            var mData = _dbContext.Vehicles
                .Include(d => d.Vehicleimages);



            return mData;
        }
        public IEnumerable<StationDto> GetAllStation()
        {
            var result = (from v in _dbContext.Stations
                          select new Station
                          {
                              StationName = v.StationName
                          }).AsEnumerable();

            var asd = _dbContext.Stations
                .Select(s => new StationDto{
                    StationId =  s.Id,
                    StationName = s.StationName
                }); 
            return asd;
        }
        public async Task AddVehicleAsync(VehicleDTO vehicle)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
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

                        Vehiclenumber = Data.Id,
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
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
        }



        public Vehicle CompleteDetails(int? id)
        {

            var allVehicleList = _dbContext.Vehicles
         .Include(d => d.Vehicleimages)
         .SingleOrDefault(m => m.Id == id);

            return allVehicleList;
        }
        public Vehicle GetVehicleById(int userId)
        {
            //var query = from u in _dbContext.Vehicles
            //            where u.Id == userId
            //            select u;
            //var vehicle = query.FirstOrDefault();
            //var model = new Vehicle()
            //{
            //    Id = userId,
            //    VehicleName = vehicle.VehicleName,
            //    PlateNo = vehicle.PlateNo,
            //    InitialRentPrice = vehicle.InitialRentPrice,
            //    RentRatePerHr = vehicle.RentRatePerHr,
            //    Penalty = vehicle.Penalty,
            //    Availability = vehicle.Availability,
            //    VehicleType = vehicle.VehicleType,
            //    VehicleKind = vehicle.VehicleKind,
            //    WhereStored = vehicle.WhereStored,
            //    Tracker = vehicle.Tracker
            //};
            //return model;
            var allVehicleList = _dbContext.Vehicles
      .Include(d => d.Vehicleimages)
      .SingleOrDefault(m => m.Id == userId);
            var oldname = allVehicleList.VehicleName;
            return allVehicleList;
        }
        public void DeleteVehicle(int userId)
        {
            Vehicle vehicle = _dbContext.Vehicles
                              .Include(d => d.Vehicleimages)
                              .SingleOrDefault(m => m.Id == userId);
            var vehicleimg = _dbContext.VehicleImages.Where(u => u.Vehiclenumber == userId);
            DeleteImages(vehicle.VehicleName, userId);
            _dbContext.Vehicles.Remove(vehicle);

            foreach (var vehicleI in vehicleimg)
            {
                _dbContext.VehicleImages.Remove(vehicleI);
            }
            _dbContext.SaveChangesAsync();
        }
        public void UpdateVehicle(Vehicle vehicle, object oldname)
        {
            Vehicle userData = _dbContext.Vehicles.Where(u => u.Id == vehicle.Id).SingleOrDefault();
            object von = oldname;
            userData.VehicleName = vehicle.VehicleName;
            userData.PlateNo = vehicle.PlateNo;
            userData.InitialRentPrice = vehicle.InitialRentPrice;
            userData.RentRatePerHr = vehicle.RentRatePerHr;
            userData.Penalty = vehicle.Penalty;
            userData.Availability = vehicle.Availability;
            userData.VehicleType = vehicle.VehicleType;
            userData.VehicleKind = vehicle.VehicleKind;
            userData.WhereStored = vehicle.WhereStored;
            userData.Tracker = vehicle.Tracker;

            MoveImages(oldname,vehicle);
            _dbContext.SaveChanges();
        }
        private void MoveImages(object von, Vehicle vehicle) {
            string olduploadFolder = Path.Combine("uploads", $"{von}_{vehicle.Id}");
            string oldcontentPath = Path.Combine(_webHostEnvironment.WebRootPath, olduploadFolder);
            string newuploadFolder = Path.Combine("uploads", $"{vehicle.VehicleName}_{vehicle.Id}");
            string newcontentPath = Path.Combine(_webHostEnvironment.WebRootPath, newuploadFolder);
            if (!Directory.Exists(newcontentPath))
            {
                var mFiles = Directory.EnumerateFiles(oldcontentPath);
                Directory.CreateDirectory(newcontentPath);
            
                foreach (var imageFile in mFiles)
                {
                    var newImageName = Path.GetFileName(imageFile);
                    File.Copy(imageFile, Path.Combine(newcontentPath, newImageName));

                }
            }
        }
        private void DeleteImages(string VehicleName, int Id)
        {

            string uploadFolder = Path.Combine("uploads", $"{VehicleName}_{Id}");
            string contentPath = Path.Combine(_webHostEnvironment.WebRootPath, uploadFolder);

            if (Directory.Exists(contentPath))
            {
                var mFiles = Directory.EnumerateFiles(contentPath);

                foreach (var imageFile in mFiles)
                {
                    File.Delete(Path.Combine(contentPath, imageFile));
                    
                }
                Directory.Delete(contentPath);
                
            }
        }
        private async Task<List<string>> SaveImages(IFormFileCollection files, Vehicle vehicle)
        {
            Guid foldername = new Guid();
            string uploadFolder = Path.Combine("uploads", $"{vehicle.VehicleName}_{vehicle.Id}");
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
                    Guid filename = new Guid();
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
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create); //create FileStream,this will finally be used to create the new image 
            Compress(srcBitMap, s, level);  //main progress to compress image
            s.Close();
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myImageCodecInfo = GetEncoderInfo("image/jpeg");
            myEncoder = Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }
    }
}
