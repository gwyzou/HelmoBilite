using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelmoBilite.Data;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Authorization;


namespace HelmoBilite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
              return _context.Driver != null ? 
                          View(await _context.Driver.ToListAsync()) :
                          View("Error","No driver found in database."+
                                       "Please contact the webmaster if the problem persists.");
                                       
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Driver == null)
            {
                return View("Error", "No driver found in database." +
                                       "Please contact the webmaster if the problem persists.");
            }

            var driver = await _context.Driver
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return View("Error", "The Driver you are looking for does not exist." +
                                       "Please contact the webmaster if the problem persists.");
            }

            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Driver == null)
            {
                return View("Error", "No driver found in database." +
                                       "Please contact the webmaster if the problem persists.");
            }

            var driver = await _context.Driver.FindAsync(id);
            if (driver == null)
            {
                return View("Error", "The Driver you are looking for does not exist." +
                                       "Please contact the webmaster if the problem persists.");
            }
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("DriverLicense,Mat,LastName,FirstName,Id,ProfilePicture")] Driver driver)
        {
            if (id != driver.Id || _context.Driver ==null)
            {
                return View("Error", "No driver found in database." +
                                       "Please contact the webmaster if the problem persists.");
            }
            var driverOfDB = await _context.Driver.FirstOrDefaultAsync(s => s.Id == id);
            if(driverOfDB == null)
            {
                return View("Error", "The Driver you are looking for does not exist." +
                                       "Please contact the webmaster if the problem persists.");
            }
            if(driverOfDB.DriverLicense > driver.DriverLicense)
            {
                var deliveries = await _context.Deliveries.Where(s => s.Driver != null && s.Driver.Id == id)
                    .Include(s => s.Driver)
                    .Include(s => s.Truck)
                    .Include(s=>s.Client)
                    .ToListAsync();
                foreach (var delivery in deliveries)
                {
                    delivery.EditDrivabilityOfDriver(driver);
                    _context.Update(delivery);
                }
                
            }

            driverOfDB.DriverLicense = driver.DriverLicense;
            _context.Update(driverOfDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
     
        }
    }
}
