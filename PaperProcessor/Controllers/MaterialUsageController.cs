using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;

namespace PaperProcessor.Controllers
{
    public class MaterialUsageController : Controller
    {
        private readonly PaperProcessorContext _context;

        public MaterialUsageController(PaperProcessorContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int workOrderId)
        {
            var usages = await _context.MaterialUsages
                .Include(u => u.Material)
                .Where(u => u.WorkOrderId == workOrderId)
                .ToListAsync();

            ViewBag.WorkOrderId = workOrderId;
            return View(usages);
        }

        public IActionResult Create(int workOrderId)
        {
            ViewBag.WorkOrderId = workOrderId;
            ViewBag.MaterialId = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(
                _context.Materials, "MaterialId", "Name");

            return View(new MaterialUsage { WorkOrderId = workOrderId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(MaterialUsage usage)
        {
            var material = await _context.Materials.FindAsync(usage.MaterialId);
            if (material == null) return BadRequest();

            usage.CostPerUnitSnapshot = material.CostPerUnit;

            _context.MaterialUsages.Add(usage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { workOrderId = usage.WorkOrderId });
        }
    }
}
