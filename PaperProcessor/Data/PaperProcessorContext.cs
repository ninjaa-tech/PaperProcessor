using Microsoft.EntityFrameworkCore;
using PaperProcessor.Models;

namespace PaperProcessor.Data
{
    public class PaperProcessorContext : DbContext
    {
        public PaperProcessorContext(DbContextOptions<PaperProcessorContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<ProductCarton> ProductCartons => Set<ProductCarton>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<ProductionStage> ProductionStages => Set<ProductionStage>();
        public DbSet<StageLog> StageLogs => Set<StageLog>();
        public DbSet<Material> Materials => Set<Material>();
        public DbSet<MaterialUsage> MaterialUsages => Set<MaterialUsage>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNo)
                .IsUnique();


            modelBuilder.Entity<StageLog>()
                .HasIndex(x => new { x.WorkOrderId, x.ProductionStageId })
                .IsUnique();

            // Unique WorkOrderNo
            modelBuilder.Entity<WorkOrder>()
                .HasIndex(w => w.WorkOrderNo)
                .IsUnique();

            // Seed fixed stages (single factory standard)
            modelBuilder.Entity<ProductionStage>().HasData(
                new ProductionStage { ProductionStageId = 1, Name = "Cutting", SortOrder = 1 },
                new ProductionStage { ProductionStageId = 2, Name = "Printing", SortOrder = 2 },
                new ProductionStage { ProductionStageId = 3, Name = "Folding", SortOrder = 3 },
                new ProductionStage { ProductionStageId = 4, Name = "Gluing", SortOrder = 4 },
                new ProductionStage { ProductionStageId = 5, Name = "QC", SortOrder = 5 },
                new ProductionStage { ProductionStageId = 6, Name = "Packing", SortOrder = 6 }
            );
        }
    }
}
