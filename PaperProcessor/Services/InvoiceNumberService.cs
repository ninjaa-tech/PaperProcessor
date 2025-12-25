using Microsoft.EntityFrameworkCore;
using PaperProcessor.Data;

namespace PaperProcessor.Services
{
    public class InvoiceNumberService
    {
        private readonly PaperProcessorContext _context;

        public InvoiceNumberService(PaperProcessorContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync()
        {
            var year = DateTime.Now.Year;
            var prefix = $"INV-{year}-";

            var last = await _context.Invoices
                .Where(i => i.InvoiceNo.StartsWith(prefix))
                .OrderByDescending(i => i.InvoiceNo)
                .Select(i => i.InvoiceNo)
                .FirstOrDefaultAsync();

            var next = 1;
            if (!string.IsNullOrWhiteSpace(last))
            {
                var parts = last.Split('-');
                if (parts.Length == 3 && int.TryParse(parts[2], out var n))
                    next = n + 1;
            }

            return $"{prefix}{next:0000}";
        }
    }
}
