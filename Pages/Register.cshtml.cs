using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Project.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public RegisterModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra nếu mật khẩu và xác nhận mật khẩu không khớp
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("Password", "Password and Confirm Password must match.");
                return Page();
            }

            // Kiểm tra xem người dùng đã tồn tại trong cơ sở dữ liệu chưa
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return Page();
            }

            // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            // Tạo đối tượng người dùng và lưu vào cơ sở dữ liệu
            var user = new User
            {
                Username = Username,
                Password = hashedPassword,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Sau khi đăng ký thành công, chuyển hướng người dùng về trang đăng nhập
            return RedirectToPage("/Login");  // Chuyển hướng về trang đăng nhập sau khi đăng ký thành công
        }
    }
}
