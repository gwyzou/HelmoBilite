using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelmoBilite.Models;

public class Delivery
{
    /*
     * > date (début + fin)
> lieu (départ + arrivé)
> float poid
> Client demandeur
> Truck camion 
> Driver chauffeur
> Status status
> string comment
     */
    [Key] public int Id { get; set; }
    
    public Truck? Truck { get; set; }
    public Driver? Driver { get; set; }
    public Client Client { get; set; }
    
    [Display(Name = "Weight")]
    public float Weight { get; set; }
    [Display(Name = "Delivery Start Date")]
    public DateTime WantedStartDate { get; set; }
    [Display(Name = "Delivery End Date")]
    public DateTime WantedEndDate { get; set; }
    [Display(Name = "From")]
    public string PickUpAddress { get; set; }
    [Display(Name = "To")]
    public string DeliveryAddress { get; set; }
    
    [Display(Name = "Content")]
    public string Content { get; set; }
    
    [Display(Name = "Status")]
    public Status Status { get; set; }
    
    [Display(Name = "Comment")]
    [Column(TypeName = "nvarchar(300)")]
    public string? Comment { get; set; }

    public CancelReason? CancelReason { get; set; }

    public void EditDrivabilityOfDriver(Driver driver)
    {
        if (driver != null && Driver != null && driver.Id == Driver.Id && Truck != null && Status == Status.Pending && !driver.canDrive(Truck))
        {
            Driver = null;
            Truck = null;
            Status = Status.Unassigned;
        }
    }
    public void Complete(string? comment)
    {
        Status = Status.Success;
        Comment = comment;
    }
    public void Cancel(CancelReason reason)
    {
        Status = Status.Cancelled;
        CancelReason = reason;
    }
    public void assign(Driver driver, Truck truck)
    {
        Driver = driver;
        Truck = truck;
        Status = Status.Pending;
    }
}