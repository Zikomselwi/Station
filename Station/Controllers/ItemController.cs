using Station.Data;
using Station.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Station.Controllers
{
    [Authorize(Roles = clsRole.roleadmin)]


    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Item> itemslist = _db.Items.Include(x => x.Point).Include(a=>a.Subscriber).ToList();
            return View(itemslist);
        }
        public async Task<IActionResult> DeleteAll()
        {
            var allMeters = await _db.Items.ToListAsync();
            _db.Items.RemoveRange(allMeters);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public void CreateBranch(int idselect = 0)
        {
            var branch = _db.Points.ToList();
            var branchItems = branch.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.NamePoint
            });
            ViewBag.Pointslist = branchItems;
        }
        public IActionResult New()
        {
            CreateBranch();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Item item)
        {
            if (ModelState.IsValid)

            {

                _db.Items.Add(item);

                _db.SaveChanges();
                TempData["data"] = "تم الاضافة بنجاح";

                return RedirectToAction("Index");
            }
            else
            {

                return View(item);
            }

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Items == null)
            {
                return NotFound();
            }
            var subscriber = await _db.Items.Include(s => s.Point)

                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriber == null)
            {
                return NotFound();
            }

            return View(subscriber);
        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _db.Items.Find(Id);
            if (item == null)
            {
                return NotFound();
            }
            CreateBranch(item.PointId);
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var relatedMeters = await _db.Meters.Where(m => m.ItemId == id).ToListAsync();
            if (relatedMeters.Any())
            {
                // Delete the related Meter records
                _db.Meters.RemoveRange(relatedMeters);
                await _db.SaveChangesAsync();
            }

            // Now delete the Item
            _db.Items.Remove(item);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                _db.Items.Update(item);
                item.IsUpdated = true;
                _db.SaveChanges();
                TempData["data"] = "تم التعديل بنجاح";

                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }

        }


    }



}