using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

public partial class PersonFinanceSysDbContext : DbContext
{
    public PersonFinanceSysDbContext()
    {
    }

    public PersonFinanceSysDbContext(DbContextOptions<PersonFinanceSysDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=ep-odd-wave-a1mzcz0k-pooler.ap-southeast-1.aws.neon.tech;Port=5432;Database=neondb;Username=neondb_owner;Password=npg_GuPr1xNw6OEy;SSL Mode=Require;Trust Server Certificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
