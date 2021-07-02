using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using BC = BCrypt.Net.BCrypt;

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

                entity.HasIndex(e => e.OrderId, "IX_DesignerOrderInfoID_OrderID");

                entity.HasIndex(e => e.UserId, "IX_DesignerOrderInfoID_UserID");

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

                entity.HasIndex(e => e.ServiceId, "IX_OrderInfo_ServiceID");

                entity.HasIndex(e => e.UserId, "IX_OrderInfo_UserID");

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
                
                
                entity.HasData(new List<Role>
                {
                    new Role
                    {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    new Role
                    {
                        RoleId = 2,
                        RoleName = "Designer"
                    },
                    new Role
                    {
                        RoleId = 3,
                        RoleName = "User"
                    }
                });
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

                entity.HasData(new List<Service>
                {
                    new Service
                    {
                        ServiceId = 1,
                        ServiceName = "Макет сайта",
                        DefaultPrice = 1000,
                        ServiceDescription = "Разработка макета веб-сайта под Ваши нужды."
                    },
                    new Service
                    {
                        ServiceId = 2,
                        ServiceName = "Логотип",
                        DefaultPrice = 300,
                        ServiceDescription = "Разработка логотипа, идеального подчеркивающего суть Вашего бизнеса."
                    },
                    new Service
                    {
                        ServiceId = 3,
                        ServiceName = "Фирменный стиль",
                        DefaultPrice = 600,
                        ServiceDescription = "Разработка фирменного стиля, чтобы Ваш бизнес выглядел уникально" +
                                             " и узнаваемо."
                    },
                    new Service
                    {
                        ServiceId = 4,
                        ServiceName = "Баннер",
                        DefaultPrice = 100,
                        ServiceDescription = "Разработка баннера под любые Ваши нужды."
                    }
                });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.RoleId, "IX_User_RoleID");

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

                entity.HasData(new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        FirstName = "admin",
                        LastName = null,
                        Email = "admin@gg",
                        Password = BC.HashPassword("admin12345"),
                        Tel = "1",
                        RoleId = 1,
                        EmailConfirmed = true,
                        Token = null
                    },
                    new User
                    {
                        UserId = 2,
                        FirstName = "designer",
                        LastName = "first",
                        Email = "designer1@gg",
                        Password = BC.HashPassword("designer12345"),
                        Tel = "2",
                        RoleId = 2,
                        EmailConfirmed = true,
                        Token = null
                    },
                    new User
                    {
                        UserId = 3,
                        FirstName = "designer",
                        LastName = "second",
                        Email = "designer2@gg",
                        Password = BC.HashPassword("designer12345"),
                        Tel = "3",
                        RoleId = 2,
                        EmailConfirmed = true,
                        Token = null
                    },
                    new User
                    {
                        UserId = 4,
                        FirstName = "designer",
                        LastName = "third",
                        Email = "designer3@gg",
                        Password = BC.HashPassword("designer12345"),
                        Tel = "4",
                        RoleId = 2,
                        EmailConfirmed = true,
                        Token = null
                    },
                    new User
                    {
                        UserId = 5,
                        FirstName = "designer",
                        LastName = "fourth",
                        Email = "designer4@gg",
                        Password = BC.HashPassword("designer12345"),
                        Tel = "5",
                        RoleId = 2,
                        EmailConfirmed = true,
                        Token = null
                    }
                });
            });

            modelBuilder.Entity<UserWork>(entity =>
            {
                entity.ToTable("UserWork");

                entity.HasIndex(e => e.UserId, "IX_UserWork_UserID");

                entity.HasIndex(e => e.WorkId, "IX_UserWork_WorkID");

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

                entity.HasData(new List<UserWork>
                {
                    new UserWork
                    {
                        UserWorkId = 1,
                        UserId = 2,
                        WorkId = 1
                    },
                    new UserWork
                    {
                        UserWorkId = 2,
                        UserId = 3,
                        WorkId = 1
                    },
                    new UserWork
                    {
                        UserWorkId = 3,
                        UserId = 3,
                        WorkId = 2
                    },
                    new UserWork
                    {
                        UserWorkId = 4,
                        UserId = 4,
                        WorkId = 3
                    },
                    new UserWork
                    {
                        UserWorkId = 5,
                        UserId = 4,
                        WorkId = 4
                    },
                    new UserWork
                    {
                        UserWorkId = 6,
                        UserId = 5,
                        WorkId = 4
                    }
                });
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("Work");

                entity.Property(e => e.WorkId).HasColumnName("WorkID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasDefaultValueSql("(CONVERT([date],getdate()))");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.WorkName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Work_Service");

                entity.HasData(new List<Work>
                {
                    new Work
                    {
                        WorkId = 1,
                        Description = "Работа 1",
                        Date = DateTime.Now,
                        WorkName = "Работа 1",
                        ServiceId = 1
                    },
                    new Work
                    {
                        WorkId = 2,
                        Description = "Работа 2",
                        Date = DateTime.Now,
                        WorkName = "Работа 2",
                        ServiceId = 2
                    },
                    new Work
                    {
                        WorkId = 3,
                        Description = "Работа 3",
                        Date = DateTime.Now,
                        WorkName = "Работа 3",
                        ServiceId = 3
                    },
                    new Work
                    {
                        WorkId = 4,
                        Description = "Работа 4",
                        Date = DateTime.Now,
                        WorkName = "Работа 4",
                        ServiceId = 4
                    },
                    
                });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
