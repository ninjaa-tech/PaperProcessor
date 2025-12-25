using Microsoft.EntityFrameworkCore;

namespace PaperProcessor.Data
{
    public class PaperProcessorContext : DbContext
    {
        public PaperProcessorContext(DbContextOptions<PaperProcessorContext> options)
            : base(options)
        {
        }

        // We'll add DbSet<> here soon (Customers, WorkOrders, Materials, Invoices...)
    }
}
