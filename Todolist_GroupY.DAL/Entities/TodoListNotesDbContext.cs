using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Todolist_GroupY.DAL.Entities;

public partial class TodoListNotesDbContext : DbContext
{
    public TodoListNotesDbContext()
    {
    }

    public TodoListNotesDbContext(DbContextOptions<TodoListNotesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Todo> Todos { get; set; }
    public virtual DbSet<User> Users { get; set; }

    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true)
            .Build();
        var strConn = config["ConnectionStrings:DefaultConnectionStringDB"];
        return strConn;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(e => e.TodoId).HasName("PK__Todos__958625523CE4F507");

            entity.HasIndex(e => e.DueDate, "IX_Todos_DueDate");
            entity.HasIndex(e => e.IsCompleted, "IX_Todos_IsCompleted");
            entity.HasIndex(e => e.ReminderTime, "IX_Todos_ReminderTime");
            entity.HasIndex(e => e.UserId, "IX_Todos_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.ReminderTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.User).WithMany(p => p.Todos)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Todos_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CD3A1804C");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534CAE91CD5").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}