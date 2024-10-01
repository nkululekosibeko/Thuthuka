using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectsController(ApplicationDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        //GET: Projects/Create
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


        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,Description,ProjectTypeId,ForemanId")] Project project/*, IFormFile file*/)
        {
            if (ModelState.IsValid)
            {
                //string wwwRootPath = _webHostEnvironment.WebRootPath;
                //if (file != null)
                //{
                //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                //    string projectImagePath = Path.Combine(wwwRootPath, @"files\projectIMG");

                //    if (!string.IsNullOrEmpty(project.ImageUrl))
                //    {
                //        var oldImagePath = Path.Combine(wwwRootPath, project.ImageUrl.TrimStart('\\'));
                //        if (System.IO.File.Exists(oldImagePath))
                //        {
                //            System.IO.File.Delete(oldImagePath);
                //        }
                //    }

                //    using (var fileStream = new FileStream(Path.Combine(projectImagePath, fileName), FileMode.Create))
                //    {
                //        file.CopyTo(fileStream);
                //    }

                //    project.ImageUrl = @"files\projectIMG\" + fileName;
                //}

                _context.Add(project);
                await _context.SaveChangesAsync();
                TempData["success"] = "Project Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            //Same filtering logic for the Foreman dropdown in case the form is reloaded
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
                return View("Error404");
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

            // Check if the project exists
            var project = await _context.projects.FindAsync(projectId);
            if (project == null)
            {
                TempData["error"] = "The project you are trying to select does not exist.";
                return RedirectToAction("Index", "Projects"); // Redirect back to project list or any appropriate page
            }

            // Create a new CustomerProject instance and assign the selected ProjectId
            var _customerProject = new CustomerProject
            {
                CustomerId = userId,
                ProjectId = projectId, // Assign the project ID here
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "pending quotation"
            };

            // Add the instance to the database
            _context.customerProjects.Add(_customerProject);
            await _context.SaveChangesAsync();

            // Inform the user that their project was successfully created
            TempData["success"] = "Project selected. The foreman will be notified to create a quotation.";

            // Redirect to the Customer Project Progress or Details page
            return RedirectToAction("Index", "Home");
        }






        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error404");
            }

            var project = await _context.projects.FindAsync(id);
            if (project == null)
            {
                return View("Error404");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,Description,ProjectTypeId,ForemanId")] Project project /*IFormFile file*/)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //string wwwRootPath = _webHostEnvironment.WebRootPath;
                    //if (file != null)
                    //{
                    //    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //    string projectImagePath = Path.Combine(wwwRootPath, @"files\projectIMG");


                    //    if (!string.IsNullOrEmpty(project.ImageUrl))
                    //    {
                    //        var oldImagePath = Path.Combine(wwwRootPath, project.ImageUrl.TrimStart('\\'));
                    //        if (System.IO.File.Exists(oldImagePath))
                    //        {
                    //            System.IO.File.Delete(oldImagePath);
                    //        }
                    //    }


                    //    if (!string.IsNullOrEmpty(project.ImageUrl))
                    //    {
                    //        var oldImagePath = Path.Combine(wwwRootPath, project.ImageUrl.TrimStart('\\'));
                    //        if (System.IO.File.Exists(oldImagePath))
                    //        {
                    //            System.IO.File.Delete(oldImagePath);
                    //        }
                    //    }

                    //    using (var fileStream = new FileStream(Path.Combine(projectImagePath, fileName), FileMode.Create))
                    //    {
                    //        file.CopyTo(fileStream);
                    //    }

                    //    project.ImageUrl = @"files\projectIMG\" + fileName;
                    //}
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    TempData["success"] = "Project Details updated Successfully";

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
            //Same filtering logic for the Foreman dropdown in case the form is reloaded
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
            TempData["success"] = "Project Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.projects.Any(e => e.ProjectId == id);
        }


        public async Task<IActionResult> MoreDetails(int customerProjectId)
        {
            // Find the project by its ID
            var project = await _context.customerProjects
                .FirstOrDefaultAsync(p => p.CustomerProjectId == customerProjectId);

            // Check if the project exists
            if (project == null)
            {
                return NotFound();
            }

            // Pass the project data to the view
            return View(project);
        }

    }
}
