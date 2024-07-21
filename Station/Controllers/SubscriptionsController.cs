//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using Station.Data;
//using Station.Models;

//namespace Station.Controllers
//{
//    public class SubscriptionsController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public SubscriptionsController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // GET: Subscriptions
//        public async Task<IActionResult> Index()
//        {
//            var applicationDbContext = _context.Subscriptions.Include(s => s.Subscriber);
//            return View(await applicationDbContext.ToListAsync());
//        }

//        // GET: Subscriptions/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Subscriptions == null)
//            {
//                return NotFound();
//            }

//            var subscription = await _context.Subscriptions
//                .Include(s => s.Subscriber)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (subscription == null)
//            {
//                return NotFound();
//            }

//            return View(subscription);
//        }

//        // GET: Subscriptions/Create
//        public IActionResult Create()
//        {
//            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Address");
//            return View();
//        }

//        // POST: Subscriptions/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Type,Price,SubId")] Subscription subscription)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(subscription);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Address", subscription.SubId);
//            return View(subscription);
//        }

//        // GET: Subscriptions/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Subscriptions == null)
//            {
//                return NotFound();
//            }

//            var subscription = await _context.Subscriptions.FindAsync(id);
//            if (subscription == null)
//            {
//                return NotFound();
//            }
//            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Address", subscription.SubId);
//            return View(subscription);
//        }

//        // POST: Subscriptions/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Price,SubscriberId")] Subscription subscription)
//        {
//            if (id != subscription.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(subscription);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!SubscriptionExists(subscription.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["SubscriberId"] = new SelectList(_context.Subscribers, "Id", "Address", subscription.SubId);
//            return View(subscription);
//        }

//        // GET: Subscriptions/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Subscriptions == null)
//            {
//                return NotFound();
//            }

//            var subscription = await _context.Subscriptions
//                .Include(s => s.Subscriber)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (subscription == null)
//            {
//                return NotFound();
//            }

//            return View(subscription);
//        }

//        // POST: Subscriptions/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Subscriptions == null)
//            {
//                return Problem("Entity set 'ApplicationDbContext.Subscriptions'  is null.");
//            }
//            var subscription = await _context.Subscriptions.FindAsync(id);
//            if (subscription != null)
//            {
//                _context.Subscriptions.Remove(subscription);
//            }
            
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool SubscriptionExists(int id)
//        {
//          return (_context.Subscriptions?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
