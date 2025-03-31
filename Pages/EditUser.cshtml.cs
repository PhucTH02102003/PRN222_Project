using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Admin")]
    public class EditUserModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public EditUserModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }  // Tạo đối tượng để lưu thông tin người dùng

        public async Task<IActionResult> OnGetAsync(int userId)
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu
            User = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Nếu không tìm thấy người dùng
            if (User == null)
            {
                return NotFound();
            }

            if (User.PhoneNumber.Length != 10 || !User.PhoneNumber.All(char.IsDigit))
            {
                ModelState.AddModelError("User.PhoneNumber", "Phone number must be 10 digits.");
                return Page(); // Nếu không hợp lệ, quay lại trang chỉnh sửa
            }

            // Nếu mật khẩu không thay đổi, giữ nguyên mật khẩu hiện tại
            if (string.IsNullOrEmpty(User.Password))
            {
                // Không thay đổi mật khẩu, chỉ cập nhật các trường khác
                _context.Attach(User).State = EntityState.Modified;
            } 
            else
            {
                // Nếu mật khẩu được thay đổi, mã hóa lại mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(User.Password);
                User.Password = hashedPassword; // Cập nhật mật khẩu mới
                _context.Attach(User).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/ManageUsers"); // Quay lại trang quản lý người dùng
        }

    }
}
