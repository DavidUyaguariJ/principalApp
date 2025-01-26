using System;
using System.Collections.Generic;
using EntityModelsPrincipalApp.Models.App;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsPrincipalApp.Models.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TProdClient> TProdClients { get; set; }

    public virtual DbSet<TProdProduct> TProdProducts { get; set; }

    public virtual DbSet<TProdSupplier> TProdSuppliers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:dbo-finalproject.database.windows.net,1433;Initial Catalog=finalexam;Persist Security Info=False;User ID=dboAdmin;Password=0902247D@y4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
