using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;
using PaperProcessor.Models;
using PaperProcessor.Services;
using QuestPDF.Fluent;
using PaperProcessor.Pdf;


namespace PaperProcessor.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly PaperProcessorContext _context;
        private readonly InvoiceNumberService _invoiceNo;

        public InvoicesController(PaperProcessorContext context, InvoiceNumberService invoiceNo)
        {
            _context = context;
            _invoiceNo = invoiceNo;
        }

        public async Task<IActionResult> Pdf(int id)
        {
            var inv = await _context.Invoices
                .Include(i => i.WorkOrder)!.ThenInclude(w => w.Customer)
                .Include(i => i.WorkOrder)!.ThenInclude(w => w.ProductCarton)
                .Include(i => i.Lines)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (inv == null) return NotFound();

            // Ensure line totals are correct
            foreach (var l in inv.Lines)
                l.LineTotal = l.Qty * l.UnitPrice;

            inv.Subtotal = inv.Lines.Sum(x => x.LineTotal);
            inv.Total = inv.Subtotal - inv.DiscountAmount;

            var document = new InvoicePdfDocument(inv);
            var pdfBytes = document.GeneratePdf();

            var fileName = $"{inv.InvoiceNo}.pdf";
            return File(pdfBytes, "application/pdf", fileName);
        }


        // Generate invoice from WorkOrder
        public async Task<IActionResult> Generate(int workOrderId)
        {
            var wo = await _context.WorkOrders
                .Include(w => w.Customer)
                .Include(w => w.ProductCarton)
                .FirstOrDefaultAsync(w => w.WorkOrderId == workOrderId);

            if (wo == null) return NotFound();

            // If already has invoice, go to details
            var existing = await _context.Invoices
                .FirstOrDefaultAsync(i => i.WorkOrderId == workOrderId);

            if (existing != null)
                return RedirectToAction(nameof(Details), new { id = existing.InvoiceId });

            var invoiceNo = await _invoiceNo.GenerateAsync();

            var lineQty = wo.QuantityOrdered;
            var unitPrice = wo.UnitSellPrice;
            var subtotal = lineQty * unitPrice;

            var inv = new Invoice
            {
                WorkOrderId = wo.WorkOrderId,
                InvoiceNo = invoiceNo,
                IssueDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                Subtotal = subtotal,
                DiscountAmount = 0,
                Total = subtotal,
                Status = InvoiceStatus.Draft,
                Lines = new List<InvoiceLine>
                {
                    new InvoiceLine
                    {
                        Description = $"{wo.ProductCarton?.Name} - {wo.QuantityOrdered} units",
                        Qty = lineQty,
                        UnitPrice = unitPrice,
                        LineTotal = subtotal
                    }
                }
            };

            _context.Invoices.Add(inv);
            await _context.SaveChangesAsync();

            // Optional: update WO status
            wo.Status = WorkOrderStatus.Invoiced;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = inv.InvoiceId });
        }

        public async Task<IActionResult> Details(int id)
        {
            var inv = await _context.Invoices
                .Include(i => i.WorkOrder)!.ThenInclude(w => w.Customer)
                .Include(i => i.WorkOrder)!.ThenInclude(w => w.ProductCarton)
                .Include(i => i.Lines)
                .FirstOrDefaultAsync(i => i.InvoiceId == id);

            if (inv == null) return NotFound();
            return View(inv);
        }

        // PDF action will be added next (QuestPDF)
    }
}
