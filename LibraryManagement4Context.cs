using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement_PRJ01.Models;

public partial class LibraryManagement4Context : DbContext
{
    public LibraryManagement4Context()
    {
    }

    public LibraryManagement4Context(DbContextOptions<LibraryManagement4Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<LogMuonSach> LogMuonSaches { get; set; }

    public virtual DbSet<MuonSach> MuonSaches { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    public virtual DbSet<SinhVienRegister> SinhVienRegisters { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<YeuCauMuonSach> YeuCauMuonSaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
		Console.WriteLine(Directory.GetCurrentDirectory());
		IConfiguration config = new ConfigurationBuilder()
		.SetBasePath(Directory.GetCurrentDirectory())
		.AddJsonFile("appsettings.json", true, true)
		.Build();
		var strConn = config["ConnectionStrings:MyDatabase"];
		optionsBuilder.UseSqlServer(strConn);
	}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__719FE4E84ACE2AB5");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<LogMuonSach>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogMuonS__5E5499A85B9DE851");

            entity.ToTable("LogMuonSach");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.GhiChu).HasMaxLength(255);

            entity.HasOne(d => d.MaMuonNavigation).WithMany(p => p.LogMuonSaches)
                .HasForeignKey(d => d.MaMuon)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__LogMuonSa__MaMuo__5441852A");
        });

        modelBuilder.Entity<MuonSach>(entity =>
        {
            entity.HasKey(e => e.MaMuon).HasName("PK__MuonSach__0A9BE5E02167EEEC");

            entity.ToTable("MuonSach");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.NgayMuon).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SachId).HasColumnName("SachID");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("DangMuon");

            entity.HasOne(d => d.Admin).WithMany(p => p.MuonSaches)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__MuonSach__AdminI__5165187F");

            entity.HasOne(d => d.MaSinhVienNavigation).WithMany(p => p.MuonSaches)
                .HasForeignKey(d => d.MaSinhVien)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MuonSach__MaSinh__4CA06362");

            entity.HasOne(d => d.Sach).WithMany(p => p.MuonSaches)
                .HasForeignKey(d => d.SachId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MuonSach__SachID__4D94879B");
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.SachId).HasName("PK__Sach__F3005E3A16336D7A");

            entity.ToTable("Sach");

            entity.Property(e => e.SachId).HasColumnName("SachID");
            entity.Property(e => e.NhaXuatBan).HasMaxLength(100);
            entity.Property(e => e.SoLuong).HasDefaultValue(0);
            entity.Property(e => e.TacGia).HasMaxLength(100);
            entity.Property(e => e.TheLoaiId).HasColumnName("TheLoaiID");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.TheLoai).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TheLoaiId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Sach__TheLoaiID__412EB0B6");
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.MaSinhVien).HasName("PK__SinhVien__939AE775261BB2E3");

            entity.ToTable("SinhVien");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Lop).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<SinhVienRegister>(entity =>
        {
            entity.HasKey(e => e.MaSinhVien).HasName("PK__SinhVien__939AE77513B9A26A");

            entity.ToTable("SinhVienRegister");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Lop).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.SoDienThoai).HasMaxLength(15);
            entity.Property(e => e.Ten).HasMaxLength(100);
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.TheLoaiId).HasName("PK__TheLoai__241C3B3A51632B41");

            entity.ToTable("TheLoai");

            entity.Property(e => e.TheLoaiId).HasColumnName("TheLoaiID");
            entity.Property(e => e.TenTheLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.ThongBaoId).HasName("PK__ThongBao__6E51A53BC001D597");

            entity.ToTable("ThongBao");

            entity.Property(e => e.ThongBaoId).HasColumnName("ThongBaoID");
            entity.Property(e => e.ThongBaoCate).HasMaxLength(100);
            entity.Property(e => e.ThongBaoName).HasMaxLength(100);
        });

        modelBuilder.Entity<YeuCauMuonSach>(entity =>
        {
            entity.HasKey(e => e.MaYeuCau).HasName("PK__YeuCauMu__CFA5DF4E7F3166E0");

            entity.ToTable("YeuCauMuonSach");

            entity.Property(e => e.AdminId).HasColumnName("AdminID");
            entity.Property(e => e.NgayYeuCau).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SachId).HasColumnName("SachID");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .HasDefaultValue("ChoDuyet");

            entity.HasOne(d => d.Admin).WithMany(p => p.YeuCauMuonSaches)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__YeuCauMuo__Admin__49C3F6B7");

            entity.HasOne(d => d.MaSinhVienNavigation).WithMany(p => p.YeuCauMuonSaches)
                .HasForeignKey(d => d.MaSinhVien)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__YeuCauMuo__MaSin__44FF419A");

            entity.HasOne(d => d.Sach).WithMany(p => p.YeuCauMuonSaches)
                .HasForeignKey(d => d.SachId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__YeuCauMuo__SachI__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
