﻿using System;
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
            var applicationDBContext = _context.projects.Include(p => p.Customer).Include(p => p.Foreman).Include(p => p.ProjectType);
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
                .Include(p => p.Customer)
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeId");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,StartDate,EndDate,ProjectTypeId,CustomerId,ForemanId,Status")] Project project)
        {
            if (ModelState.IsValid)
            {
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.CustomerId);
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.ForemanId);
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeId", project.ProjectTypeId);
            return View(project);
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.CustomerId);
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.ForemanId);
            ViewData["ProjectTypeId"] = new SelectList(_context.projectTypes, "ProjectTypeId", "ProjectTypeId", project.ProjectTypeId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,StartDate,EndDate,ProjectTypeId,CustomerId,ForemanId,Status")] Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.CustomerId);
            ViewData["ForemanId"] = new SelectList(_context.applicationUsers, "Id", "Id", project.ForemanId);
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
                .Include(p => p.Customer)
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
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.projects.Any(e => e.ProjectId == id);
        }
    }
}