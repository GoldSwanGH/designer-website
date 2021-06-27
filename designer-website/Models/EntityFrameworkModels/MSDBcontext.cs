using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace designer_website.Models.EntityFrameworkModels
{
    public partial class MSDBcontext : DbContext
    {
        public MSDBcontext()
        {
        }

        public MSDBcontext(DbContextOptions<MSDBcontext> options)
            : base(options)
        {
        }

        public virtual DbSet<DesignerOrderInfoId> DesignerOrderInfoIds { get; set; }
        public virtual DbSet<OrderInfo> OrderInfos { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserWork> UserWorks { get; set; }
        public virtual DbSet<Work> Works { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:DesignerWebsiteDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<DesignerOrderInfoId>(entity =>
            {
                entity.HasKey(e => e.DesignerOrderInfoId1);

                entity.ToTable("DesignerOrderInfoID");

                entity.Property(e => e.DesignerOrderInfoId1).HasColumnName("DesignerOrderInfoID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DesignerOrderInfoIds)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesignerOrderInfoID_OrderInfo");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DesignerOrderInfoIds)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DesignerOrderInfoID_User");
            });

            modelBuilder.Entity<OrderInfo>(entity =>
            {
                entity.HasKey(e => e.OrderId);

                entity.ToTable("OrderInfo");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.OrderDescription).IsUnicode(false);

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_OrderInfo_Service");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OrderInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderInfo_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.RoleId, "UK_Role_RoleName")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.HasIndex(e => e.ServiceId, "UK_Service_ServiceName")
                    .IsUnique();

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.ServiceDescription).IsUnicode(false);

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UK_User_Email")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .HasDefaultValueSql("((3))");

                entity.Property(e => e.Tel)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<UserWork>(entity =>
            {
                entity.ToTable("UserWork");

                entity.Property(e => e.UserWorkId).HasColumnName("UserWorkID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserWorks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserWork_User");

                entity.HasOne(d => d.Work)
                    .WithMany(p => p.UserWorks)
                    .HasForeignKey(d => d.WorkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserWork_Work");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("Work");

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.WorkName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
