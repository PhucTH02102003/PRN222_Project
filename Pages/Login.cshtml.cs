using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace PRN222_Project.Pages
{
    public class LoginModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public LoginModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            Debug.WriteLine("Username: " + Username);  // Kiểm tra xem có nhận đúng giá trị không

            // Tìm người dùng trong cơ sở dữ liệu
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                // Lấy vai trò của người dùng từ bảng UserRoles
                var userRoles = await _context.UserRoles
                    .Where(ur => ur.UserId == user.UserId)
                    .Include(ur => ur.Role)  // Kết nối với bảng Role để lấy thông tin vai trò
                    .ToListAsync();

                // Tạo các claims (bao gồm vai trò)
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    // Lấy các vai trò và thêm vào claims
                    new Claim(ClaimTypes.Role, string.Join(",", userRoles.Select(ur => ur.Role.RoleName)))
                };

                var identity = new ClaimsIdentity(claims, "Login");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Kiểm tra vai trò và chuyển hướng đến trang tương ứng
                if (userRoles.Any(ur => ur.Role!.RoleName == "Admin"))
                {
                    // Nếu là Admin, chuyển hướng đến trang Admin
                    return RedirectToPage("/AdminPage");
                }
                else if (userRoles.Any(ur => ur.Role!.RoleName == "Customer"))
                {
                    // Nếu là Customer, chuyển hướng đến trang Customer
                    return RedirectToPage("/CustomerPage");
                }
            }

            // Nếu đăng nhập thất bại, giữ lại trang đăng nhập
            return Page();
        }
    }
}
