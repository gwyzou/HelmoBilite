using HelmoBilite.Models;

namespace HelmoBilite.ViewModel;

public class DeliveryWithAssignation
{
    public DeliveryWithAssignation(List<Driver> drivers, List<Truck> trucks, Delivery delivery)
    {
        Drivers = drivers;
        Trucks = trucks;
        Delivery = delivery;
    }
    
    public List<Driver> Drivers { get; set; }
    public List<Truck> Trucks { get; set; }
    public Delivery Delivery { get; set; }


}