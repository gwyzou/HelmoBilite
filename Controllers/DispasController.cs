using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelmoBilite.Data;
using HelmoBilite.Models;
using Microsoft.AspNetCore.Authorization;

namespace HelmoBilite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DispasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DispasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Dispas
        public async Task<IActionResult> Index()
        {
              return _context.Dispa != null ? 
                          View(await _context.Dispa.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Dispa'  is null.");
        }

        // GET: Dispas/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Dispa == null)
            {
                return NotFound();
            }

            var dispa = await _context.Dispa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dispa == null)
            {
                return NotFound();
            }

            return View(dispa);
        }

        // GET: Dispas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dispas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Diploma,Mat,LastName,FirstName,Id,ProfilePicture")] Dispa dispa)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dispa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dispa);
        }

        // GET: Dispas/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Dispa == null)
            {
                return NotFound();
            }

            var dispa = await _context.Dispa.FindAsync(id);
            if (dispa == null)
            {
                return NotFound();
            }
            return View(dispa);
        }

        // POST: Dispas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Diploma,Mat,LastName,FirstName,Id,ProfilePicture")] Dispa dispa)
        {
            if (id != dispa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dispa);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DispaExists(dispa.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dispa);
        }

        // GET: Dispas/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Dispa == null)
            {
                return NotFound();
            }

            var dispa = await _context.Dispa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dispa == null)
            {
                return NotFound();
            }

            return View(dispa);
        }

        // POST: Dispas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dispa == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Dispa'  is null.");
            }
            var dispa = await _context.Dispa.FindAsync(id);
            if (dispa != null)
            {
                _context.Dispa.Remove(dispa);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DispaExists(string id)
        {
          return (_context.Dispa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}