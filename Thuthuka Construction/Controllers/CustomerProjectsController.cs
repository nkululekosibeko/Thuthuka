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

            ViewData["ForemanId"] = new SelectList(foremanUsers, "Id", "UserName");
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "Name");

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

            var customerUsers = _context.applicationUsers
                                       .Where(u => customer.Contains(u.Id))
                                       .Select(u => new { u.Id, u.UserName })
                                       .ToList();

            ViewBag.Project = new SelectList(await _context.projects.ToListAsync(), "ProjectId", "ProjectName");
            ViewData["CustomerId"] = new SelectList(customerUsers, "Id", "UserName", customerProject.CustomerId);

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
                    TempData["success"] = "Customer Project Updated Successfully";
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

        private bool CustomerProjectExists(int id)
        {
            return _context.customerProjects.Any(e => e.CustomerProjectId == id);
        }
    }
}
