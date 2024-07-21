using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Station.Data;
using Station.Models;
using Station.ViewModel;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleadmin)]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Existing Index action to list all points
        public ActionResult Index()
        {
            var reportData = _context.Items
                .Include(i => i.Subscriber)
                .Include(i => i.Meter)
                .Include(i => i.Point)
                .Select(item => new PointReportViewModel
                {
                    PointId = item.Id,
                    PointName = item.Name,
                    branchName = item.Point.NamePoint, // Assuming Branch is a navigation property
                    TotalAmountCollected = _context.Bills.Where(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id).Sum(b => b.ConsumptionCost),
                    TotalSubscribers = item.Subscriber.Count(),
                    TotalBillsPaid = _context.Bills.Count(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                    TotalBillsUnpaid = _context.Bills.Count(b => !b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                    TotalMeters = item.Meter.Count(),
                    InactiveSubscribers = item.Subscriber.Count(s => !s.IsUpdate)
                }).ToList();

            var model = new ReportViewModel
            {
                PointsReport = reportData
            };

            return View(model);
        }

        // New action to generate report for a single point
        public ActionResult PointReport(int itemId)
        {
            var item = _context.Items
                .Include(i => i.Subscriber)
                .Include(i => i.Meter)
                .Include(i => i.Point)
                .FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            var reportData = new PointReportViewModel
            {
                PointId = item.Id,
                PointName = item.Name,
                branchName = item.Point.NamePoint, // Assuming Branch is a navigation property
                TotalAmountCollected = _context.Bills.Where(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id).Sum(b => b.ConsumptionCost),
                TotalSubscribers = item.Subscriber.Count(),
                TotalBillsPaid = _context.Bills.Count(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                TotalBillsUnpaid = _context.Bills.Count(b => !b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                TotalMeters = item.Meter.Count(),
                InactiveSubscribers = item.Subscriber.Count(s => !s.IsUpdate)
            };

            var model = new ReportViewModel
            {
                PointsReport = new List<PointReportViewModel> { reportData }
            };

            return View("Index", model); // Reusing the same view for single point report
        }

        // New action to generate PDF for the report
        public IActionResult ExportToPdf(int itemId)
        {
            var item = _context.Items
                .Include(i => i.Subscriber)
                .Include(i => i.Meter)
                .Include(i => i.Point)
                .FirstOrDefault(i => i.Id == itemId);

            if (item == null)
            {
                return NotFound();
            }

            var reportData = new PointReportViewModel
            {
                PointId = item.Id,
                PointName = item.Name,
                branchName = item.Point.NamePoint, // Assuming Branch is a navigation property
                TotalAmountCollected = _context.Bills.Where(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id).Sum(b => b.ConsumptionCost),
                TotalSubscribers = item.Subscriber.Count(),
                TotalBillsPaid = _context.Bills.Count(b => b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                TotalBillsUnpaid = _context.Bills.Count(b => !b.IsPaid && b.Meter.Subscriber.PointId == item.Id),
                TotalMeters = item.Meter.Count(),
                InactiveSubscribers = item.Subscriber.Count(s => !s.IsUpdate)
            };

            var model = new ReportViewModel
            {
                PointsReport = new List<PointReportViewModel> { reportData }
            };

            // Create PDF document
            var pdfDoc = new Document(PageSize.A4);
            var memoryStream = new MemoryStream();
            var writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            // Add content to PDF
            var font = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            pdfDoc.Add(new Paragraph($"Point Report for {reportData.PointName}", font));
            pdfDoc.Add(new Paragraph($"Branch Name: {reportData.branchName}", font));
            pdfDoc.Add(new Paragraph($"Total Amount Collected: {reportData.TotalAmountCollected:N2}", font));
            pdfDoc.Add(new Paragraph($"Total Subscribers: {reportData.TotalSubscribers}", font));
            pdfDoc.Add(new Paragraph($"Total Bills Paid: {reportData.TotalBillsPaid}", font));
            pdfDoc.Add(new Paragraph($"Total Bills Unpaid: {reportData.TotalBillsUnpaid}", font));
            pdfDoc.Add(new Paragraph($"Total Meters: {reportData.TotalMeters}", font));
            pdfDoc.Add(new Paragraph($"Inactive Subscribers: {reportData.InactiveSubscribers}", font));

            pdfDoc.Close();
            var bytes = memoryStream.ToArray();
            memoryStream.Close();

            return File(bytes, "application/pdf", $"PointReport_{reportData.PointName}.pdf");
        }
    }
}
