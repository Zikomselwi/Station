using Microsoft.AspNetCore.Mvc;
using Station.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Station.Models;
using System.Linq;
using Station.ViewModel;

using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DinkToPdf;
using System.Text;
using DinkToPdf.Contracts;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleadmin)]

    public class BillController : Controller
    {
        private readonly ApplicationDbContext _context;


        public BillController( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new BillViewModel
            {
                Meters = await _context.Meters.ToListAsync(),
                Subscribers = await _context.Subscribers.ToListAsync(),
                Items = await _context.Items.ToListAsync(),
                Readings = await _context.Readings.OrderByDescending(r => r.dateTime).ToListAsync(),
                Bills = await _context.Bills.ToListAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new BillViewModel
            {
                Meters = await _context.Meters.ToListAsync(),
                Subscribers = await _context.Subscribers.ToListAsync(),
                Items = await _context.Items.ToListAsync(),
                Readings = await _context.Readings.OrderByDescending(r => r.dateTime).ToListAsync(),
                Bills = await _context.Bills.ToListAsync()
            };

            return View(model);
        }


        [HttpPost]
        [HttpPost]
        public async Task<JsonResult> GetBillDetails(int meterId)
        {
            var meter = await _context.Meters.FindAsync(meterId);
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.MeterId == meter.Id);
            var readings = await _context.Readings.Where(r => r.MeterId == meterId).OrderByDescending(r => r.dateTime).Take(2).ToListAsync();
            var currentReading = readings.FirstOrDefault();
            var subscription = await _context.Subscriptions.FirstOrDefaultAsync(s => s.Id == subscriber.SubscriptionId);
            var previousReading = readings.Skip(1).FirstOrDefault();

            var consumptionDifference = currentReading?.CurrentRead - (previousReading?.CurrentRead ?? 0);
            decimal consumptionCost = 0;

            if (subscription != null)
            {
                if (subscription.Type == "تجاري")
                {
                    consumptionCost = (decimal)(consumptionDifference / 1000.0) * 300;
                }
                else if (subscription.Type == "منزلي")
                {
                    consumptionCost = (decimal)(consumptionDifference / 1000.0) * 200;
                }
            }

            return Json(new
            {
                subscriptionType = subscription?.Type,
                subscriberId = subscriber?.Id,
                subscriberName = subscriber?.FullName, // Include subscriber's full name here
                currentReading = currentReading?.CurrentRead,
                previousReading = previousReading?.CurrentRead,
                consumptionDifference = consumptionDifference,
                consumptionCost = consumptionCost
            });
        }


        [HttpPost]
        public async Task<JsonResult> MarkAsPaid(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return Json(new { success = false, message = "الفاتورة غير موجودة" });
            }

            bill.IsPaid = true;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult DeleteBill(int id)
        {
            var bill = _context.Bills.Find(id);
            if (bill == null)
            {
                return NotFound();
            }

            _context.Bills.Remove(bill);
            _context.SaveChanges();
            return View();
        
        }

        public IActionResult DeleteAllBills()
        {
            try
            {
                var bills = _context.Bills.ToList();
                _context.Bills.RemoveRange(bills);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error deleting bills: {ex.Message}";
                return View("Error");
            }
        }

        public IActionResult GetMeter(int pointId)
        {
            var meters = _context.Meters
                .Where(s => s.ItemId == pointId)
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.numberMeter.ToString()
                }).ToList();

            return Json(meters);
        }

        [HttpPost]
        public IActionResult EditBill(BillViewModel model)
        {
            if (ModelState.IsValid)
            {
                var bill = _context.Bills.FirstOrDefault(b => b.Id == model.Id);
                if (bill == null)
                {
                    return NotFound();
                }

                bill.MeterId = model.MeterId;
                bill.numberbill = model.numberbill;
                bill.CurrentReading = (double)model.CurrentReading;
                bill.PreviousReading = model.PreviousReading;
                bill.ConsumptionDifference = model.ConsumptionDifference;
                bill.ConsumptionCost = (decimal)model.ConsumptionCost;
                bill.SubscriptionType = model.SubscriptionType;

                _context.Bills.Update(bill);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult PrintInvoice(int id)
        {
            var bill = _context.Bills.Include(b => b.Subscriber).FirstOrDefault(b => b.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            var billViewModel = new BillViewModel
            {
                Id = bill.Id,
                SubscriptionType = bill.SubscriptionType,
                numberbill = bill.numberbill,
                SubscriberName = bill.Subscriber.FullName, // Assuming Subscriber entity has a FullName property
                CurrentReading = bill.CurrentReading,
                PreviousReading = bill.PreviousReading,
                ConsumptionDifference = bill.ConsumptionDifference,
                ConsumptionCost = bill.ConsumptionCost
            };

            return PartialView("_PrintInvoice",billViewModel); // Ensure the partial view is named "_PrintInvoice"
        }

        public async Task<IActionResult> Details(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Subscriber)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                return NotFound();
            }

            var billViewModel = new BillViewModel
            {
                Id = bill.Id,
                SubscriptionType = bill.SubscriptionType,
                numberbill = bill.numberbill,
                SubscriberName = bill.Subscriber.FullName, // Assuming Subscriber entity has a Name property
                CurrentReading = bill.CurrentReading,
                PreviousReading = bill.PreviousReading,
                ConsumptionDifference = bill.ConsumptionDifference,
                ConsumptionCost = bill.ConsumptionCost
            };

            return PartialView(billViewModel);
        }
        public async Task<JsonResult> SubmitBill(BillViewModel model)
        {
            var maxNumberBill = _context.Bills.Max(b => (int?)b.numberbill) ?? 0;

            var bill = new Bill
            {
                subscriberId = model.subscriberId,
                numberbill = maxNumberBill + 1,
                CurrentReading = model.CurrentReading ?? 0,
                PreviousReading = model.PreviousReading,
                ConsumptionDifference = model.ConsumptionDifference,
                ConsumptionCost = model.ConsumptionCost ?? 0,
                BillDate = DateTime.Now,
                IsPaid = model.IsPaid,
                SubscriptionType = model.SubscriptionType,
                MeterId = model.MeterId 
            };

            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();

            return Json(new { success = true, bill });
        }

        
    }


}

