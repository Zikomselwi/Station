using Station.Data;
using Station.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Station.Models;
using Station.ViewModel;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleauser + "," + clsRole.roleadmin)]

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _logger;

        public HomeController(ApplicationDbContext logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var item = _logger.Items
                .Include(i => i.Subscriber)
                .Include(i => i.Meter)
                .Include(i => i.Point).ToList();

            if (item == null)
            {
                return NotFound();
            }

            var dashboardData = new DashboardViewModel
            {
                SubscriberCount = _logger.Subscribers.Count(),
                ItemCount = _logger.Items.Count(),
                SubscriptionCount = _logger.Subscriptions.Count(),
                MeterCount = _logger.Meters.Count(),
                PointCount = _logger.Points.Count(),
                TotalAmountCollected = (int)_logger.Bills.Where(b => b.IsPaid).Sum(b => b.ConsumptionCost),
                TotalAmountUnCollected = (int)_logger.Bills.Where(b => !b.IsPaid).Sum(b => b.ConsumptionCost),
                TotalBillsPaid = _logger.Bills.Count(b => b.IsPaid),
                TotalBillsUnpaid = _logger.Bills.Count(b => !b.IsPaid)
            
            };
            return View(dashboardData);
        }

        public IActionResult table()
        {
            var dashboardData = new DashboardViewModel
            {
                SubscriberCount = _logger.Subscribers.Count(),
                ItemCount = _logger.Items.Count(),
                SubscriptionCount = _logger.Subscriptions.Count(),
                MeterCount = _logger.Meters.Count(),
                PointCount = _logger.Points.Count()
            };
            return View(dashboardData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}