using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace reactAzure.Data
{
    public partial class TechCMSContext : DbContext
    {
        public TechCMSContext()
        {
        }

        public TechCMSContext(DbContextOptions<TechCMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:TechCMSDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RId)
                    .HasName("PK_Roles");

                entity.ToTable("Role");

                entity.Property(e => e.RId).HasColumnName("rID");

                entity.Property(e => e.RActive)
                    .IsRequired()
                    .HasColumnName("rActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.RAdministrator).HasColumnName("rAdministrator");

                entity.Property(e => e.RCreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rCreatedBy");

                entity.Property(e => e.RCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rCreatedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rModifiedBy");

                entity.Property(e => e.RModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("rModifiedDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.RName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("rName");

                entity.Property(e => e.RSupervisor).HasColumnName("rSupervisor");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UsrId);

                entity.ToTable("USERS");

                entity.Property(e => e.UsrId)
                    .ValueGeneratedNever()
                    .HasColumnName("USR_ID");

                entity.Property(e => e.UsrActive)
                    .HasColumnName("USR_ACTIVE")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UsrApps).HasColumnName("USR_APPS");

                entity.Property(e => e.UsrClock).HasColumnName("USR_CLOCK");

                entity.Property(e => e.UsrCreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USR_CREATED_BY");

                entity.Property(e => e.UsrCreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("USR_CREATED_DATE");

                entity.Property(e => e.UsrCsdbName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USR_CSDB_NAME");

                entity.Property(e => e.UsrDefaultRole).HasColumnName("USR_DefaultRole");

                entity.Property(e => e.UsrEmail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USR_EMAIL");

                entity.Property(e => e.UsrFirst)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USR_FIRST");

                entity.Property(e => e.UsrFlags).HasColumnName("USR_FLAGS");

                entity.Property(e => e.UsrFte).HasColumnName("USR_FTE");

                entity.Property(e => e.UsrLast)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USR_LAST");

                entity.Property(e => e.UsrLogin)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USR_LOGIN");

                entity.Property(e => e.UsrModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USR_MODIFIED_BY");

                entity.Property(e => e.UsrModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("USR_MODIFIED_DATE");

                entity.Property(e => e.UsrModifiedTimestamp)
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("USR_MODIFIED_TIMESTAMP");

                entity.Property(e => e.UsrNetLogin)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USR_NET_LOGIN");

                entity.Property(e => e.UsrPassword)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("USR_PASSWORD");

                entity.Property(e => e.UsrSettings)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("USR_SETTINGS");

                entity.HasOne(d => d.UsrDefaultRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UsrDefaultRole)
                    .HasConstraintName("FK_USERS_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
