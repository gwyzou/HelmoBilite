using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace HelmoBilite.Models;

public class Driver : HelmoMember
{
    [Display(Name = "Driver License")]
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public DriverLicence DriverLicense { get; set; }

    public Boolean isAvailable(Delivery toAssign, IEnumerable <Delivery> assignedDeliveries)
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

    public Boolean canDrive(Truck truck)
    {
        //return true; //TODO Change to verify if the driver can drive the truck
        return truck.Type <= DriverLicense;
    }
}
