using EntityModelsHumanTalentApp.Models.App;
using EntityModelsPrincipalApp.Models.App;
using Microsoft.EntityFrameworkCore;

namespace HumanTalentApp.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmnUser> AdmnUsers { get; set; }

        public virtual DbSet<TAdmnRole> TAdmnRoles { get; set; }
        public virtual DbSet<TProdClient> TProdClients { get; set; }

        public virtual DbSet<TProdProduct> TProdProducts { get; set; }

        public virtual DbSet<TProdSupplier> TProdSuppliers { get; set; }
        public virtual DbSet<ProdInventory> ProdInventories { get; set; }

        public virtual DbSet<ProdPurchase> ProdPurchases { get; set; }

        public virtual DbSet<ProdSale> ProdSales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DataBase");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmnUser>(entity =>
            {
                entity.Property(e => e.IdeUser).HasDefaultValueSql("(newid())");

                entity.HasOne(d => d.IdeRoleNavigation).WithMany(p => p.AdmnUsers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_T_ADMN_ROLES_ADMN_USERS");
            });

            modelBuilder.Entity<TAdmnRole>(entity =>
            {
                entity.Property(e => e.IdeRole).HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<TProdClient>(entity =>
            {
                entity.HasKey(e => e.IdeClient).HasName("PK__T_PROD_C__CEA8C3C23CEC9BC8");

                entity.Property(e => e.IdeClient).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TProdProduct>(entity =>
            {
                entity.HasKey(e => e.IdeProduct).HasName("PK__T_PROD_P__9D664B37A9488368");

                entity.Property(e => e.IdeProduct).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TProdSupplier>(entity =>
            {
                entity.HasKey(e => e.IdeSupplier).HasName("PK__T_PROD_S__07AD67C1BDE87DC2");

                entity.Property(e => e.IdeSupplier).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<ProdInventory>(entity =>
            {
                entity.HasKey(e => e.IdeInventory).HasName("PK__PROD_INV__0826DD839EE262A8");

                entity.Property(e => e.IdeInventory).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProdPurchase>(entity =>
            {
                entity.HasKey(e => e.IdePurchase).HasName("PK__PROD_PUR__428CE0E1201E8FE2");

                entity.Property(e => e.IdePurchase).ValueGeneratedNever();
            });

            modelBuilder.Entity<ProdSale>(entity =>
            {
                entity.HasKey(e => e.IdeSale).HasName("PK__PROD_SAL__D72E2E794647A099");

                entity.Property(e => e.IdeSale).ValueGeneratedNever();
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
