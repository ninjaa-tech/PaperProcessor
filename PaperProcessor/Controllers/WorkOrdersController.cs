using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;

namespace PaperProcessor.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly PaperProcessorContext _context;

        public WorkOrdersController(PaperProcessorContext context)
        {
            _context = context;
        }

        // GET: WorkOrders
        public async Task<IActionResult> Index(string? search, WorkOrderStatus? status)
        {
            var query = _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(w =>
                    w.WorkOrderNo.Contains(search) ||
                    w.Customer!.Name.Contains(search) ||
                    w.ProductCarton!.Name.Contains(search));
            }

            if (status.HasValue)
                query = query.Where(w => w.Status == status.Value);

            ViewBag.Search = search ?? "";
            ViewBag.Status = status?.ToString() ?? "";
            ViewBag.StatusList = new SelectList(
                Enum.GetValues(typeof(WorkOrderStatus)).Cast<WorkOrderStatus>()
                    .Select(s => new { Id = s, Name = s.ToString() }),
                "Id", "Name", status);

            var result = await query
                .OrderByDescending(w => w.CreatedAt)
                .ToListAsync();

            return View(result);
        }

        // GET: WorkOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .FirstOrDefaultAsync(m => m.WorkOrderId == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // GET: WorkOrders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name");
            ViewData["ProductCartonId"] = new SelectList(_context.ProductCartons, "ProductCartonId", "Name");
            return View();
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkOrderId,WorkOrderNo,CustomerId,ProductCartonId,QuantityOrdered,DueDate,UnitSellPrice,Status,CreatedAt")] WorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", workOrder.CustomerId);
            ViewData["ProductCartonId"] = new SelectList(_context.ProductCartons, "ProductCartonId", "Name", workOrder.ProductCartonId);
            return View(workOrder);
        }

        // GET: WorkOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", workOrder.CustomerId);
            ViewData["ProductCartonId"] = new SelectList(_context.ProductCartons, "ProductCartonId", "Name", workOrder.ProductCartonId);
            return View(workOrder);
        }

        // POST: WorkOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkOrderId,WorkOrderNo,CustomerId,ProductCartonId,QuantityOrdered,DueDate,UnitSellPrice,Status,CreatedAt")] WorkOrder workOrder)
        {
            if (id != workOrder.WorkOrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkOrderExists(workOrder.WorkOrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "Name", workOrder.CustomerId);
            ViewData["ProductCartonId"] = new SelectList(_context.ProductCartons, "ProductCartonId", "Name", workOrder.ProductCartonId);
            return View(workOrder);
        }

        // GET: WorkOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workOrder = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .FirstOrDefaultAsync(m => m.WorkOrderId == id);
            if (workOrder == null)
            {
                return NotFound();
            }

            return View(workOrder);
        }

        // POST: WorkOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workOrder = await _context.WorkOrders.FindAsync(id);
            if (workOrder != null)
            {
                _context.WorkOrders.Remove(workOrder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkOrderExists(int id)
        {
            return _context.WorkOrders.Any(e => e.WorkOrderId == id);
        }
    }
}
