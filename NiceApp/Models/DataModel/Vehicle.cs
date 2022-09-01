using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiceApp.Models.DataModel
{

    public partial class Vehicle
    {
        public Vehicle()
        {
            Vehicleimages = new HashSet<VehicleImage>();
        }
        [Key]
        public int Id { get; set; }
      
        public string VehicleName { get; set; }
        public string PlateNo { get; set; }
        public string InitialRentPrice { get; set; }
        public string RentRatePerHr { get; set; }
        public string Penalty { get; set; }
        public bool Availability { get; set; }
        public string VehicleType { get; set; }
        public string VehicleKind { get; set; }
        public string WhereStored { get; set; }
        public string Tracker { get; set; }
        [NotMapped]
        public string VehicleImage { get; set; }

        [NotMapped]
        [InverseProperty("Vehicle")]
        public virtual ICollection<VehicleImage> Vehicleimages { get; set; }
    }
}
