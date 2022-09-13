using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NiceApp.Models.DataModel;

namespace NiceApp.Data
{
    public class AppDbContext : IdentityDbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<RentVehicle> RentVehicles { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}
