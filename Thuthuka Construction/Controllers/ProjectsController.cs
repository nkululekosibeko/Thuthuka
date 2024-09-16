using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thuthuka_Construction.DB;
using Thuthuka_Construction.Models;

namespace Thuthuka_Construction.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public ProjectsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.projects.Include(p => p.Foreman).Include(p => p.ProjectType);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.projects
                .Include(p => p.Foreman)
                .Include(p => p.ProjectType)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            // Filter only users with Foreman role and populate the dropdown list
            var foremanRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Foreman")?.Id;
            var foremen = _context.UserRoles
                                  .Where(ur => ur.RoleId == foremanRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var foremanUsers = _context.applicationUsers
                                       .Where(u => foremen.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName"); // Display UserName instead of ID

            ViewData["ProjectTypeId"] = new SelectList(
                _context.projectTypes,
                "ProjectTypeId",     // Value field
                "Name"               // Display field
            );
            ViewBag.ProjectTypeId = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,Description,StartDate,EndDate,Status,ProjectTypeId,ForemanId")] Project project)
        {
            // Validate date ranges
            if (project.StartDate > project.EndDate)
            {
                ModelState.AddModelError("", "End Date cannot be before Start Date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["success"] = "Project Created Successfully";
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

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName", project.ForemanId); // Set selected value
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeId", project.ProjectTypeId);
            return View(project);
        }

        public async Task<IActionResult> ListByCategory(int categoryId)
        {
            // Fetch the projects that belong to the selected project type (category)
            var projects = await _context.projects
                .Include(p => p.ProjectType)
                .Where(p => p.ProjectTypeId == categoryId)
                .ToListAsync();

            if (projects == null || !projects.Any())
            {
                return NotFound();
            }

            return View(projects);
        }


        public async Task<IActionResult> BuildCustomerProject(int projectId)
        {
            // Retrieve the current user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                // Handle user not being logged in
                return RedirectToAction("Login", "Account");
            }

            // Create a new CustomerProject instance
            var _customerProject = new CustomerProject
            {
                CustomerId = userId,
                QuatationId = 0, // Placeholder, will be set later
                SelectDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "pending quotation"
            };

            // Add the instance to the database
            _context.customerProjects.Add(_customerProject);
            await _context.SaveChangesAsync();

            // Redirect to a confirmation page or the project details page
            return RedirectToAction("ProjectDetails", new { id = projectId });
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var foremanRoleId = _context.Roles.FirstOrDefault(r => r.Name == "Foreman")?.Id;
            var foremen = _context.UserRoles
                                  .Where(ur => ur.RoleId == foremanRoleId)
                                  .Select(ur => ur.UserId).ToList();

            var foremanUsers = _context.applicationUsers
                                       .Where(u => foremen.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName"); // Display UserName instead of ID

            ViewData["ProjectTypeId"] = new SelectList(
                _context.projectTypes,
                "ProjectTypeId",     // Value field
                "Name"               // Display field
            );
            ViewBag.ProjectTypeId = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeName");
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,Description,StartDate,EndDate,Status,ProjectTypeId,ForemanId")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            // Validate date ranges
            if (project.StartDate > project.EndDate)
            {
                ModelState.AddModelError("", "End Date cannot be before Start Date.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Projects Details Updated Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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

            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "UserName", project.ForemanId); // Display UserName instead of ID
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "Name", project.ProjectTypeId);
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.projects
                .Include(p => p.Foreman)
                .Include(p => p.ProjectType)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.projects.FindAsync(id);
            if (project != null)
            {
                _context.projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Projects Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.projects.Any(e => e.ProjectId == id);
        }
    }
}
