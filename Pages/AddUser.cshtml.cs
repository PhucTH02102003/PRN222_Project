using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using BCrypt.Net;

namespace PRN222_Project.Pages
{
    public class AddUserModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AddUserModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public string Role { get; set; } // Để nhận giá trị role từ dropdown

        public void OnGet()
        {
            // Không cần làm gì trong OnGet, chỉ hiển thị form AddUser
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra xem mật khẩu có được nhập hay không
            if (string.IsNullOrEmpty(User.Password))
            {
                ModelState.AddModelError("User.Password", "Password is required.");
                return Page();
            }

            // Mã hóa mật khẩu người dùng trước khi lưu vào cơ sở dữ liệu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(User.Password);
            User.Password = hashedPassword; // Lưu mật khẩu đã mã hóa

            // Thiết lập thời gian tạo cho người dùng
            User.CreatedAt = DateTime.Now;

            // Thêm người dùng mới vào cơ sở dữ liệu
            _context.Users.Add(User);
            await _context.SaveChangesAsync();

            // Thêm vai trò cho người dùng
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == Role);
            if (role != null)
            {
                var userRole = new UserRole
                {
                    UserId = User.UserId,
                    RoleId = role.RoleId
                };

                _context.UserRoles.Add(userRole);  // Gán vai trò cho người dùng
                await _context.SaveChangesAsync();
            }

            // Quay lại trang quản lý người dùng sau khi thêm thành công
            return RedirectToPage("/ManageUsers");
        }
    }
}
