using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TrainingProject.Models
{
    public partial class GasStationContext : DbContext
    {
        public GasStationContext()
        {
        }

        public GasStationContext(DbContextOptions<GasStationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GasStation> GasStation { get; set; }
        public virtual DbSet<GasStationFeedback> GasStationFeedback { get; set; }
        public virtual DbSet<GasStationGasType> GasStationGasType { get; set; }
        public virtual DbSet<MDistrict> MDistrict { get; set; }
        public virtual DbSet<MType> MType { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=192.168.1.5;Initial Catalog=GasStation;Integrated Security=False;User ID=sa;Password=hpbvn123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GasStation>(entity =>
            {
                entity.HasMany(e => e.GasStationGasType).WithOne(c => c.GasStation).OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Address).HasMaxLength(200);

                entity.Property(e => e.DeletedAt).HasColumnType("datetime");

                entity.Property(e => e.GasStationName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.InsertedAt).HasColumnType("datetime");

                entity.Property(e => e.OpeningTime).HasMaxLength(50);

                entity.Property(e => e.Rating)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<GasStationFeedback>(entity =>
            {
                entity.HasIndex(e => e.GasStationFeedbackId)
                    .HasName("IX_GasStationFeedback");

                entity.Property(e => e.Feedback)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.FeedbackAt).HasColumnType("datetime");

                entity.HasOne(d => d.GasStation)
                    .WithMany(p => p.GasStationFeedback)
                    .HasForeignKey(d => d.GasStationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GasStationFeedback_GasStation1");
            });

            modelBuilder.Entity<GasStationGasType>(entity =>
            {
                entity.Property(e => e.GasType)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.GasStation)
                    .WithMany(p => p.GasStationGasType)
                    .HasForeignKey(d => d.GasStationId)
                    .HasConstraintName("FK_GasStationGasType_GasStation1");
            });

            modelBuilder.Entity<MDistrict>(entity =>
            {
                entity.HasKey(e => e.DistrictId);

                entity.ToTable("M_District");

                entity.Property(e => e.DistrictName).HasMaxLength(50);
            });

            modelBuilder.Entity<MType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.ToTable("M_Type");

                entity.Property(e => e.TypeCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.TypeText)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .IsRequired()
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });
        }

        internal Task FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
    }
}
