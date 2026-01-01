using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;

namespace PaperProcessor.Controllers
{
    public class HomeController : Controller
    {
        private readonly PaperProcessorContext _context;

        public HomeController(PaperProcessorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var in7Days = today.AddDays(7);

            // KPI counts
            var totalWorkOrders = await _context.WorkOrders.CountAsync();
            var wipWorkOrders = await _context.WorkOrders.CountAsync(w =>
                w.Status == WorkOrderStatus.Approved || w.Status == WorkOrderStatus.InProduction);

            var overdueWorkOrders = await _context.WorkOrders.CountAsync(w =>
                w.DueDate < today && w.Status != WorkOrderStatus.Completed && w.Status != WorkOrderStatus.Cancelled);

            var totalInvoices = await _context.Invoices.CountAsync();

            // Revenue (from invoices)
            var totalRevenue = await _context.Invoices
                .Where(i => i.Status != InvoiceStatus.Cancelled)
                .SumAsync(i => (decimal?)i.Total) ?? 0m;

            // Due soon list
            var dueSoon = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .Where(w => w.DueDate >= today && w.DueDate <= in7Days &&
                            w.Status != WorkOrderStatus.Completed &&
                            w.Status != WorkOrderStatus.Cancelled)
                .OrderBy(w => w.DueDate)
                .Take(8)
                .ToListAsync();

            // Recent work orders
            var recent = await _context.WorkOrders
                .Include(w => w.Customer)
                .OrderByDescending(w => w.CreatedAt)
                .Take(8)
                .ToListAsync();

            ViewBag.TotalWorkOrders = totalWorkOrders;
            ViewBag.WipWorkOrders = wipWorkOrders;
            ViewBag.OverdueWorkOrders = overdueWorkOrders;
            ViewBag.TotalInvoices = totalInvoices;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.DueSoon = dueSoon;
            ViewBag.Recent = recent;

            return View();
        }
    }
}
