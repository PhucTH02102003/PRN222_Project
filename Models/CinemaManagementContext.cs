using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Project.Models;

public partial class CinemaManagementContext : DbContext
{
    public CinemaManagementContext()
    {
    }

    public CinemaManagementContext(DbContextOptions<CinemaManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Screen> Screens { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<SeatBooking> SeatBookings { get; set; }

    public virtual DbSet<Showtime> Showtimes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-3LVVNGB5\\SQLEXPRESS;uid=sa;pwd=123;database=CinemaManagement;Encrypt=false; Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__Bookings__5DE3A5B104A0AF5C");

            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.BookingTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booking_time");
            entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_amount");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Showtime).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ShowtimeId)
                .HasConstraintName("FK__Bookings__showti__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Bookings__user_i__5CD6CB2B");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.CinemaId).HasName("PK__Cinemas__566287786EF40241");

            entity.Property(e => e.CinemaId).HasColumnName("cinema_id");
            entity.Property(e => e.CinemaName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cinema_name");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("location");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.MovieId).HasName("PK__Movies__83CDF74950A742D9");

            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Genre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("genre");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__E5331AFA498B9BE7");

            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("permission_name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__760965CCC19D116C");

            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__Role_Per__B1E85A10FC281B57");

            entity.ToTable("Role_Permissions");

            entity.Property(e => e.RolePermissionId).HasColumnName("role_permission_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK__Role_Perm__permi__6D0D32F4");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Role_Perm__role___6C190EBB");
        });

        modelBuilder.Entity<Screen>(entity =>
        {
            entity.HasKey(e => e.ScreenId).HasName("PK__Screens__CC19B67AA56AE8F1");

            entity.Property(e => e.ScreenId).HasColumnName("screen_id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.CinemaId).HasColumnName("cinema_id");
            entity.Property(e => e.ScreenName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("screen_name");

            entity.HasOne(d => d.Cinema).WithMany(p => p.Screens)
                .HasForeignKey(d => d.CinemaId)
                .HasConstraintName("FK__Screens__cinema___5070F446");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.SeatId).HasName("PK__Seats__906DED9CBBDF3E77");

            entity.Property(e => e.SeatId).HasColumnName("seat_id");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");
            entity.Property(e => e.SeatNumber)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("seat_number");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("available")
                .HasColumnName("status");

            entity.HasOne(d => d.Screen).WithMany(p => p.Seats)
                .HasForeignKey(d => d.ScreenId)
                .HasConstraintName("FK__Seats__screen_id__5812160E");
        });

        modelBuilder.Entity<SeatBooking>(entity =>
        {
            entity.HasKey(e => e.SeatBookingId).HasName("PK__Seat_Boo__714E8C0FD3EE721B");

            entity.ToTable("Seat_Booking");

            entity.Property(e => e.SeatBookingId).HasColumnName("seat_booking_id");
            entity.Property(e => e.BookingId).HasColumnName("booking_id");
            entity.Property(e => e.SeatId).HasColumnName("seat_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.SeatBookings)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("FK__Seat_Book__booki__60A75C0F");

            entity.HasOne(d => d.Seat).WithMany(p => p.SeatBookings)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK__Seat_Book__seat___619B8048");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.HasKey(e => e.ShowtimeId).HasName("PK__Showtime__A406B51842021F1A");

            entity.Property(e => e.ShowtimeId).HasColumnName("showtime_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.MovieId).HasColumnName("movie_id");
            entity.Property(e => e.ScreenId).HasColumnName("screen_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");

            entity.HasOne(d => d.Movie).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK__Showtimes__movie__534D60F1");

            entity.HasOne(d => d.Screen).WithMany(p => p.Showtimes)
                .HasForeignKey(d => d.ScreenId)
                .HasConstraintName("FK__Showtimes__scree__5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__B9BE370FFC343299");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__User_Rol__B8D9ABA21F53AFAB");

            entity.ToTable("User_Roles");

            entity.Property(e => e.UserRoleId).HasColumnName("user_role_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__User_Role__role___693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__User_Role__user___68487DD7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
