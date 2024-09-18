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
    public class CustomerProjects1Controller : Controller
    {
        private readonly ApplicationDBContext _context;

        public CustomerProjects1Controller(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CustomerProjects1
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.customerProjects.Include(c => c.Customer).Include(c => c.Project);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: CustomerProjects1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProject = await _context.customerProjects
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);
            if (customerProject == null)
            {
                return NotFound();
            }

            return View(customerProject);
        }

        // GET: CustomerProjects1/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId");
            return View();
        }

        // POST: CustomerProjects1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerProjectId,CustomerId,ProjectId,SelectDate,Status")] CustomerProject customerProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerProject.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerProject.ProjectId);
            return View(customerProject);
        }

        // GET: CustomerProjects1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProject = await _context.customerProjects.FindAsync(id);
            if (customerProject == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerProject.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerProject.ProjectId);
            return View(customerProject);
        }

        // POST: CustomerProjects1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerProjectId,CustomerId,ProjectId,SelectDate,Status")] CustomerProject customerProject)
        {
            if (id != customerProject.CustomerProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProjectExists(customerProject.CustomerProjectId))
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerProject.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectId", customerProject.ProjectId);
            return View(customerProject);
        }

        // GET: CustomerProjects1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProject = await _context.customerProjects
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);
            if (customerProject == null)
            {
                return NotFound();
            }

            return View(customerProject);
        }

        // POST: CustomerProjects1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerProject = await _context.customerProjects.FindAsync(id);
            if (customerProject != null)
            {
                _context.customerProjects.Remove(customerProject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerProjectExists(int id)
        {
            return _context.customerProjects.Any(e => e.CustomerProjectId == id);
        }
    }
}
