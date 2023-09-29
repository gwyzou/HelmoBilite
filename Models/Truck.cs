using System.ComponentModel.DataAnnotations;

namespace HelmoBilite.Models;

public class Truck
{
    [Key] public int Id { get; set; }
    [Display(Name = "Brand")] public TruckBrand Brand { get; set; }

    [Display(Name = "Model")] public string Model { get; set; }

    [Display(Name = "Plate")]
    [RegularExpression(@"^\d{1}-[A-Z]{3}-\d{3}$", ErrorMessage = "Invalid plate format")]
    public string Plate { get; set; }

    [Display(Name = "Type")] public DriverLicence Type { get; set; }

    [Display(Name = "Max Weight (Kg)")] public float MaxWeight { get; set; }

    public string Picture { get; set; }
    public Boolean isAvailable(Delivery toAssign, IEnumerable<Delivery> assignedDeliveries)
    {
        DateTime toAssignStartDate = toAssign.WantedStartDate;
        DateTime toAssignEndDate = toAssign.WantedEndDate;

        foreach (var assignedDelivery in assignedDeliveries)
        {
            DateTime assignedStartDate = assignedDelivery.WantedStartDate;
            DateTime assignedEndDate = assignedDelivery.WantedEndDate.AddHours(1);

            if (IsOverlap(toAssignStartDate, toAssignEndDate, assignedStartDate, assignedEndDate))
            {
                // There is an overlap, return false
                return false;
            }
        }

        // No overlap found, return true
        return true;
    }
    private bool IsOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
    {
        return start1 < end2 && start2 < end1;
    }
}