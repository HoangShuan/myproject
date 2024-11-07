using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjectQLNV.DuLieu;

public partial class QlnhanVienContext : DbContext
{
    public QlnhanVienContext()
    {
    }

    public QlnhanVienContext(DbContextOptions<QlnhanVienContext> options)
        : base(options)
    {
    }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PhongBan> PhongBans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=Thanh_Tr;Initial Catalog=QLNhanVien;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => new { e.MaNv, e.MaPb }).HasName("PK_NV");

            entity.ToTable("NhanVien");

            entity.Property(e => e.MaNv)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaNV");
            entity.Property(e => e.MaPb)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaPB");
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.Luong).HasColumnType("money");
            entity.Property(e => e.NgaySinh).HasColumnType("datetime");
            entity.Property(e => e.NgoaiNgu).HasMaxLength(50);

            entity.HasOne(d => d.MaPbNavigation).WithMany(p => p.NhanViens)
                .HasForeignKey(d => d.MaPb)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_NV_PB");
        });

        modelBuilder.Entity<PhongBan>(entity =>
        {
            entity.HasKey(e => e.MaPb).HasName("PK__PhongBan__2725E7E40CFDC11E");

            entity.ToTable("PhongBan");

            entity.Property(e => e.MaPb)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("MaPB");
            entity.Property(e => e.TenPb)
                .HasMaxLength(50)
                .HasColumnName("TenPB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
