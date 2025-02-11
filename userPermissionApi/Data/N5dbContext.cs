using Microsoft.EntityFrameworkCore;
using userPermissionApi.Models;

namespace userPermissionApi.Data;

public partial class N5dbContext : DbContext
{
    public N5dbContext()
    {
    }

    public N5dbContext(DbContextOptions<N5dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<TipoPermiso> TipoPermisos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__Permisos__3213E83FA89E4464");

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.apellidoEmpleado).HasColumnType("text");
            entity.Property(e => e.nombreEmpleado).HasColumnType("text");

            entity.HasOne(d => d.oTipoPermiso).WithMany(p => p.Permisos)
                .HasForeignKey(d => d.tipoPermiso)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permisos__TipoPe__4BAC3F29");
        });

        modelBuilder.Entity<TipoPermiso>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TipoPerm__3213E83FA9E1FA77");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descripcion).HasColumnType("text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
