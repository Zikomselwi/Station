using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Station.Data;
using Station.Models;
using Station.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Station.Controllers
{

    public class ReadingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReadingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = clsRole.roleauser + "," + clsRole.roleadmin)]
        public IActionResult Index()
        {
            IEnumerable<Reading> itemslist = _context.Readings.Include(x => x.Meter).Include(a => a.Item).ToList();
            return View(itemslist);
        }
        [Authorize(Roles = clsRole.roleauser + "," + clsRole.roleadmin)]

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ReadingViewModel
            {
                Items = _context.Items.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),

             

                Meters = new List<SelectListItem>()
            };
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Readings == null)
            {
                return NotFound();
            }

            var reading = await _context.Readings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reading == null)
            {
                return NotFound();
            }

            return View(reading);
        }
        [Authorize(Roles = clsRole.roleauser + "," + clsRole.roleadmin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReadingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var latestReading = await _context.Readings
                    .Where(r => r.MeterId == viewModel.MeterId && r.ItemId == viewModel.PointId)
                    .OrderByDescending(r => r.dateTime)
                    .FirstOrDefaultAsync();

                if (latestReading != null && viewModel.CurrentRead < latestReading.CurrentRead)
                {
                   
                    ModelState.AddModelError("CurrentRead", "القراءة الحالية لا يمكن أن تكون أقل من القراءة السابقة.");
                }
                else
                {
                    var reading = new Reading
                    {
                        CurrentRead = viewModel.CurrentRead,
                        dateTime = (DateTime)viewModel.Created,
                        MeterId = viewModel.MeterId,
                        ItemId = viewModel.PointId
                    };

                    _context.Add(reading);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // Reload the dropdown lists in case of validation failure
            viewModel.Items = _context.Items.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            viewModel.Meters = _context.Meters.Where(m => m.ItemId == viewModel.PointId).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.numberMeter.ToString()
            }).ToList();

            return View(viewModel);
        }


        public IActionResult GetMeters(int pointId)
        {
            var meters = _context.Meters
                .Where(m => m.ItemId == pointId)
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.numberMeter.ToString()
                }).ToList();

            return Json(meters);
        }



        [Authorize(Roles = clsRole.roleadmin)]
        public async Task<IActionResult> Edit(int id)
        {
            var reading = await _context.Readings.FindAsync(id);

            if (reading == null)
            {
                return NotFound();
            }

            var viewModel = new ReadingViewModel
            {
                Id = reading.Id,
                CurrentRead = reading.CurrentRead,
                Created = reading.dateTime,
                MeterId = reading.MeterId,
                PointId = reading.ItemId,
                Items = _context.Items.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),
                Meters = _context.Meters.Where(m => m.ItemId == reading.ItemId).Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.numberMeter.ToString()
                }).ToList()
            };

            return View(viewModel);
        }


    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = clsRole.roleadmin)]

        public async Task<IActionResult> Edit(int id, ReadingViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var reading = await _context.Readings.FindAsync(id);
                if (reading == null)
                {
                    return NotFound();
                }

                reading.CurrentRead = viewModel.CurrentRead;
                reading.dateTime = (DateTime)viewModel.Created;
                reading.MeterId = viewModel.MeterId;
                reading.ItemId = viewModel.PointId;

                try
                {
                    _context.Update(reading);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Readings.Any(e => e.Id == id))
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

            viewModel.Items = _context.Items.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            viewModel.Meters = _context.Meters.Where(m => m.ItemId == viewModel.PointId).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.numberMeter.ToString()
            }).ToList();

            return View(viewModel);
        }

        [Authorize(Roles = clsRole.roleadmin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Readings == null)
            {
                return NotFound();
            }

            var reading = await _context.Readings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reading == null)
            {
                return NotFound();
            }

            return View(reading);
        }

        [Authorize(Roles = clsRole.roleadmin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Readings == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Readings'  is null.");
            }
            var reading = await _context.Readings.FindAsync(id);
            if (reading != null)
            {
                _context.Readings.Remove(reading);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReadingExists(int id)
        {
          return (_context.Readings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
