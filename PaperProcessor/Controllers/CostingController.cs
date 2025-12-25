using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.ViewModels;

namespace PaperProcessor.Controllers
{
    public class CostingController : Controller
    {
        private readonly PaperProcessorContext _context;

        public CostingController(PaperProcessorContext context)
        {
            _context = context;
        }

        // /Costing/Summary/5
        public async Task<IActionResult> Summary(int id)
        {
            var wo = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .FirstOrDefaultAsync(w => w.WorkOrderId == id);

            if (wo == null) return NotFound();

            var materialCost = await _context.MaterialUsages
                .Where(u => u.WorkOrderId == id)
                .SumAsync(u => u.QuantityUsed * u.CostPerUnitSnapshot);

            var laborCost = await _context.StageLogs
                .Where(l => l.WorkOrderId == id && l.EndedAt != null)
                .SumAsync(l => (l.LaborMinutes / 60m) * l.HourlyRateSnapshot);

            var vm = new WorkOrderCostingVm
            {
                WorkOrderId = wo.WorkOrderId,
                WorkOrderNo = wo.WorkOrderNo,
                CustomerName = wo.Customer?.Name ?? "",
                ProductName = wo.ProductCarton?.Name ?? "",
                QuantityOrdered = wo.QuantityOrdered,
                UnitSellPrice = wo.UnitSellPrice,
                MaterialCost = materialCost,
                LaborCost = laborCost
            };

            return View(vm);
        }
    }
}
