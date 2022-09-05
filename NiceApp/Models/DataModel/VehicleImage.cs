using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NiceApp.Models.DataModel
{
    public partial class VehicleImage
    {

     
        public int Id { get; set; }
        public int Vehiclenumber { get; set; }
   
       
        public string VehicleImage1 { get; set; }

        [ForeignKey("Vehiclenumber")]
        [InverseProperty("Vehicleimages")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
