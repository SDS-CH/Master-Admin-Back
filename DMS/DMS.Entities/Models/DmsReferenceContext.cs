#nullable disable
using Microsoft.EntityFrameworkCore;

namespace DMS.Entities.Models;

public partial class DmsReferenceContext : DbContext
{
    public DmsReferenceContext(DbContextOptions<DmsReferenceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Department { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("department");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.DepartmentColor).HasColumnName("department_color");
            entity.Property(e => e.EditNewTime).HasColumnName("edit_new_time");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.IndustryId).HasColumnName("industry_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
