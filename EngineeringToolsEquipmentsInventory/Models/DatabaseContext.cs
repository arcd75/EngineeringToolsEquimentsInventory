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
        public DbSet<JOSDelivery> JOSDeliveries { get; set; }
        public DbSet<JigPurchaseOrder> JigPurchaseOrders { get; set; }
        public DbSet<UOM> UOMs { get; set; }
        public DbSet<DeliveryItemSparePart> DeliveryItemSpareParts { get; set; }
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<ToolCondition> ToolConditions { get; set; }
        public DbSet<ToolMst> ToolMsts { get; set; }
        public DbSet<ItemMst> ItemMsts { get; set; }
        public DbSet<AssetAndEquipment> AssetAndEquipments { get; set; }
        public DbSet<AssetAndEquipmentTransaction> AssetAndEquipmentTransactions { get; set; }
        public DbSet<AssetTransactionItem> AssetTransactionItems { get; set; }
        public DbSet<DeliveryAsset> DeliveryAssets { get; set; }
        public DbSet<JOSUpdate> JOSUpdates { get; set; }


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
            modelBuilder.Entity<JOSDelivery>().ToTable("JOSDelivery", "public");
            modelBuilder.Entity<JigPurchaseOrder>().ToTable("JigPurchaseOrder", "public");
            modelBuilder.Entity<UOM>().ToTable("UOM", "public");
            modelBuilder.Entity<DeliveryItemSparePart>().ToTable("DeliveryItemSparePart", "public");
            modelBuilder.Entity<Classification>().ToTable("Classification", "public");
            modelBuilder.Entity<ToolCondition>().ToTable("ToolCondition", "public");
            modelBuilder.Entity<ToolMst>().ToTable("ToolMst", "public");
            modelBuilder.Entity<ItemMst>().ToTable("ItemMst", "public");
            modelBuilder.Entity<AssetAndEquipment>().ToTable("AssetAndEquipment", "public");
            modelBuilder.Entity<AssetAndEquipmentTransaction>().ToTable("AssetAndEquipmentTransaction", "public");
            modelBuilder.Entity<AssetTransactionItem>().ToTable("AssetTransactionItem", "public");
            modelBuilder.Entity<DeliveryAsset>().ToTable("DeliveryAsset", "public");
            modelBuilder.Entity<JOSUpdate>().ToTable("JOSUpdate", "public");
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