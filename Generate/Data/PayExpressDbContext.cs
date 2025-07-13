using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PaymentCoreServiceApi.Generate.ModelsGenerate;

namespace PaymentCoreServiceApi.Generate.Data;

public partial class PayExpressDbContext : DbContext
{
    public PayExpressDbContext()
    {
    }

    public PayExpressDbContext(DbContextOptions<PayExpressDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=PaymentCoreDbDev;Username=postgres;Password=1234");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
