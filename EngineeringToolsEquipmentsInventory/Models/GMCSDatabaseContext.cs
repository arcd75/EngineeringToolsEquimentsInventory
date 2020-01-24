using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringToolsEquipmentsInventory.Models;

namespace EngineeringToolsEquipmentsInventory.Models
{
    public class GMCSDatabaseContext : DbContext
    {
        public DbSet<single_issue_tran> single_issue_trans { get; set; }
        public DbSet<bom_mst_dtl> bom_mst_dtls { get; set; }
        public DbSet<product_mst> product_msts { get; set; }
        public DbSet<material_mst> material_msts { get; set; }
        public DbSet<bom_mst_dtl_qry> bom_Mst_Dtl_Qries { get; set; }
        public DbSet<line_mst> line_Msts { get; set; }
 
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new Initializer());
            modelBuilder.Entity<single_issue_tran>().ToTable("single_issue_tran", "dbo");
            modelBuilder.Entity<bom_mst_dtl>().ToTable("bom_mst_dtl", "dbo");
            modelBuilder.Entity<product_mst>().ToTable("product_mst", "dbo");
            modelBuilder.Entity<material_mst>().ToTable("material_mst", "dbo");  
            modelBuilder.Entity<line_mst>().ToTable("line_mst", "dbo");  
        }
        public class Initializer : IDatabaseInitializer<GMCSDatabaseContext>
        {
            public void InitializeDatabase(GMCSDatabaseContext context)
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    Seed(context);
                    context.SaveChanges(); 
                }
            }

            private void Seed(GMCSDatabaseContext context)
            {
                throw new NotImplementedException();
            }
        }

    }
}

