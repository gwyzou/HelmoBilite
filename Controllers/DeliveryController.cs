using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelmoBilite.Data;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using HelmoBilite.ViewModel;
using System.Diagnostics;
using Microsoft.CodeAnalysis.Differencing;

namespace HelmoBilite.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly UserManager<BasicUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DeliveryController(UserManager<BasicUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Delivery
        [Authorize(Roles = "Admin, Dispatcher, Driver, Client")]
        public async Task<IActionResult> Index()
        {
            var connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");

            }
            List<Delivery>? connectedDeliveries = null;

            if (User.IsInRole("Admin"))
            {
                connectedDeliveries = await _context.Deliveries
                    .Include(d => d.Client)
                    .ToListAsync();
            }
            if (User.IsInRole("Dispatcher"))
            {
                connectedDeliveries = await _context.Deliveries
                    .Include(d => d.Client)
                    .Include(d => d.Driver)
                    .ToListAsync();
            }
            if (User.IsInRole("Driver"))
            {
                connectedDeliveries = await _context.Deliveries
                    .Where(d => d.Driver != null && d.Driver.Id == connected.Id)
                    .Include(d => d.Client)
                    .ToListAsync();
            }
            if (User.IsInRole("Client"))
            {
                connectedDeliveries = await _context.Deliveries
                    .Where(d => d.Client.Id == connected.Id)
                    .Include(d => d.Client)
                    .ToListAsync();
            }
            if (connectedDeliveries == null)
            {
                return View("Error", "No deliveries found for this user. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            connectedDeliveries
                .Sort((d1, d2) => d1.WantedStartDate.CompareTo(d2.WantedStartDate));
            return _context.Deliveries != null ?
                        View(connectedDeliveries) :
                        View("Error", "No deliveries were found. " +
                                      "Please contact the webmaster if the problem persists.");
        }

        [Authorize(Roles = "Admin, Dispatcher")]
        public async Task<IActionResult> AllWaitingAssignation()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            List<Delivery>? waitingAssignation =
                await _context.Deliveries
                    .Where(d => d.Status == Status.Unassigned)
                    .Where(d => d.Driver == null)
                    .Where(d => d.Truck == null)
                    .Include(d => d.Client)
                    //.Where(d => d.Client.Tag == Tag.GoodPayment)
                    .ToListAsync();


            waitingAssignation
                .Sort((d1, d2) => d1.WantedStartDate.CompareTo(d2.WantedStartDate));
            return View(waitingAssignation);
        }
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> AllDoneAssignation()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            List<Delivery>? waitingAssignation =
                await _context.Deliveries
                    .Where(d => d.Status != Status.Unassigned)
                    .Where(d => d.Driver != null)
                    .Where(d => d.Truck != null)
                    .Include(d => d.Client)
                    //.Where(d => d.Client.Tag == Tag.GoodPayment)
                    .ToListAsync();


            waitingAssignation
                .Sort((d1, d2) => d1.WantedStartDate.CompareTo(d2.WantedStartDate));
            return View(waitingAssignation);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllWaitingToBeDone()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            List<Delivery>? waitingAssignation =
                await _context.Deliveries
                    .Where(d => d.Status == Status.Pending)
                    .Where(d => d.Driver != null)
                    .Where(d => d.Truck != null)
                    .Include(d => d.Client)
                    //.Where(d => d.Client.Tag == Tag.GoodPayment)
                    .ToListAsync();


            waitingAssignation
                .Sort((d1, d2) => d1.WantedStartDate.CompareTo(d2.WantedStartDate));
            return View(waitingAssignation);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllDone()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            List<Delivery>? waitingAssignation =
                await _context.Deliveries
                    .Where(d => d.Status == Status.Success || d.Status == Status.Cancelled)
                    .Where(d => d.Driver != null)
                    .Where(d => d.Truck != null)
                    .Include(d => d.Client)
                    //.Where(d => d.Client.Tag == Tag.GoodPayment)
                    .ToListAsync();


            waitingAssignation
                .Sort((d1, d2) => d1.WantedStartDate.CompareTo(d2.WantedStartDate));
            return View(waitingAssignation);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientAllPending()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            List<Delivery>? connectedPendingDeliveries =
                await _context.Deliveries
                    .Where(d => d.Client.Id == connected.Id)
                    .Where(d => d.Status == Status.Unassigned || d.Status == Status.Pending)
                    .ToListAsync();
            return View(connectedPendingDeliveries);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> ClientAllTerminated()
        {
            BasicUser? connected = await _userManager.GetUserAsync(User);
            if (connected == null)
            {
                return View("Error", "User not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            List<Delivery>? connectedPendingDeliveries =
                await _context.Deliveries
                    .Where(d => d.Client.Id == connected.Id)
                    .Where(d => d.Status == Status.Cancelled || d.Status == Status.Success)
                    .ToListAsync();
            return View(connectedPendingDeliveries);
        }


        // GET: Delivery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return View("Error", "Delivery not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            var delivery = await _context.Deliveries.Include(d => d.Client).FirstOrDefaultAsync(d => d.Id == id);
            if (User.IsInRole("Client"))
            {
                var user = await _userManager.GetUserAsync(User);

                var verif = VerifyClient(delivery, user);
                if (verif != null)
                {
                    return verif;
                }
            }
            if (delivery == null)
            {
                return View("Error" , "Delivery not found. " +
                                      "Please contact the webmaster if the problem persists.");
            }

            return View(delivery);
        }

        // GET: Delivery/Create
        [Authorize(Roles = "Client")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create([Bind("Weight,WantedStartDate,WantedEndDate,PickUpAddress,DeliveryAddress,Content")] DeliveryViewModel deliveryModel)
        {
            if (ModelState.IsValid)
            {
                Delivery delivery = new Delivery
                {
                    Weight = deliveryModel.Weight,
                    WantedStartDate = deliveryModel.WantedStartDate,
                    WantedEndDate = deliveryModel.WantedEndDate,
                    PickUpAddress = deliveryModel.PickUpAddress,
                    DeliveryAddress = deliveryModel.DeliveryAddress,
                    Content = deliveryModel.Content,
                    Status = Status.Unassigned,
                    Client = (Client)await _userManager.GetUserAsync(User)
                };
                _context.Add(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deliveryModel);
        }

        // GET: Delivery/Edit/5
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return View("Error","Delivery not found or entity set 'ApplicationDbContext.Deliveries' is null." +
                                     "Please contact the webmaster if the problem persists.");
            }

            
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Client).FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyClient(delivery, user);
            
            if(delivery.Status != Status.Unassigned)
            {
                return View("Error", "Delivery cannot be edited because it is already assigned to a driver.");
            }
            
            if (verif != null)
            {
                return verif;
            }
            
            var viewModel = new DeliveryViewModel
            {
                Weight = delivery.Weight,
                WantedStartDate = delivery.WantedStartDate,
                WantedEndDate = delivery.WantedEndDate,
                PickUpAddress = delivery.PickUpAddress,
                DeliveryAddress = delivery.DeliveryAddress,
                Content = delivery.Content
            };
            return View(viewModel);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Edit(int id,
            [Bind("Weight,WantedStartDate,WantedEndDate,PickUpAddress,DeliveryAddress,Content")]
            DeliveryViewModel delivery)
        {
            var user = await _userManager.GetUserAsync(User);
            var deliveryToUpdate =
                await _context.Deliveries.Include(d => d.Client).FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyClient(deliveryToUpdate, user);
            if (verif != null)
            {
                return verif;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    deliveryToUpdate.Weight = delivery.Weight;
                    deliveryToUpdate.WantedStartDate = delivery.WantedStartDate;
                    deliveryToUpdate.WantedEndDate = delivery.WantedEndDate;
                    deliveryToUpdate.PickUpAddress = delivery.PickUpAddress;
                    deliveryToUpdate.DeliveryAddress = delivery.DeliveryAddress;
                    deliveryToUpdate.Content = delivery.Content;

                    _context.Update(deliveryToUpdate);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryExists(id))
                    {
                        return View("Error", "Delivery not found. " +
                                             "Please contact the webmaster if the problem persists.");
                    }
                    else
                    {
                        throw;
                    }

                }
            }

            return View(delivery);

        }

        // GET: Delivery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Deliveries == null)
            {
                return View("Error","Delivery not found or entity set 'ApplicationDbContext.Deliveries' is null." +
                                     "Please contact the webmaster if the problem persists.");
            }
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Client)
                .Where(d => d.Status == Status.Unassigned)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyClient(delivery, user);
            if (verif != null)
            {
                return verif;
            }
            
            return View(delivery);
        }

        // POST: Delivery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Deliveries == null)
            {
                return View("Error","Delivery not found or entity set 'ApplicationDbContext.Deliveries' is null." +
                                     "Please contact the webmaster if the problem persists.");
            }
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Client)
                .Where(d=>d.Status == Status.Unassigned)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyClient(delivery, user);
            if (verif != null)
            {
                return verif;
            }
            
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        private bool DeliveryExists(int? id)
        {
            return (_context.Deliveries?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> Assign(int? id)
        {
            if (id == null || _context.Deliveries == null || _context.Driver == null)
            {
                return View("Error", "Delivery not found. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            var deliveryToAssign = await _context.Deliveries
                .Where(d => d.Status == Status.Unassigned)
                .FirstOrDefaultAsync(d=> d.Id == id);
            if (deliveryToAssign == null)
            {
                return View("Error", "The delivery you are trying to assign does not exist. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            DateTime startDate = deliveryToAssign.WantedStartDate.AddHours(-1);
            DateTime endDate = deliveryToAssign.WantedEndDate;
            float deliveryWeight = deliveryToAssign.Weight;

            List<Truck> availableTrucks = await _context.Trucks
                .Where(truck =>
                !_context.Deliveries.Any(delivery =>
                    (delivery.Truck != null) &&
                    (delivery.Truck.Id == truck.Id) &&
                    ((delivery.WantedStartDate <= startDate && delivery.WantedEndDate >= startDate) ||
                         (delivery.WantedStartDate <= endDate && delivery.WantedEndDate <= endDate))
                ) &&
                deliveryWeight <= truck.MaxWeight
            )
            .ToListAsync();



            List<Driver> availableDrivers = await _context.Driver
                .Where(driver =>
                    !_context.Deliveries.Any(delivery =>
                        (delivery.Driver != null) &&
                        (delivery.Driver.Id == driver.Id) &&
                        ((delivery.WantedStartDate <= startDate && delivery.WantedEndDate >= startDate) ||
                         (delivery.WantedStartDate <= endDate && delivery.WantedEndDate >= endDate))
                    )
                )
                .ToListAsync();
            availableDrivers.Sort((d1, d2) => d1.DriverLicense.CompareTo(d2.DriverLicense));
            availableTrucks.Sort((d1, d2) => d1.Type.CompareTo(d2.Type));

            return View(new DeliveryWithAssignation(availableDrivers, availableTrucks, deliveryToAssign));
        }

        
        [HttpPost, ActionName("Assign")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Dispatcher")]
        public async Task<IActionResult> Assign(int DeliveryId, int TruckId, string DriverId)
        {
            if (_context.Deliveries == null || _context.Driver == null || _context.Trucks == null)
            {
                
                return View("Error","Entity set 'ApplicationDbContext.Deliveries' or 'ApplicationDbContext.Driver' or 'ApplicationDbContext.Trucks' is null.");
            }

            var delivery = await _context.Deliveries.FindAsync(DeliveryId);
            var driver = await _context.Driver.FindAsync(DriverId);
            var truck = await _context.Trucks.FindAsync(TruckId);

            if (delivery == null || driver == null || truck == null)
            {
                TempData["ErrorString"] = "Please Select All Fields";
                return RedirectToAction(nameof(Assign), DeliveryId);

            }
            var deliveriesOfDriver = await _context.Deliveries.Where(delivery => delivery.Driver != null && delivery.Driver.Id == driver.Id).ToListAsync();
            var deliveriesOfTruck = await _context.Deliveries.Where(delivery => delivery.Truck != null && delivery.Truck.Id == truck.Id).ToListAsync();

            if (driver.canDrive(truck) && driver.isAvailable(delivery, deliveriesOfDriver) && truck.isAvailable(delivery, deliveriesOfTruck))
            {

                delivery.assign(driver, truck);
                _context.Update(delivery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorString"] = "An Error Occured, Possible error : truck or driver was not aviable or driver couldn't drive selected truck";
            return RedirectToAction(nameof(Assign), DeliveryId);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics()
        {
            if (_context.Deliveries == null || _context.Driver == null || _context.Client == null)
            {
                return View("Error", "There are either no deliveries, drivers or clients in the database. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            var deliveries = await _context.Deliveries
                .ToListAsync();
            var drivers = await _context.Driver.ToListAsync();
            var clients = await _context.Client.ToListAsync();

            return View(new StatisticsViewModel{
                Clients= clients,Drivers= drivers,Delivery= deliveries });

        }
        [HttpPost, ActionName("Statistics")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Statistics(string? DriverId , string? ClientId, DateTime? WantedStartDate, DateTime? WantedEndDate)
        {
            if(_context.Deliveries == null || _context.Driver == null || _context.Client == null)
            {
                return View("Error", "There are either no deliveries, drivers or clients in the database. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            
            var deliveries = await _context.Deliveries
                .Include(d => d.Driver)
                .Include(d => d.Client)
                .ToListAsync();


            if(DriverId != null)
            {
                deliveries = deliveries.FindAll(delivery =>delivery.Driver!=null && delivery.Driver.Id == DriverId);
            }
            if (ClientId != null)
            {
                deliveries = deliveries.FindAll(delivery => delivery.Client != null && delivery.Client.Id == ClientId);
            }
            if (WantedStartDate != null)
            {
                deliveries = deliveries.FindAll(delivery => delivery.WantedStartDate >= WantedStartDate);
            }
            if (WantedEndDate != null)
            {
                deliveries = deliveries.FindAll(delivery => delivery.WantedEndDate <= WantedEndDate);
            }

            var drivers = await _context.Driver.ToListAsync();
            var clients = await _context.Client.ToListAsync();

            return View(new StatisticsViewModel
            {
                Clients = clients,
                Drivers = drivers,
                Delivery = deliveries,

                IdDriver = DriverId,
                IdClient = ClientId,
                WantedStartDate = WantedStartDate,
                WantedEndDate = WantedEndDate
            });

        }




        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> ConfirmDelivery(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Driver)
                .Where(d => d.Status == Status.Pending)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyDriver(delivery, user);
            if (verif != null)
            {
                return verif;
            }

            return View(delivery);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost, ActionName("ConfirmDelivery")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelivery(int? id, string Comment)
        {
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Driver)
                .Where(d => d.Status == Status.Pending)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyDriver(delivery, user);
            if (verif != null)
            {
                return verif;
            }
            delivery.Complete(Comment);

            _context.Update(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> CancelDelivery(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries
                .Include(d => d.Driver)
                .Where(d => d.Status == Status.Pending)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyDriver(delivery, user);
            if (verif != null)
            {
                return verif;
            }

            return View(delivery);
        }

        [Authorize(Roles = "Driver")]
        [HttpPost, ActionName("CancelDelivery")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelDelivery(int? id, CancelReason cancelReason)
        {
            var user = await _userManager.GetUserAsync(User);
            var delivery = await _context.Deliveries.Include(d => d.Driver)
                .Where(d => d.Status == Status.Pending)
                .FirstOrDefaultAsync(d => d.Id == id);
            var verif = VerifyDriver(delivery, user);
            if (verif != null)
            {
                return verif;
            }
            delivery.Cancel(cancelReason);
            _context.Update(delivery);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Driver")]

        public async Task<IActionResult> Calendar()
        {
            var user = await _userManager.GetUserAsync(User);
            var usersDeliveries = await _context.Deliveries
                .Include(d => d.Driver)
                .Include(d => d.Client)
                .Where(d => d.Driver != null && d.Driver.Id == user.Id)
                .ToListAsync();
            

            
            return View(usersDeliveries);
        }

        
        private IActionResult? VerifyClient(Delivery? delivery, BasicUser client)
        {
            if (delivery == null)
            {
                return View("Error", "The delivery you are trying to access does not exist. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            if (delivery.Client==null || delivery?.Client.Id != client.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new {area = "Identity"});
            }
            return null;
        }
        private IActionResult? VerifyDriver(Delivery? delivery, BasicUser user)
        {
            if (delivery == null)
            {
                return View("Error", "The delivery you are trying to access does not exist. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            if (delivery.Driver==null|| delivery?.Driver.Id != user.Id)
            {
                return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
            }
            return null;
        }

    }
    
}
