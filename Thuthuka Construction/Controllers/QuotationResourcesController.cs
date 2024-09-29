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
    public class QuotationResourcesController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuotationResourcesController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: QuotationResources
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.quotationResources.Include(q => q.Quotation).Include(q => q.Resource);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: QuotationResources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotationResource = await _context.quotationResources
                .Include(q => q.Quotation)
                .Include(q => q.Resource)
                .FirstOrDefaultAsync(m => m.QuotationResourceId == id);
            if (quotationResource == null)
            {
                return NotFound();
            }

            return View(quotationResource);
        }

        // GET: QuotationResources/Create
        public IActionResult Create()
        {
            ViewData["QuotationId"] = new SelectList(_context.quotations, "QuotationId", "QuotationId");
            ViewData["ResourceId"] = new SelectList(_context.resources, "ResourceId", "ResourceId");
            return View();
        }

        // POST: QuotationResources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuotationResourceId,QuotationId,ResourceId,Quantity")] QuotationResource quotationResource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quotationResource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuotationId"] = new SelectList(_context.quotations, "QuotationId", "QuotationId", quotationResource.QuotationId);
            ViewData["ResourceId"] = new SelectList(_context.resources, "ResourceId", "ResourceId", quotationResource.ResourceId);
            return View(quotationResource);
        }

        // GET: QuotationResources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotationResource = await _context.quotationResources.FindAsync(id);
            if (quotationResource == null)
            {
                return NotFound();
            }
            ViewData["QuotationId"] = new SelectList(_context.quotations, "QuotationId", "QuotationId", quotationResource.QuotationId);
            ViewData["ResourceId"] = new SelectList(_context.resources, "ResourceId", "ResourceId", quotationResource.ResourceId);
            return View(quotationResource);
        }

        // POST: QuotationResources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuotationResourceId,QuotationId,ResourceId,Quantity")] QuotationResource quotationResource)
        {
            if (id != quotationResource.QuotationResourceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quotationResource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuotationResourceExists(quotationResource.QuotationResourceId))
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
            ViewData["QuotationId"] = new SelectList(_context.quotations, "QuotationId", "QuotationId", quotationResource.QuotationId);
            ViewData["ResourceId"] = new SelectList(_context.resources, "ResourceId", "ResourceId", quotationResource.ResourceId);
            return View(quotationResource);
        }

        // GET: QuotationResources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotationResource = await _context.quotationResources
                .Include(q => q.Quotation)
                .Include(q => q.Resource)
                .FirstOrDefaultAsync(m => m.QuotationResourceId == id);
            if (quotationResource == null)
            {
                return NotFound();
            }

            return View(quotationResource);
        }

        // POST: QuotationResources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quotationResource = await _context.quotationResources.FindAsync(id);
            if (quotationResource != null)
            {
                _context.quotationResources.Remove(quotationResource);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuotationResourceExists(int id)
        {
            return _context.quotationResources.Any(e => e.QuotationResourceId == id);
        }
    }
}
