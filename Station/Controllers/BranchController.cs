using Station.Data;
using Station.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Station.Controllers
{

    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BranchController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var itemslist = _db.Points.Include(a=>a.Items).ToList();
            return View(itemslist);
        }

      
     

    
        public IActionResult New(Point item)
        {
            if (ModelState.IsValid)

            {

                _db.Points.Add(item);

                _db.SaveChanges();
                TempData["data"] = "تم الاضافة بنجاح";

                return RedirectToAction("Index");
            }
            else
            {

                return View(item);
            }

        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _db.Points.Find(Id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Point item)
        {
            if (ModelState.IsValid)
            {
                _db.Points.Update(item);
                _db.SaveChanges();
                TempData["data"] = "تم التعديل بنجاح";

                return RedirectToAction("Index");
            }
            else
            {
                return View(item);
            }
}
public IActionResult Delete(int id)
        {
            var point = _db.Points.Find(id);
            if (point == null)
            {
                return NotFound();
            }

            var relatedItems = _db.Items.Where(i => i.PointId == id).ToList();
            if (relatedItems.Any())
            {
                using (var transaction = _db.Database.BeginTransaction())
                {
                    try
                    {
                        var relatedMeters = _db.Meters.Where(m => relatedItems.Select(i => i.Id).Contains(m.ItemId)).ToList();
                        _db.Meters.RemoveRange(relatedMeters);
                        _db.SaveChanges();
                        _db.Items.RemoveRange(relatedItems);
                        _db.SaveChanges();
                        _db.Points.Remove(point);
                        _db.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                _db.Points.Remove(point);
                _db.SaveChanges();
            }

            TempData["data"] = "تم الحذف بنجاح";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteAll()
        {
            var allMeters = await _db.Points.ToListAsync();
            _db.Points.RemoveRange(allMeters);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }


}

