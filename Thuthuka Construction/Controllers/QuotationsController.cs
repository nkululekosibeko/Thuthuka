using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Thuthuka_Construction.DB;
using Thuthuka_Construction.Models;
using System.Linq;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Thuthuka_Construction.Controllers
{
    [Authorize]
    public class QuotationsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public QuotationsController(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var quotations = await _context.quotations
                .Include(q => q.CustomerProject) // Include CustomerProject data
                .Include(q => q.QuotationResources) // Include QuotationResources
                .ThenInclude(qr => qr.Resource) // Include Resource data for each QuotationResource
                .ToListAsync();

            return View(quotations);
        }

        // GET: Quotations/Details/5
        // GET: Quotations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Fetch the quotation with related data
            var quotation = await _context.quotations
                .Include(q => q.CustomerProject) // Include CustomerProject data
                .Include(q => q.QuotationResources) // Include QuotationResources
                .ThenInclude(qr => qr.Resource) // Include Resource data for each QuotationResource
                .FirstOrDefaultAsync(m => m.QuotationId == id);

            if (quotation == null)
            {
                return NotFound();
            }

            return View(quotation);
        }


        // GET: Quotations/Create
        public IActionResult Create()
        {
            var resources = _context.resources.ToList();
            ViewBag.Resources = resources;
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId");
            return View();
        }

        // POST: Quotations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int customerProjectId, List<int> resourceIds, List<int> quantities)
        {
            if (resourceIds.Count != quantities.Count)
            {
                return BadRequest("Resource selection and quantities do not match.");
            }

            // Create a new Quotation
            var quotation = new Quotation
            {
                CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending",
                CustomerProjectId = customerProjectId,
                QuotationResources = new List<QuotationResource>()
            };


            //var _customerProject = new CustomerProject
            //{
            //    Status = "quotation available"
            //};

            double totalCost = 0;

            // Iterate through the selected resources and quantities to calculate the total cost
            for (int i = 0; i < resourceIds.Count; i++)
            {
                var resource = await _context.resources.FindAsync(resourceIds[i]);
                if (resource != null)
                {
                    var quantity = quantities[i];
                    var cost = resource.PricePerUnit * quantity;
                    totalCost += cost;

                    // Add the selected resource and its quantity to the Quotation
                    quotation.QuotationResources.Add(new QuotationResource
                    {
                        ResourceId = resourceIds[i],
                        Quantity = quantity
                    });
                }
            }

            // Set the total cost of the quotation
            quotation.TotalCost = totalCost;

            // Save the quotation
            _context.quotations.Add(quotation);
            await _context.SaveChangesAsync();

            TempData["success"] = "Quotation Created Successfully";
            return RedirectToAction(nameof(Index));
        }

        // GET: Quotations/Edit/5
        // GET: Quotations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quotation = await _context.quotations
                .Include(q => q.QuotationResources)
                .ThenInclude(qr => qr.Resource)
                .FirstOrDefaultAsync(q => q.QuotationId == id);

            if (quotation == null)
            {
                return NotFound();
            }

            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "ProjectName");
            ViewBag.Resources = await _context.resources.ToListAsync(); // Load all resources for the dropdown

            return View(quotation);
        }

        // POST: Quotations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int customerProjectId, List<int> resourceIds, List<int> quantities)
        {
            var quotation = await _context.quotations
                .Include(q => q.QuotationResources)
                .FirstOrDefaultAsync(q => q.QuotationId == id);

            if (quotation == null)
            {
                return NotFound();
            }

            if (resourceIds.Count != quantities.Count)
            {
                ModelState.AddModelError(string.Empty, "Resource selection and quantities do not match.");
                ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "ProjectName", quotation.CustomerProjectId);

                ViewBag.Resources = await _context.resources.ToListAsync(); // Reload resources
                return View(quotation);
            }

            // Update the quotation details
            quotation.CustomerProjectId = customerProjectId;

            // Clear existing resources
            quotation.QuotationResources.Clear();

            double totalCost = 0;

            // Iterate through the selected resources and quantities
            for (int i = 0; i < resourceIds.Count; i++)
            {
                var resource = await _context.resources.FindAsync(resourceIds[i]);
                if (resource != null)
                {
                    var quantity = quantities[i];
                    var cost = resource.PricePerUnit * quantity;
                    totalCost += cost;

                    // Add the updated resource and its quantity to the Quotation
                    quotation.QuotationResources.Add(new QuotationResource
                    {
                        ResourceId = resourceIds[i],
                        Quantity = quantity
                    });
                }
            }

            // Set the total cost of the quotation
            quotation.TotalCost = totalCost;

            // Save changes
            _context.Update(quotation);
            await _context.SaveChangesAsync();

            TempData["success"] = "Quotation Updated Successfully";
            return RedirectToAction(nameof(Index));
        }


        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quotation = await _context.quotations.FindAsync(id);
            if (quotation != null)
            {
                _context.quotations.Remove(quotation);
                await _context.SaveChangesAsync();
                TempData["success"] = "Quotation Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        // Helper Method to Check if a Quotation Exists
        private bool QuotationExists(int id)
        {
            return _context.quotations.Any(e => e.QuotationId == id);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptQuotation(int id)
        {
            // Log or set a breakpoint here to check the value of id
            if (id == 0)
            {
                return BadRequest("Invalid Quotation ID");
            }

            var quotation = await _context.quotations.FindAsync(id);

            if (quotation == null)
            {
                // Log or display an error message
                TempData["error"] = "Quotation not found";
                return NotFound();
            }

            var customerProject = await _context.customerProjects.FindAsync(quotation.CustomerProjectId);

            if (customerProject != null)
            {
                customerProject.Status = "Pending Payment";
            }

            quotation.Status = "Quotation Accepted";
            await _context.SaveChangesAsync();
            TempData["success"] = "Quotation Accepted";

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projectId = quotation.CustomerProjectId;
            return RedirectToAction("CreatePayment", "Payments", new { customerId = customerId, projectId = projectId, amount = quotation.TotalCost });
        }


        // Decline Quotation
        [HttpPost]
        public async Task<IActionResult> DeclineQuotation(int id)
        {
            var quotation = await _context.quotations.FindAsync(id);

            if (quotation != null)
            {
                quotation.Status = "Quotation Declined";
                await _context.SaveChangesAsync();
                TempData["success"] = "Quotation Declined";

                // Optionally, you can redirect to a different page after declining the quotation.
                return RedirectToAction("CustomerProjectProgress", "CustomerProjects");
            }

            return NotFound();
        }

    }
}
