using HelmoBilite.Models;
using Microsoft.AspNetCore.Mvc;

namespace HelmoBilite.ViewModel
{
    public class StatisticsViewModel
    {
        public List<Client>? Clients { get; set; }

        public List<Driver>? Drivers { get; set; }
        public List<Delivery>? Delivery { get; set; }

        public string? IdDriver { get; set; }
        public string? IdClient { get; set; }
        public DateTime? WantedStartDate { get; set; }
        public DateTime? WantedEndDate { get; set; }

        public int TotalDeliveries => Delivery?.Count ?? 0;
        public int SuccededDeliveries => Delivery?.Count(d => d.Status == Status.Success) ?? 0;
        public int UnAssignedDeliveries => Delivery?.Count(d => d.Status == Status.Unassigned) ?? 0;
        public int PendingDeliveries => Delivery?.Count(d => d.Status == Status.Pending) ?? 0;
        public int CanceleddDeliveries => Delivery?.Count(d => d.Status == Status.Cancelled) ?? 0;

        
    }
}
