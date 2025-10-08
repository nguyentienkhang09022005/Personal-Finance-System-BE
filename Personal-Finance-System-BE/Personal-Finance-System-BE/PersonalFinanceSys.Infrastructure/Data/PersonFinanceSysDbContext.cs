using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

public partial class PersonFinanceSysDbContext : DbContext
{
    public PersonFinanceSysDbContext(DbContextOptions<PersonFinanceSysDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<InvalidatedToken> InvalidatedTokens { get; set; }

    public virtual DbSet<Otp> Otps { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvalidatedToken>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("invalidated_token_pkey");

            entity.ToTable("invalidated_token");

            entity.Property(e => e.IdToken)
                .ValueGeneratedNever()
                .HasColumnName("id_token");
            entity.Property(e => e.ExpiryTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiry_time");
        });

        modelBuilder.Entity<Otp>(entity =>
        {
            entity.HasKey(e => e.IdOtp).HasName("otp_pkey");

            entity.ToTable("otp");

            entity.Property(e => e.IdOtp)
                .ValueGeneratedNever()
                .HasColumnName("id_otp");
            entity.Property(e => e.ExpiryTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expiry_time");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Otps)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_otp_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phone, "users_phone_key").IsUnique();

            entity.Property(e => e.IdUser)
                .ValueGeneratedNever()
                .HasColumnName("id_user");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(5)
                .HasColumnName("gender");
            entity.Property(e => e.IsDarkmode)
                .HasDefaultValue(false)
                .HasColumnName("is_darkmode");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
