using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringToolsEquipmentsInventory.Models;

namespace EngineeringToolsEquipmentsInventory.Models
{
    public class iJosDatabaseContext : DbContext
    {
        public DbSet<JigPriceList> JigPriceLists { get; set; } 
        public DbSet<Receiving> Receivings { get; set; } 
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; } 
        public DbSet<JigClassification> JigClassifications { get; set; } 
      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new Initializer());
            modelBuilder.Entity<JigPriceList>().ToTable("JigPriceList", "dbo"); 
            modelBuilder.Entity<Receiving>().ToTable("Receiving", "dbo"); 
            modelBuilder.Entity<PurchaseOrder>().ToTable("PurchaseOrder", "dbo"); 
            modelBuilder.Entity<JigClassification>().ToTable("Classification", "dbo"); 
  
        }
        public class Initializer : IDatabaseInitializer<iJosDatabaseContext>
        {
            public void InitializeDatabase(iJosDatabaseContext context)
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    Seed(context);
                    context.SaveChanges();
                }
            }

            private void Seed(iJosDatabaseContext context)
            {
                throw new NotImplementedException();
            }
        }

    }
}

