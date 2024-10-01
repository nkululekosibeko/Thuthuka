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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDBContext _context;

        public PaymentsController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            var applicationDBContext = _context.payments.Include(p => p.Customer).Include(p => p.CustomerProject);
            return View(await applicationDBContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.Customer)
                .Include(p => p.CustomerProject)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id");
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,PaymentType,Amount,PaymentDate,CustomerId,CustomerProjectId,Status")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", payment.CustomerId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", payment.CustomerProjectId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", payment.CustomerId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", payment.CustomerProjectId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,PaymentType,Amount,PaymentDate,CustomerId,CustomerProjectId,Status")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["CustomerId"] = new SelectList(_context.applicationUsers, "Id", "Id", payment.CustomerId);
            ViewData["CustomerProjectId"] = new SelectList(_context.customerProjects, "CustomerProjectId", "CustomerProjectId", payment.CustomerProjectId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.payments
                .Include(p => p.Customer)
                .Include(p => p.CustomerProject)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.payments.FindAsync(id);
            if (payment != null)
            {
                _context.payments.Remove(payment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.payments.Any(e => e.PaymentId == id);
        }

        public IActionResult CreatePayment(string customerId, int projectId, double amount)
        {
            var payment = new Payment
            {
                CustomerId = customerId,
                CustomerProjectId = projectId,
                Amount = amount
            };

            return View(payment);
        }



        [HttpPost]
        public async Task<IActionResult> ProcessPayment(Payment payment)
        {
            if (ModelState.IsValid)
            {
                var _payment = new Payment
                {
                    CustomerId = payment.CustomerId,
                    CustomerProjectId = payment.CustomerProjectId,
                    Amount = payment.Amount,
                    PaymentType = payment.PaymentType,
                    PaymentDate = DateOnly.FromDateTime(DateTime.Now),
                    Status = "Payment Completed"
                };
                var _customerProject = await _context.customerProjects
                    .FirstOrDefaultAsync(cp => cp.CustomerProjectId == payment.CustomerProjectId);

                if (_customerProject != null)
                {
                    // Update the customer project status
                    _customerProject.Status = "Payment Completed";
                    _context.customerProjects.Update(_customerProject);
                }



                _context.payments.Add(_payment);
                await _context.SaveChangesAsync();

                int paymentId = _payment.PaymentId;

                // Send confirmation or notification to customer and admin
                // Redirect to a confirmation page
                return RedirectToAction("PaymentConfirmation", new { paymentId = paymentId });
            }

            return View(payment);
        }


        public IActionResult PaymentConfirmation(int paymentId)
        {
            // Retrieve the payment details directly from the context
            Payment payment = _context.payments.FirstOrDefault(p => p.PaymentId == paymentId);

            // If payment is null, handle the error (optional)
            if (payment == null)
            {
                // Handle error (e.g., payment not found)
                return NotFound();
            }

            // Pass the payment model to the view
            return View(payment);
        }

    }
}
