using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thuthuka_Construction.DB;
using Thuthuka_Construction.Models;

namespace Thuthuka_Construction.Controllers
{
    [Authorize]
    public class ProgressesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProgressesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Progresses
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.progresses.Include(p => p.customerProject);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Progresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var progress = await _context.progresses
                .Include(p => p.customerProject)
                .FirstOrDefaultAsync(m => m.ProgressId == id);
            if (progress == null)
            {
                return View("Error404");
            }

            return View(progress);
        }

        // GET: Progresses/Create
        public IActionResult Create()
        {
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId");
            return View();
        }

        // POST: Progresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgressId,CustomerProjectId,CurrentPhase,UpdateDate")] Progress progress)
        {
            if (ModelState.IsValid)
            {
                _context.Add(progress);
                await _context.SaveChangesAsync();
                TempData["success"] = "Progress Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", progress.CustomerProjectId);
            return View(progress);
        }

        // GET: Progresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var progress = await _context.progresses.FindAsync(id);
            if (progress == null)
            {
                return View("Error404");
            }
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", progress.CustomerProjectId);
            return View(progress);
        }

        // POST: Progresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProgressId,CustomerProjectId,CurrentPhase,UpdateDate")] Progress progress)
        {
            if (id != progress.ProgressId)
            {
                return View("Error404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progress);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Progress Details Updated Successfully";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgressExists(progress.ProgressId))
                    {
                        return View("Error404");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", progress.CustomerProjectId);
            return View(progress);
        }

        // GET: Progresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var progress = await _context.progresses
                .Include(p => p.customerProject)
                .FirstOrDefaultAsync(m => m.ProgressId == id);
            if (progress == null)
            {
                return View("Error404");
            }

            return View(progress);
        }

        // POST: Progresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progress = await _context.progresses.FindAsync(id);
            if (progress != null)
            {
                _context.progresses.Remove(progress);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Progress Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ProgressExists(int id)
        {
            return _context.progresses.Any(e => e.ProgressId == id);
        }
    }
}
