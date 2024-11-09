using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace QuanLiCuaHang.Models;

public partial class QlcuahangContext : DbContext
{
    public QlcuahangContext()
    {
    }

    public QlcuahangContext(DbContextOptions<QlcuahangContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<BillDetail> BillDetails { get; set; }

    public virtual DbSet<BlockVoucher> BlockVouchers { get; set; }

    public virtual DbSet<Consignment> Consignments { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<InputInfo> InputInfos { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<SalaryBill> SalaryBills { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Voucher> Vouchers { get; set; }

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
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bill__3214EC07DEDFE6AE");

            entity.ToTable("Bill");

            entity.Property(e => e.BillDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bills)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Bill__CustomerId__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Bills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bill__UserId__6B24EA82");
        });

        modelBuilder.Entity<BillDetail>(entity =>
        {
            entity.HasKey(e => new { e.BillId, e.ProductId });

            entity.ToTable("BillDetail");

            entity.Property(e => e.ProductId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Bill).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.BillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillDetai__BillI__6E01572D");

            entity.HasOne(d => d.Product).WithMany(p => p.BillDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillDetai__Produ__6EF57B66");
        });

        modelBuilder.Entity<BlockVoucher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BlockVou__3214EC0780BA7FF2");

            entity.ToTable("BlockVoucher");

            entity.Property(e => e.ReleaseName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Consignment>(entity =>
        {
            entity.HasKey(e => new { e.InputInfoId, e.ProductId });

            entity.ToTable("Consignment");

            entity.Property(e => e.ProductId)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.InputInfo).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.InputInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InputInfo");

            entity.HasOne(d => d.Product).WithMany(p => p.Consignments)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0754BDE1F1");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Email, "Idx_CustomerEmail_NotNull")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Phone, "Idx_CustomerPhone_NotNull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InputInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InputInf__3214EC07869616D7");

            entity.ToTable("InputInfo");

            entity.HasOne(d => d.Supplier).WithMany(p => p.InputInfos)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__InputInfo__Suppl__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.InputInfos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__InputInfo__UserI__619B8048");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Barcode).HasName("PK__Product__177800D291227589");

            entity.ToTable("Product");

            entity.Property(e => e.Barcode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.ProductionSite)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Report__3214EC07A757A2C2");

            entity.ToTable("Report");

            entity.Property(e => e.FinishDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Image).HasColumnType("image");
            entity.Property(e => e.StartDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SubmittedAt).HasColumnType("smalldatetime");
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Staff).WithMany(p => p.Reports)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Report__StaffId__797309D9");
        });

        modelBuilder.Entity<SalaryBill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SalaryBi__3214EC077BECB50E");

            entity.ToTable("SalaryBill");

            entity.HasOne(d => d.User).WithMany(p => p.SalaryBills)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SalaryBil__UserI__76969D2E");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07850849FD");

            entity.ToTable("Supplier");

            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0778554154");

            entity.HasIndex(e => e.UserName, "Idx_UserAccount_NotNull")
                .IsUnique()
                .HasFilter("([UserName] IS NOT NULL)");

            entity.HasIndex(e => e.Email, "Idx_UserEmail_NotNull")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Phone, "Idx_UserPhone_NotNull")
                .IsUnique()
                .HasFilter("([Phone] IS NOT NULL)");

            entity.Property(e => e.Avatar).HasColumnType("image");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserRole).HasMaxLength(20);
        });

        modelBuilder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PK__Voucher__A25C5AA6B2F8EA46");

            entity.ToTable("Voucher");

            entity.Property(e => e.Code)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Block).WithMany(p => p.Vouchers)
                .HasForeignKey(d => d.BlockId)
                .HasConstraintName("FK__Voucher__BlockId__73BA3083");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
