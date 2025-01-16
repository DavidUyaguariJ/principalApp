using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EntityModelsHumanTalentApp.Models.Context;

public partial class AppDbContext : DbContext
{
    private AppDbContext()
    {
    }

    private AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
