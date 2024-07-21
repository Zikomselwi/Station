using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Station.Models; // Update with your actual namespace
using Station.Data;
using Microsoft.EntityFrameworkCore;
using Station.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleadmin)]

    public class SubscribersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscribersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Subscriber> item = _context.Subscribers.Include(a => a.Meter).Include(a => a.Item).Include(a => a.Subscription).ToList();

            return View(item);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var reading = await _context.Subscribers.FindAsync(id);

            if (reading == null)
            {
                return NotFound();
            }

            var viewModel = new SubscriberViewModel
            {
                Id = reading.Id,
                FullName = reading.FullName,
                Address = reading.Address,
                Phone = reading.Phone,
                SubscriberNumber = reading.SubscriberNumber,

                dataTime = reading.Created,
                MeterId = reading.MeterId,
                PointId = reading.PointId,
                SubscriptionId = reading.SubscriptionId,
                Items = _context.Items.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),
                Meters = _context.Meters.Where(m => m.ItemId == reading.PointId).Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = m.numberMeter.ToString()
                }).ToList(),

                Subscriptions = _context.Subscriptions.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Type
                }).ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubscriberViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var reading = await _context.Subscribers.FindAsync(id);
                if (reading == null)
                {
                    return NotFound();
                }

                reading.FullName = viewModel.FullName;
                reading.SubscriberNumber = viewModel.SubscriberNumber;
                reading.Phone = viewModel.Phone;
                reading.Address = viewModel.Address;
                reading.MeterId = viewModel.MeterId;
                reading.PointId = viewModel.PointId;
                reading.Created = viewModel.dataTime;
                reading.SubscriptionId = viewModel.SubscriptionId;
                try
                {
                    _context.Update(reading);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Subscribers.Any(e => e.Id == id))
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
            }).ToList(); viewModel.Subscriptions = _context.Subscriptions.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Type
            }).ToList();

            viewModel.Meters = _context.Meters.Where(m => m.ItemId == viewModel.PointId).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.numberMeter.ToString()
            }).ToList();

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new SubscriberViewModel
            {
                Items = _context.Items.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList(),

                Subscriptions = _context.Subscriptions.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Type
                }).ToList(),

                Meters = new List<SelectListItem>()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriberViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var subscriber = new Subscriber
                {
                    FullName = viewModel.FullName,
                    SubscriberNumber = viewModel.SubscriberNumber,
                    Phone = viewModel.Phone,
                    Address = viewModel.Address,
                    MeterId = viewModel.MeterId,
                    PointId = viewModel.PointId,
                    Created = viewModel.dataTime,
                    SubscriptionId = viewModel.SubscriptionId

                };

                _context.Add(subscriber);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.Items = _context.Items.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            viewModel.Subscriptions = _context.Subscriptions.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Type
            }).ToList();

            viewModel.Meters = _context.Meters.Where(m => m.ItemId == viewModel.PointId).Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.numberMeter.ToString()
            }).ToList();

            return View(viewModel);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Subscribers.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var related = await _context.Subscribers.Where(m => m.SubscriptionId == id).ToListAsync();
            if (related.Any())
            {
                _context.Subscribers.RemoveRange(related);
                await _context.SaveChangesAsync();
            }

            _context.Subscribers.Remove(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Subscribers == null)
            {
                return NotFound();
            }

            var subscriber = await _context.Subscribers
                .Include(s => s.Meter).ThenInclude(s => s.Item)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }
        [HttpPost]
        public JsonResult CheckMeterIdExists(int meterId)
        {
            var existingSubscriber = _context.Subscribers.Any(s => s.MeterId == meterId);
            return Json(existingSubscriber);
        }
        public async Task<IActionResult> DeleteAll()
        {
            var allMeters = await _context.Subscribers.ToListAsync();
            _context.Subscribers.RemoveRange(allMeters);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
    }
}
