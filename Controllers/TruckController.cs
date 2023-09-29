using HelmoBilite.Data;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelmoBilite.Controllers;

[Authorize(Roles = "Admin")]
public class TruckController : Controller
{
    private readonly ApplicationDbContext _context;

    public TruckController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Truck
    public async Task<IActionResult> Index()
    {
        return _context.Trucks != null
            ? View(await _context.Trucks.ToListAsync())
            : View("Error","Entity set 'ApplicationDbContext.Trucks'  is null.");
    }

    // GET: Truck/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Trucks == null) return View("Error","No trucks found in the database."+
                                                                       "Please contact the webmaster if the problem persists.");


        var truck = await _context.Trucks
            .FirstOrDefaultAsync(m => m.Id == id);
        if (truck == null) return View("Error", "The truck you're looking for doesn't exist." +
                                                "Please contact the webmaster if the problem persists.");

        return View(truck);
    }

    // GET: Truck/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Truck/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Brand,Model,Plate,Type,MaxWeight")] ViewModel.TruckViewModel truckModel)
    {
        if (ModelState.IsValid)
        {
            var truck = new Truck
            {
                Brand = truckModel.Brand,
                Model = truckModel.Model,
                Plate = truckModel.Plate,
                Type = truckModel.Type,
                MaxWeight = truckModel.MaxWeight,
                Picture = "theirTruck.png"
            };
            _context.Add(truck);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(truckModel);
    }

    // GET: Truck/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Trucks == null) return View("Error", "No trucks found in the database." +
                                                                        "Please contact the webmaster if the problem persists.");

        var truck = await _context.Trucks.FindAsync(id);
        if (truck == null) return View("Error", "The truck you're looking for doesn't exist." +
                                                "Please contact the webmaster if the problem persists.");
        
        var truckModel = new ViewModel.TruckViewModel
        {
            Brand = truck.Brand,
            Model = truck.Model,
            Plate = truck.Plate,
            Type = truck.Type,
            MaxWeight = truck.MaxWeight,
        };
        
        return View(truckModel);
    }

    // POST: Truck/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Plate,Type,MaxWeight")] ViewModel.TruckViewModel truckModel)
    {
        var truck = await _context.Trucks.FindAsync(id);

        if (ModelState.IsValid && truck != null)
        {
            try
            {
                
                truck.Brand = truckModel.Brand;
                truck.Model = truckModel.Model;
                truck.Plate = truckModel.Plate;
                truck.Type = truckModel.Type;
                truck.MaxWeight = truckModel.MaxWeight;
                
                _context.Update(truck);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                
                    return View("Error", "an error occured while trying to update date." +
                                         "Please contact the webmaster if the problem persists.");
            }

            return RedirectToAction(nameof(Index));
        }

        return View(truckModel);
    }

    // GET: Truck/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Trucks == null) return View("Error", "No trucks found in the database." +
                                                                        "Please contact the webmaster if the problem persists.");

        var truck = await _context.Trucks
            .FirstOrDefaultAsync(m => m.Id == id);
        if (truck == null) return View("Error","The truck you're looking for doesn't exist." +
                                                "Please contact the webmaster if the problem persists.");

        return View(truck);
    }

    // POST: Truck/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Trucks == null) return View("Error","Entity set 'ApplicationDbContext.Trucks'  is null.");
        var truck = await _context.Trucks.FindAsync(id);
        if (truck != null) _context.Trucks.Remove(truck);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TruckExists(int id)
    {
        return (_context.Trucks?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}