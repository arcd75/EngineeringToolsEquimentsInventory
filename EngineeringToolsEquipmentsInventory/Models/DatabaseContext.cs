using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EngineeringToolsEquipmentsInventory.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Tool> Tools { get; set; } 
        public DbSet<Loan> Loans { get; set; } 
        public DbSet<LoanedTool> LoanedTools { get; set; } 
        public DbSet<Transaction> Transactions { get; set; } 
        public DbSet<TransactionItem> TransactionItems { get; set; } 
        public DbSet<Consumable> Consumables { get; set; } 
        public DbSet<Delivery> Deliveries { get; set; } 
        public DbSet<DeliveryItems> DeliveriesItem { get; set; } 
        public DbSet<DeliveryToolItems> DeliveriesToolItem { get; set; } 
        public DbSet<ItemType> ItemTypes { get; set; } 
        public DbSet<ProductGroup> ProductGroups { get; set; } 
        public DbSet<Type> ToolTypes { get; set; } 
        public DbSet<SparePart> SpareParts { get; set; } 
        public DbSet<SparePartTransaction> SparePartTransactions { get; set; } 
        public DbSet<SparePartTransactionItem> SparePartTransactionItems { get; set; } 
        public DbSet<Jig> Jigs { get; set; } 
        public DbSet<JigTransaction> JigTransactions { get; set; } 
        public DbSet<JigTransactionItem> JigTransactionItems { get; set; } 
        public DbSet<Location> Locations { get; set; } 
        public DbSet<JigTransactionType> JigTransactionTypes { get; set; } 

  

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new Initializer());
            modelBuilder.Entity<User>().ToTable("User", "public"); 
            modelBuilder.Entity<Tool>().ToTable("Tool", "public"); 
            modelBuilder.Entity<Loan>().ToTable("Loan", "public"); 
            modelBuilder.Entity<LoanedTool>().ToTable("LoanedTool", "public"); 
            modelBuilder.Entity<Transaction>().ToTable("Transaction", "public"); 
            modelBuilder.Entity<TransactionItem>().ToTable("TransactionItem", "public"); 
            modelBuilder.Entity<Consumable>().ToTable("Consumables", "public"); 
            modelBuilder.Entity<Delivery>().ToTable("Delivery", "public"); 
            modelBuilder.Entity<DeliveryItems>().ToTable("DeliveryItems", "public"); 
            modelBuilder.Entity<ItemType>().ToTable("ItemType", "public"); 
            modelBuilder.Entity<DeliveryToolItems>().ToTable("DeliveryToolItems", "public"); 
            modelBuilder.Entity<ProductGroup>().ToTable("ProductGroup", "public"); 
            modelBuilder.Entity<Type>().ToTable("Type", "public"); 
            modelBuilder.Entity<SparePart>().ToTable("SparePart", "public"); 
            modelBuilder.Entity<SparePartTransaction>().ToTable("SparePartTransaction", "public"); 
            modelBuilder.Entity<SparePartTransactionItem>().ToTable("SparePartTransactionItem", "public"); 
            modelBuilder.Entity<Jig>().ToTable("Jig", "public"); 
            modelBuilder.Entity<JigTransaction>().ToTable("JigTransaction", "public"); 
            modelBuilder.Entity<JigTransactionItem>().ToTable("JigTransactionItem", "public"); 
            modelBuilder.Entity<Location>().ToTable("Location", "public"); 
            modelBuilder.Entity<JigTransactionType>().ToTable("JigTransactionType", "public"); 
       
        }
        public class Initializer : IDatabaseInitializer<DatabaseContext>
        {
            public void InitializeDatabase(DatabaseContext context)
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    Seed(context);
                    context.SaveChanges();

                }
            }

            private void Seed(DatabaseContext context)
            {
                throw new NotImplementedException();
            }
        }

    }
}