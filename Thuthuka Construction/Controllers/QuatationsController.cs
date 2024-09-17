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

        // POST: Accept Quotation
        [HttpPost]
        public IActionResult AcceptQuotation(int id)
        {
            var quotation = _context.quatations.Find(id);  // Find the quotation by ID
            if (quotation == null)
            {
                return NotFound();  // Handle case where quotation is not found
            }

            quotation.Status = "Quotation Accepted";  // Update the status to "Accepted"
            _context.Update(quotation);  // Update the quotation in the database
            _context.SaveChanges();  // Save the changes

            TempData["success"] = "Quotation Accepted, the foreman will be notified";  // Set a success message

            return RedirectToAction("CustomerProjectProgress", "CustomerProjects");  // Redirect to another page
        }

        // POST: Decline Quotation
        [HttpPost]
        public IActionResult DeclineQuotation(int id)
        {
            var quotation = _context.quatations.Find(id);  // Find the quotation by ID
            if (quotation == null)
            {
                return NotFound();  // Handle case where quotation is not found
            }

            quotation.Status = "Quotation Declined";  // Update the status to "Declined"
            _context.Update(quotation);  // Update the quotation in the database
            _context.SaveChanges();  // Save the changes

            TempData["success"] = "Quotation Declined, the foreman will be notified";  // Set a success message

            return RedirectToAction("CustomerProjectProgress", "CustomerProjects");  // Redirect to another page
        }






        // GET: Quatations/Create
        public IActionResult Create()
        {
            var foremanRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Foreman")?.Id;
            var foremen = _context.UserRoles
                                  .Where(ur => ur.RoleId == foremanRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var foremanUsers = _context.applicationUsers
                                       .Where(u => foremen.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName"); // Display UserName instead of ID
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
                TempData["success"] = "Quatation Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            // Same filtering logic for the Foreman dropdown in case the form is reloaded
            var foremanRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Foreman")?.Id;
            var foremen = _context.UserRoles
                                  .Where(ur => ur.RoleId == foremanRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var foremanUsers = _context.applicationUsers
                                       .Where(u => foremen.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName", quatation.ForemanId);
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
                    TempData["success"] = "Quatation Detailsn Updated Successfully";

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
            TempData["success"] = "Quatation Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool QuatationExists(int id)
        {
            return _context.quatations.Any(e => e.QuatationId == id);
        }
    }
}
