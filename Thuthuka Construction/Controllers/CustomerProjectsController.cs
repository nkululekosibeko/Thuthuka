using System;
using System.Linq;
using System.Security.Claims;
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
    public class CustomerProjectsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public CustomerProjectsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: CustomerProjects
        public async Task<IActionResult> Index()
        {
            var customerProjectsDBContext = _context.customerProjects
                .Include(cp => cp.Customer)
                .Include(cp => cp.Project);

            return View(await customerProjectsDBContext.ToListAsync());
        }

        // GET: CustomerProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var customerProject = await _context.customerProjects
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);

            if (customerProject == null)
            {
                return View("Error404");
            }

            return View(customerProject);
        }

        // GET: CustomerProjects/Create
        public IActionResult Create()
        {
            var customerRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Customer")?.Id;
            var customer = _context.UserRoles
                                  .Where(ur => ur.RoleId == customerRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var CustomerUsers = _context.applicationUsers
                                       .Where(u => customer.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["CustomerId"] = new SelectList(CustomerUsers, "Id", "UserName");
            ViewBag.Projects = new SelectList(_context.projects, "ProjectId", "ProjectName");

            return View();
        }

        // POST: CustomerProjects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerProjectId,CustomerId,QuatationId,SelectDate,Status")] CustomerProject customerProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerProject);
                await _context.SaveChangesAsync();
                TempData["success"] = "Customer Project Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            var customerRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Customer")?.Id;
            var customer = _context.UserRoles
                                  .Where(ur => ur.RoleId == customerRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var CustomerUsers = _context.applicationUsers
                                       .Where(u => customer.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["CustomerId"] = new SelectList(CustomerUsers, "Id", "UserName", customerProject.CustomerId);
            ViewBag.Projects = new SelectList(await _context.projects.ToListAsync(), "ProjectId", "ProjectName");

            return View(customerProject);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            // Check for a valid ID
            if (id == null)
            {
                return View("Error404");
            }

            // Fetch the customer project based on the provided ID
            var customerProject = await _context.customerProjects.FindAsync(id);
            if (customerProject == null)
            {
                return View("Error404");
            }

            // Fetch all users with the "Customer" role
            var customerRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Customer")?.Id;
            var customerIds = _context.UserRoles
                                       .Where(ur => ur.RoleId == customerRoleId)
                                       .Select(ur => ur.UserId).ToList();

            var CustomerUsers = _context.applicationUsers
                                         .Where(u => customerIds.Contains(u.Id))
                                         .Select(u => new { u.Id, u.UserName })
                                         .ToList();

            // Populate ViewData for dropdowns
            ViewData["CustomerId"] = new SelectList(CustomerUsers, "Id", "UserName", customerProject.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectName", customerProject.ProjectId);

            // Define possible statuses
            var statuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending Quotation", Text = "Pending Quotation" },
        new SelectListItem { Value = "In Progress", Text = "In Progress" },
        new SelectListItem { Value = "Completed", Text = "Completed" },
        new SelectListItem { Value = "On Hold", Text = "On Hold" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };
            ViewData["Status"] = new SelectList(statuses, "Value", "Text", customerProject.Status);

            return View(customerProject);
        }

        // POST: CustomerProjects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerProjectId,CustomerId,ProjectId,StartDate,Status,StatusReason")] CustomerProject customerProject)
        {
            if (id != customerProject.CustomerProjectId)
            {
                return View("Error404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    customerProject.CreatedDate = DateOnly.FromDateTime(DateTime.Now); // This may not be necessary to set here, based on your requirements.
                    _context.Update(customerProject);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Customer project updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProjectExists(customerProject.CustomerProjectId))
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

            // If the ModelState is invalid, repopulate ViewData for dropdowns
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "UserName", customerProject.CustomerId);
            ViewData["ProjectId"] = new SelectList(_context.projects, "ProjectId", "ProjectName", customerProject.ProjectId);

            // Repopulate Status dropdown
            ViewData["Status"] = new SelectList(new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending Quotation", Text = "Pending Quotation" },
        new SelectListItem { Value = "In Progress", Text = "In Progress" },
        new SelectListItem { Value = "Completed", Text = "Completed" },
        new SelectListItem { Value = "On Hold", Text = "On Hold" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    }, "Value", "Text", customerProject.Status);

            return View(customerProject);
        }




        // GET: CustomerProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var customerProject = await _context.customerProjects
                .Include(c => c.Customer)
                .Include(c => c.Project)
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);
            if (customerProject == null)
            {
                return View("Error404");
            }

            return View(customerProject);
        }

        // POST: CustomerProjects/Delete/5
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
            TempData["success"] = "Customer Project deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerProjectExists(int id)
        {
            return _context.customerProjects.Any(e => e.CustomerProjectId == id);
        }

        // Additional Custom Methods

        public async Task<IActionResult> CustomerProjectProgress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("UserId", "User ID could not be retrieved. Please log in again.");
                return RedirectToAction("Login", "Account");
            }

            var customerProjects = await _context.customerProjects
                .Include(cp => cp.Project)
                .Include(cp => cp.Customer)
                .Where(cp => cp.CustomerId == userId)
                .ToListAsync();

            if (customerProjects == null || !customerProjects.Any())
            {
                return View("Error404");
            }

            return View(customerProjects);
        }

        public IActionResult NoQuotation()
        {
            return View();
        }

        public async Task<IActionResult> CustomerProjectQuotation(int customerProjectId)
        {
            var quotation = await _context.quotations
                .Include(q => q.QuotationResources)
                .ThenInclude(qr => qr.Resource)
                .Include(q => q.CustomerProject)
                .FirstOrDefaultAsync(q => q.CustomerProjectId == customerProjectId);

            if (quotation == null)
            {
                return RedirectToAction("NoQuotation");
            }

            return View(quotation);
        }

        public async Task<IActionResult> ProjectDetails(int customerProjectId)
        {
            // Retrieve the CustomerProject that contains the project ID
            var customerProject = await _context.customerProjects
                .Include(cp => cp.Project) // Include the Project details
                .FirstOrDefaultAsync(cp => cp.CustomerProjectId == customerProjectId);

            // Check if the customer project exists
            if (customerProject == null)
            {
                return NotFound(); // Return 404 if not found
            }

            // Pass the Project to the view
            return View(customerProject.Project); // Return the related Project
        }

    }
}
