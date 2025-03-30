using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PRN222_Project.Pages
{
    public class EditProfileModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public EditProfileModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        // Thông báo lỗi và thành công
        public bool IsProfileUpdated { get; set; }
        public bool IsEmailValid { get; set; } = true;
        public bool IsPhoneValid { get; set; } = true;

        public async Task OnGetAsync()
        {
            // Lấy username từ Claims
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(usernameClaim))
            {
                RedirectToPage("/Login");
                return;
            }

            // Lấy thông tin người dùng từ database
            User = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameClaim);

            if (User == null)
            {
                RedirectToPage("/Login");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra email hợp lệ
            if (!IsValidEmail(User.Email))
            {
                IsEmailValid = false;
                return Page();
            }

            // Kiểm tra số điện thoại hợp lệ
            if (!IsValidPhoneNumber(User.PhoneNumber))
            {
                IsPhoneValid = false;
                return Page();
            }

            // Lấy thông tin người dùng từ Claims
            var usernameClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var userFromDb = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == usernameClaim);

            if (userFromDb == null)
            {
                return RedirectToPage("/Login");
            }

            // Cập nhật thông tin người dùng
            userFromDb.Email = User.Email;
            userFromDb.PhoneNumber = User.PhoneNumber;
            // Không thay đổi password, để giá trị password cũ
            // userFromDb.Password sẽ không thay đổi

            await _context.SaveChangesAsync();

            // Đánh dấu là thông tin đã được cập nhật thành công
            IsProfileUpdated = true;

            return Page();
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, emailRegex);
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var phoneRegex = @"^\d{10}$";
            return Regex.IsMatch(phoneNumber, phoneRegex);
        }
    }
}
