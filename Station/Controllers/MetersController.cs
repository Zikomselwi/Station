using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Station.Data;
using Station.Models;

namespace Water_sayan1.Controllers
{
    [Authorize(Roles = clsRole.roleadmin)]

    public class MetersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MetersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Meters.Include(m => m.Item);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> DeleteAll()
        {
            var allMeters = await _context.Meters.ToListAsync();
            _context.Meters.RemoveRange(allMeters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Meters == null)
            {
                return NotFound();
            }

            var meter = await _context.Meters
                .Include(m => m.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }

            return View(meter);
        }

        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,numberMeter,price,dateTime,ItemId")] Meter meter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", meter.ItemId);
            return View(meter);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Meters == null)
            {
                return NotFound();
            }

            var meter = await _context.Meters.FindAsync(id);
            if (meter == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", meter.ItemId);
            return View(meter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,numberMeter,price,dateTime,ItemId")] Meter meter)
        {
            if (id != meter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeterExists(meter.Id))
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
            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name", meter.ItemId);
            return View(meter);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meters == null)
            {
                return NotFound();
            }

            var meter = await _context.Meters
                
                .FirstOrDefaultAsync(m => m.Id == id);
            _context.Meters.RemoveRange(meter);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool MeterExists(int id)
        {
          return (_context.Meters?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
