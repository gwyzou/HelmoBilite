using Bogus.DataSets;
using HelmoBilite.Models;
using System.ComponentModel.DataAnnotations;

namespace HelmoBilite.ViewModel
{
    public class TruckViewModel
    {
        [Display(Name = "Brand")] public TruckBrand Brand { get; set; }

        [Display(Name = "Model")] public string Model { get; set; }

        [Display(Name = "Plate")]
        [RegularExpression(@"^\d{1}-[A-Z]{3}-\d{3}$", ErrorMessage = "Invalid plate format")]
        public string Plate { get; set; }

        [Display(Name = "Type")] public DriverLicence Type { get; set; }

        [Display(Name = "Max Weight (Kg)")] public float MaxWeight { get; set; }

        public string? Picture { get; set; }
    }
}
