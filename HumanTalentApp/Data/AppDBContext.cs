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
        public virtual DbSet<TAudtAlignmentObjetive> TAudtAlignmentObjetives { get; set; }

        public virtual DbSet<TAudtEmpresarialObjetive> TAudtEmpresarialObjetives { get; set; }

        public virtual DbSet<TAudtGovermentObjetive> TAudtGovermentObjetives { get; set; }
        public virtual DbSet<TProdClient> TProdClients { get; set; }

        public virtual DbSet<TProdProduct> TProdProducts { get; set; }

        public virtual DbSet<TProdSupplier> TProdSuppliers { get; set; }

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
            modelBuilder.Entity<TAudtAlignmentObjetive>(entity =>
            {
                entity.Property(e => e.IdeAlignmentObjetive).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TAudtEmpresarialObjetive>(entity =>
            {
                entity.HasKey(e => e.IdeEmpresarialObjetive).HasName("PK_T_AUDT_EMPRESARIAL_OBJETIVE");

                entity.Property(e => e.IdeEmpresarialObjetive).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TAudtGovermentObjetive>(entity =>
            {
                entity.Property(e => e.IdeGuvermentObjetive).HasDefaultValueSql("(newid())");
            });
            modelBuilder.Entity<TProdClient>(entity =>
            {
                entity.HasKey(e => e.IdeClient).HasName("PK__T_PROD_C__CEA8C3C2EDE70CE8");

                entity.Property(e => e.IdeClient).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TProdProduct>(entity =>
            {
                entity.HasKey(e => e.IdeProduct).HasName("PK__T_PROD_P__9D664B37B54EE095");

                entity.Property(e => e.IdeProduct).HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<TProdSupplier>(entity =>
            {
                entity.HasKey(e => e.IdeSupplier).HasName("PK__T_PROD_S__07AD67C1FE5956AE");

                entity.Property(e => e.IdeSupplier).HasDefaultValueSql("(newid())");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
