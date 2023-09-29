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
using System.Data;

namespace HelmoBilite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
            
        }
        
        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return _context.Client != null ? 
                          View(await  _context.Client
                                .Where(c => _context.Deliveries.Any(d =>  d.Client.Id == c.Id))
                                .ToListAsync()) :
                          View("Error","Entity set 'ApplicationDbContext.Client'  is null. "+
                                       "Please contact the webmaster if the problem persists.");
        }
        

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.Client == null)
            {
                return View("Error", "The client you are looking for does not exist." +
                                     "Please contact the webmaster if the problem persists.");
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return View("Error", "The client you are looking for does not exist." +
                                     "Please contact the webmaster if the problem persists.");            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Client == null)
            {
                return View("Error", "There is no client in the database. " +
                                     "Please contact the webmaster if the problem persists.");
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return View("Error", "The client you are looking for does not exist." +
                                     "Please contact the webmaster if the problem persists.");
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string? id, Tag? Tag)
        {
            if(id == null || _context.Client == null)
            {
                return View("Error", "There is no client in the database. " +
                                     "Please contact the webmaster if the problem persists.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return View("Error", "The client you are looking for does not exist." +
                                     "Please contact the webmaster if the problem persists.");
            }
            if(client.Tag != Tag)
            {
                client.Tag = Tag;
                
                _context.Update(client);
                await _context.SaveChangesAsync();
            }   
            
            return RedirectToAction(nameof(Index));
        }
    }
}
