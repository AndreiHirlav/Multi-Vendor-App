using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using WebApplication1.Models;

namespace WebApplication1;

public partial class DbmarketingContext : DbContext
{
    public DbmarketingContext()
    {
    }

    public DbmarketingContext(DbContextOptions<DbmarketingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdmin> TblAdmins { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblProduct> TblProducts { get; set; }

    public virtual DbSet<TblUser> TblUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=dbmarketing;user=root;password=parola12", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<TblAdmin>(entity =>
        {
            entity.HasKey(e => e.AdId).HasName("PRIMARY");

            entity.ToTable("tbl_admin");

            entity.HasIndex(e => e.AdUsername, "username_UNIQUE").IsUnique();

            entity.Property(e => e.AdId).HasColumnName("ad_id");
            entity.Property(e => e.AdPassword)
                .HasMaxLength(50)
                .HasColumnName("ad_password");
            entity.Property(e => e.AdUsername)
                .HasMaxLength(50)
                .HasColumnName("ad_username");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PRIMARY");

            entity.ToTable("tbl_category");

            entity.HasIndex(e => e.CatName, "cat_name_UNIQUE").IsUnique();

            entity.Property(e => e.CatId).HasColumnName("cat_id");
            entity.Property(e => e.CatFkAd).HasColumnName("cat_fk_ad");
            entity.Property(e => e.CatImage)
                .HasMaxLength(1000)
                .HasColumnName("cat_image");
            entity.Property(e => e.CatName)
                .HasMaxLength(50)
                .HasColumnName("cat_name");
            entity.Property(e => e.CatStatus).HasColumnName("cat_status");
        });

        modelBuilder.Entity<TblProduct>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PRIMARY");

            entity.ToTable("tbl_product");

            entity.Property(e => e.ProId).HasColumnName("pro_id");
            entity.Property(e => e.ProDes)
                .HasMaxLength(2000)
                .HasColumnName("pro_des");
            entity.Property(e => e.ProFkCat).HasColumnName("pro_fk_cat");
            entity.Property(e => e.ProFkUser).HasColumnName("pro_fk_user");
            entity.Property(e => e.ProImage)
                .HasMaxLength(1000)
                .HasColumnName("pro_image");
            entity.Property(e => e.ProName)
                .HasMaxLength(50)
                .HasColumnName("pro_name");
            entity.Property(e => e.ProPrice).HasColumnName("pro_price");
        });

        modelBuilder.Entity<TblUser>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PRIMARY");

            entity.ToTable("tbl_user");

            entity.HasIndex(e => e.UContact, "u_contact_UNIQUE").IsUnique();

            entity.HasIndex(e => e.UEmail, "u_email_UNIQUE").IsUnique();

            entity.Property(e => e.UId).HasColumnName("u_id");
            entity.Property(e => e.UContact)
                .HasMaxLength(50)
                .HasColumnName("u_contact");
            entity.Property(e => e.UEmail)
                .HasMaxLength(50)
                .HasColumnName("u_email");
            entity.Property(e => e.UImage)
                .HasMaxLength(1000)
                .HasColumnName("u_image");
            entity.Property(e => e.UName)
                .HasMaxLength(50)
                .HasColumnName("u_name");
            entity.Property(e => e.UPassword)
                .HasMaxLength(50)
                .HasColumnName("u_password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
