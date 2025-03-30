using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;  // Thêm namespace này
using System.Threading.Tasks;
using BCrypt.Net;  // Đảm bảo bạn sử dụng BCrypt để mã hóa mật khẩu

namespace PRN222_Project.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ChangePasswordModel(CinemaManagementContext context)
        {
            _context = context;
        }

        // Các thuộc tính liên quan đến thay đổi mật khẩu
        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        [BindProperty]
        public string ConfirmNewPassword { get; set; }

        // Thông báo thành công
        public bool IsPasswordChanged { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra nếu người dùng đã đăng nhập
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Login"); // Chuyển hướng về trang đăng nhập nếu chưa đăng nhập
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy thông tin người dùng từ Claims
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usernameClaim))
            {
                return RedirectToPage("/Login");
            }

            // Tìm người dùng trong database
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameClaim);

            if (user == null)
            {
                return RedirectToPage("/Login");
            }

            // Kiểm tra mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(CurrentPassword, user.Password))
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return Page(); // Nếu mật khẩu cũ không đúng, giữ lại trang và thông báo lỗi
            }

            // Kiểm tra mật khẩu mới với xác nhận mật khẩu mới
            if (NewPassword != ConfirmNewPassword)
            {
                ModelState.AddModelError(string.Empty, "New password and confirmation do not match.");
                return Page(); // Nếu mật khẩu mới và xác nhận không khớp, giữ lại trang và thông báo lỗi
            }

            // Mã hóa mật khẩu mới trước khi lưu vào database
            var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            user.Password = hashedNewPassword; // Cập nhật mật khẩu mới vào cơ sở dữ liệu

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Đánh dấu là mật khẩu đã được thay đổi thành công
            IsPasswordChanged = true;

            return Page(); // Giữ lại trên trang ChangePassword với thông báo thành công
        }
    }
}
