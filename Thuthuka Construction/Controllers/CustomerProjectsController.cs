using System;
using System.Collections.Generic;
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





        public async Task<IActionResult> CustomerProjectProgress()
        {
            // Retrieve the user ID of the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user ID was retrieved correctly
            if (string.IsNullOrEmpty(userId))
            {
                // Handle the case where the user ID is not found, and redirect to login
                ModelState.AddModelError("UserId", "User ID could not be retrieved. Please log in again.");
                return RedirectToAction("Login", "Account"); // Adjust this to your login page/action
            }

            // Fetch all projects for the logged-in customer directly from the database
            var customerProjects = await _context.customerProjects
                .Include(cp => cp.Project)    // Include related Project entity
                .Include(cp => cp.Customer)   // Include related Customer entity
                .Where(cp => cp.CustomerId == userId) // Filter for the currently logged-in user
                .ToListAsync();

            // Check if there are no projects
            if (customerProjects == null || !customerProjects.Any())
            {
                return NotFound(); // Return 404 if no projects are found
            }

            // Return the list of projects to the view
            return View(customerProjects);
        }

        public IActionResult NoQuotation()
        {
            return View();
        }


        public async Task<IActionResult> CustomerProjectQuotation(int customerProjectId)
        {
            // Retrieve the Quotation based on the CustomerProjectId
            var quotation = await _context.quatations
                .Include(q => q.customerProject)   // Include the related CustomerProject entity
                .Include(q => q.Foreman)           // Include the related Foreman (if applicable)
                .FirstOrDefaultAsync(q => q.CustomerProjectId == customerProjectId);


            // Check if the quotation was found
            if (quotation == null)
            {
                return RedirectToAction("NoQuotation"); // Return 404 if no quotation is found
            }

            ViewBag.Project = quotation.customerProject.Project;

            // Return the quotation to the view
            return View(quotation);
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
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);
            if (customerProject == null)
            {
                return View("Error404");
            }

            return View(customerProject);
        }

        // GET: CustomerProjects/Create
        // GET: CustomerProjects/Create
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

            // Foreman dropdown
            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName");

            // ProjectType dropdown
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "Name");

            return View();
        }




        // POST: CustomerProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            // Populate the projects for the ViewBag in case of an error
            ViewBag.Project = new SelectList(await _context.projects.ToListAsync(), "ProjectId", "ProjectName");
            ViewData["CustomerId"] = new SelectList(CustomerUsers, "Id", "UserName", customerProject.CustomerId); // Display UserName instead of ID

            return View(customerProject);
        }


        // GET: CustomerProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var customerProject = await _context.customerProjects.FindAsync(id);
            if (customerProject == null)
            {
                return NotFound();
            }
            ViewBag.Projects = new SelectList(await _context.projects.ToListAsync(), "ProjectId", "ProjectName");
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerProject.CustomerId);
            return View(customerProject);
        }

        // POST: CustomerProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerProjectId,CustomerId,QuatationId,SelectDate,Status")] CustomerProject customerProject)
        {
            if (id != customerProject.CustomerProjectId)
            {
                return View("Error404");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerProject);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Customer Updated successfull Successfully";
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
            ViewBag.Projects = new SelectList(await _context.projects.ToListAsync(), "ProjectId", "ProjectName");
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", customerProject.CustomerId);
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
                .FirstOrDefaultAsync(m => m.CustomerProjectId == id);
            if (customerProject == null)
            {
                return NotFound();
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
    }
}
