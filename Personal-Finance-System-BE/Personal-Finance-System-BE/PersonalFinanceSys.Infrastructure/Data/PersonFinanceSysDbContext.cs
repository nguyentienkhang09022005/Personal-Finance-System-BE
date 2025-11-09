using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data.Entities;

namespace Personal_Finance_System_BE.PersonalFinanceSys.Infrastructure.Data;

public partial class PersonFinanceSysDbContext : DbContext
{
    public PersonFinanceSysDbContext(DbContextOptions<PersonFinanceSysDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Evaluate> Evaluates { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<FinanceDetail> FinanceDetails { get; set; }

    public virtual DbSet<Friendship> Friendships { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<InvalidatedToken> InvalidatedTokens { get; set; }

    public virtual DbSet<InvestmentAsset> InvestmentAssets { get; set; }

    public virtual DbSet<InvestmentDetail> InvestmentDetails { get; set; }

    public virtual DbSet<InvestmentFund> InvestmentFunds { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SavingDetail> SavingDetails { get; set; }

    public virtual DbSet<SavingGoal> SavingGoals { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.IdBudget).HasName("budgets_pkey");

            entity.ToTable("budgets");

            entity.Property(e => e.IdBudget)
                .ValueGeneratedNever()
                .HasColumnName("id_budget");
            entity.Property(e => e.BudgetGoal)
                .HasPrecision(15, 2)
                .HasColumnName("budget_goal");
            entity.Property(e => e.BudgetName)
                .HasMaxLength(80)
                .HasColumnName("budget_name");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_budget_user");
        });

        modelBuilder.Entity<Evaluate>(entity =>
        {
            entity.HasKey(e => e.IdEvaluate).HasName("evaluates_pkey");

            entity.ToTable("evaluates");

            entity.Property(e => e.IdEvaluate)
                .ValueGeneratedNever()
                .HasColumnName("id_evaluate");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Star).HasColumnName("star");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_evaluate_post");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Evaluates)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_evaluate_user");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.IdFavorite).HasName("favorite_pkey");

            entity.ToTable("favorite");

            entity.Property(e => e.IdFavorite)
                .ValueGeneratedNever()
                .HasColumnName("id_favorite");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdPost).HasColumnName("id_post");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IsFavorite)
                .HasDefaultValue(false)
                .HasColumnName("is_favorite");

            entity.HasOne(d => d.IdPostNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.IdPost)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_favorite_post");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_favorite_user");
        });

        modelBuilder.Entity<FinanceDetail>(entity =>
        {
            entity.HasKey(e => e.IdFinance).HasName("finance_detail_pkey");

            entity.ToTable("finance_detail");

            entity.Property(e => e.IdFinance)
                .ValueGeneratedNever()
                .HasColumnName("id_finance");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("amount");
            entity.Property(e => e.FinanceName)
                .HasMaxLength(50)
                .HasColumnName("finance_name");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FinanceDetails)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_finance_user");
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(e => e.IdFriendship).HasName("friendships_pkey");

            entity.ToTable("friendships");

            entity.Property(e => e.IdFriendship)
                .ValueGeneratedNever()
                .HasColumnName("id_friendship");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdRef).HasColumnName("id_ref");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'PENDING'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdRefNavigation).WithMany(p => p.FriendshipIdRefNavigations)
                .HasForeignKey(d => d.IdRef)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_friend_ref");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.FriendshipIdUserNavigations)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_friend_user");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.IdImage).HasName("images_pkey");

            entity.ToTable("images");

            entity.Property(e => e.IdImage)
                .ValueGeneratedNever()
                .HasColumnName("id_image");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdRef).HasColumnName("id_ref");
            entity.Property(e => e.RefType)
                .HasMaxLength(50)
                .HasColumnName("ref_type");
            entity.Property(e => e.Url).HasColumnName("url");
        });

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

        modelBuilder.Entity<InvestmentAsset>(entity =>
        {
            entity.HasKey(e => e.IdAsset).HasName("investment_asset_pkey");

            entity.ToTable("investment_asset");

            entity.Property(e => e.IdAsset)
                .ValueGeneratedNever()
                .HasColumnName("id_asset");
            entity.Property(e => e.AssetName)
                .HasMaxLength(20)
                .HasColumnName("asset_name");
            entity.Property(e => e.AssetSymbol)
                .HasMaxLength(20)
                .HasColumnName("asset_symbol");
            entity.Property(e => e.Id)
                .HasMaxLength(50)
                .HasColumnName("id");
            entity.Property(e => e.IdFund).HasColumnName("id_fund");

            entity.HasOne(d => d.IdFundNavigation).WithMany(p => p.InvestmentAssets)
                .HasForeignKey(d => d.IdFund)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_asset_fund");
        });

        modelBuilder.Entity<InvestmentDetail>(entity =>
        {
            entity.HasKey(e => e.IdDetail).HasName("investment_detail_pkey");

            entity.ToTable("investment_detail");

            entity.Property(e => e.IdDetail)
                .ValueGeneratedNever()
                .HasColumnName("id_detail");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.Expense)
                .HasPrecision(30, 2)
                .HasColumnName("expense");
            entity.Property(e => e.Fee)
                .HasPrecision(15, 2)
                .HasColumnName("fee");
            entity.Property(e => e.IdAsset).HasColumnName("id_asset");
            entity.Property(e => e.Price)
                .HasPrecision(30, 2)
                .HasColumnName("price");
            entity.Property(e => e.Quantity)
                .HasPrecision(30, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .HasColumnName("type");

            entity.HasOne(d => d.IdAssetNavigation).WithMany(p => p.InvestmentDetails)
                .HasForeignKey(d => d.IdAsset)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_detail_asset");
        });

        modelBuilder.Entity<InvestmentFund>(entity =>
        {
            entity.HasKey(e => e.IdFund).HasName("investment_fund_pkey");

            entity.ToTable("investment_fund");

            entity.Property(e => e.IdFund)
                .ValueGeneratedNever()
                .HasColumnName("id_fund");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FundName)
                .HasMaxLength(100)
                .HasColumnName("fund_name");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.InvestmentFunds)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_fund_user");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.IdMessage).HasName("message_pkey");

            entity.ToTable("message");

            entity.Property(e => e.IdMessage)
                .ValueGeneratedNever()
                .HasColumnName("id_message");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.IdFriendship).HasColumnName("id_friendship");
            entity.Property(e => e.IsFriend).HasColumnName("is_friend");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.SendAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("send_at");

            entity.HasOne(d => d.IdFriendshipNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.IdFriendship)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_message_friendship");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.IdNotification).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.IdNotification)
                .ValueGeneratedNever()
                .HasColumnName("id_notification");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.IdRelated).HasColumnName("id_related");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IsRead)
                .HasDefaultValue(false)
                .HasColumnName("is_read");
            entity.Property(e => e.NotificationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("notification_date");
            entity.Property(e => e.NotificationType)
                .HasMaxLength(50)
                .HasColumnName("notification_type");
            entity.Property(e => e.RelatedType)
                .HasMaxLength(50)
                .HasColumnName("related_type");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_notification_user");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IdPayment).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.IdPayment)
                .ValueGeneratedNever()
                .HasColumnName("id_payment");
            entity.Property(e => e.Amount)
                .HasPrecision(12, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdAppTrans)
                .HasMaxLength(100)
                .HasColumnName("id_app_trans");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdZpTrans).HasColumnName("id_zp_trans");
            entity.Property(e => e.Method)
                .HasMaxLength(20)
                .HasColumnName("method");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_payment_user");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionName).HasName("permission_pkey");

            entity.ToTable("permission");

            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .HasColumnName("permission_name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.IdPost).HasName("posts_pkey");

            entity.ToTable("posts");

            entity.Property(e => e.IdPost)
                .ValueGeneratedNever()
                .HasColumnName("id_post");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("update_at");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_post_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleName).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<SavingDetail>(entity =>
        {
            entity.HasKey(e => e.IdDetail).HasName("saving_detail_pkey");

            entity.ToTable("saving_detail");

            entity.Property(e => e.IdDetail)
                .ValueGeneratedNever()
                .HasColumnName("id_detail");
            entity.Property(e => e.Amount)
                .HasPrecision(15, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IdSaving).HasColumnName("id_saving");

            entity.HasOne(d => d.IdSavingNavigation).WithMany(p => p.SavingDetails)
                .HasForeignKey(d => d.IdSaving)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_detail_saving");
        });

        modelBuilder.Entity<SavingGoal>(entity =>
        {
            entity.HasKey(e => e.IdSaving).HasName("saving_goals_pkey");

            entity.ToTable("saving_goals");

            entity.Property(e => e.IdSaving)
                .ValueGeneratedNever()
                .HasColumnName("id_saving");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.SavingName)
                .HasMaxLength(100)
                .HasColumnName("saving_name");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TargetAmount)
                .HasPrecision(18, 2)
                .HasColumnName("target_amount");
            entity.Property(e => e.TargetDate).HasColumnName("target_date");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.SavingGoals)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_saving_user");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.IdTransaction).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.IdTransaction)
                .ValueGeneratedNever()
                .HasColumnName("id_transaction");
            entity.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .HasColumnName("amount");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("create_at");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.TransactionCategory)
                .HasMaxLength(50)
                .HasColumnName("transaction_category");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transaction_date");
            entity.Property(e => e.TransactionName)
                .HasMaxLength(100)
                .HasColumnName("transaction_name");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(20)
                .HasColumnName("transaction_type");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_transaction_user");
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
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .HasColumnName("phone");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .HasColumnName("role_name");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("total_amount");

            entity.HasOne(d => d.RoleNameNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleName)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("user_role");

            entity.HasMany(d => d.PermissionNames).WithMany(p => p.IdUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "UserPermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasForeignKey("PermissionName")
                        .HasConstraintName("fk_user_permission_permission"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("IdUser")
                        .HasConstraintName("fk_user_permission_user"),
                    j =>
                    {
                        j.HasKey("IdUser", "PermissionName").HasName("user_permission_pkey");
                        j.ToTable("user_permission");
                        j.IndexerProperty<Guid>("IdUser").HasColumnName("id_user");
                        j.IndexerProperty<string>("PermissionName")
                            .HasMaxLength(50)
                            .HasColumnName("permission_name");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
