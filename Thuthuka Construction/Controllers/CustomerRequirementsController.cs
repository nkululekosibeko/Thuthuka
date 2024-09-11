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
    public class CustomerRequirementsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CustomerRequirementsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CustomerRequirements
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.customerRequirements.Include(c => c.Customer).Include(c => c.Project);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: CustomerRequirements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRequirements = await _context.customerRequirements
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.RequirementsId == id);
            if (customerRequirements == null)
            {
                return NotFound();
            }

            return View(customerRequirements);
        }

        // GET: CustomerRequirements/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId");
            return View();
        }

        // POST: CustomerRequirements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequirementsId,Description,ProjectId,CustomerId")] CustomerRequirements customerRequirements)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerRequirements);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerRequirements.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerRequirements.ProjectId);
            return View(customerRequirements);
        }

        // GET: CustomerRequirements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRequirements = await _context.customerRequirements.FindAsync(id);
            if (customerRequirements == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerRequirements.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerRequirements.ProjectId);
            return View(customerRequirements);
        }

        // POST: CustomerRequirements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RequirementsId,Description,ProjectId,CustomerId")] CustomerRequirements customerRequirements)
        {
            if (id != customerRequirements.RequirementsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerRequirements);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerRequirementsExists(customerRequirements.RequirementsId))
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerRequirements.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerRequirements.ProjectId);
            return View(customerRequirements);
        }

        // GET: CustomerRequirements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerRequirements = await _context.customerRequirements
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.RequirementsId == id);
            if (customerRequirements == null)
            {
                return NotFound();
            }

            return View(customerRequirements);
        }

        // POST: CustomerRequirements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerRequirements = await _context.customerRequirements.FindAsync(id);
            if (customerRequirements != null)
            {
                _context.customerRequirements.Remove(customerRequirements);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerRequirementsExists(int id)
        {
            return _context.customerRequirements.Any(e => e.RequirementsId == id);
        }
    }
}
