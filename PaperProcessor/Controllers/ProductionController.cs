using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;

namespace PaperProcessor.Controllers
{
    public class ProductionController : Controller
    {
        private readonly PaperProcessorContext _context;

        public ProductionController(PaperProcessorContext context)
        {
            _context = context;
        }

        // /Production/Timeline/5
        public async Task<IActionResult> Timeline(int id)
        {
            var workOrder = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .FirstOrDefaultAsync(w => w.WorkOrderId == id);

            if (workOrder == null) return NotFound();

            var stages = await _context.ProductionStages
                .OrderBy(s => s.SortOrder)
                .ToListAsync();

            var logs = await _context.StageLogs
                .Where(l => l.WorkOrderId == id)
                .ToListAsync();

            // Ensure every stage has a log row (auto create missing)
            foreach (var stage in stages)
            {
                if (!logs.Any(l => l.ProductionStageId == stage.ProductionStageId))
                {
                    _context.StageLogs.Add(new StageLog
                    {
                        WorkOrderId = id,
                        ProductionStageId = stage.ProductionStageId
                    });
                }
            }

            if (_context.ChangeTracker.HasChanges())
                await _context.SaveChangesAsync();

            // Reload logs with stage data
            var timeline = await _context.StageLogs
                .Include(l => l.ProductionStage)
                .Where(l => l.WorkOrderId == id)
                .OrderBy(l => l.ProductionStage!.SortOrder)
                .ToListAsync();

            ViewBag.WorkOrder = workOrder;
            return View(timeline);
        }

        [HttpPost]
        public async Task<IActionResult> Start(int stageLogId)
        {
            var log = await _context.StageLogs.FindAsync(stageLogId);
            if (log == null) return NotFound();

            if (log.StartedAt == null)
                log.StartedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Timeline), new { id = log.WorkOrderId });
        }

        [HttpPost]
        public async Task<IActionResult> Finish(int stageLogId, int qtyOut, int scrapQty, string? notes)
        {
            var log = await _context.StageLogs.FindAsync(stageLogId);
            if (log == null) return NotFound();

            log.EndedAt = DateTime.UtcNow;
            log.QtyOut = qtyOut;
            log.ScrapQty = scrapQty;
            log.Notes = notes;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Timeline), new { id = log.WorkOrderId });
        }
    }
}
