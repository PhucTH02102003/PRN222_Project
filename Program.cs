using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Hubs;
using PRN222_Project.Models;

namespace PRN222_Project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Cấu hình Authentication (sử dụng Cookie Authentication)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Login";  // Đường dẫn trang đăng nhập
                    options.LogoutPath = "/Logout";
                    options.AccessDeniedPath = "/AccessDenied";  // Đường dẫn khi người dùng không có quyền truy cập
                });

            // Đăng ký DbContext với DI container
            builder.Services.AddDbContext<CinemaManagementContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))); // Sử dụng connection string trong appsettings.json

            // Thêm dịch vụ Razor Pages
            builder.Services.AddRazorPages();
            // Thêm SignalR vào dịch vụ
            builder.Services.AddSignalR();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // Thêm Authentication và Authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            // Chuyển hướng đến trang Login khi ứng dụng được khởi chạy
            app.MapGet("/", context =>
            {
                context.Response.Redirect("/Login");  // Chuyển hướng đến trang Login
                return Task.CompletedTask;
            });
            app.MapHub<BookingHub>("/bookingHub");
            // Định nghĩa các trang Razor Pages
            app.MapRazorPages();

            app.Run();
        }
    }
}
