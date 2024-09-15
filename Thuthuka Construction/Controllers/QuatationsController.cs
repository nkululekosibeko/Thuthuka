using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thuthuka_Construction.DB;
using Thuthuka_Construction.Models;

namespace Thuthuka_Construction.Controllers
{
    public class QuatationsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuatationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Quatations
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.quatations.Include(q => q.Foreman).Include(q => q.customerProject);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Quatations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quatation = await _context.quatations
                .Include(q => q.Foreman)
                .Include(q => q.customerProject)
                .FirstOrDefaultAsync(m => m.QuatationId == id);
            if (quatation == null)
            {
                return NotFound();
            }

            return View(quatation);
        }

        // GET: Quatations/Create
        public IActionResult Create()
        {
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId");
            return View();
        }

        // POST: Quatations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuatationId,CustomerProjectId,ForemanId,TotalCost,Resources,DateCreated,Status")] Quatation quatation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quatation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", quatation.ForemanId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", quatation.CustomerProjectId);
            return View(quatation);
        }

        // GET: Quatations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quatation = await _context.quatations.FindAsync(id);
            if (quatation == null)
            {
                return NotFound();
            }
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", quatation.ForemanId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", quatation.CustomerProjectId);
            return View(quatation);
        }

        // POST: Quatations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuatationId,CustomerProjectId,ForemanId,TotalCost,Resources,DateCreated,Status")] Quatation quatation)
        {
            if (id != quatation.QuatationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quatation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuatationExists(quatation.QuatationId))
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
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", quatation.ForemanId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", quatation.CustomerProjectId);
            return View(quatation);
        }

        // GET: Quatations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quatation = await _context.quatations
                .Include(q => q.Foreman)
                .Include(q => q.customerProject)
                .FirstOrDefaultAsync(m => m.QuatationId == id);
            if (quatation == null)
            {
                return NotFound();
            }

            return View(quatation);
        }

        // POST: Quatations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quatation = await _context.quatations.FindAsync(id);
            if (quatation != null)
            {
                _context.quatations.Remove(quatation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuatationExists(int id)
        {
            return _context.quatations.Any(e => e.QuatationId == id);
        }
    }
}
