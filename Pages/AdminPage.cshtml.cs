using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Admin")]  // Chỉ cho phép người dùng có vai trò Admin truy cập
    public class AdminPageModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AdminPageModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Kiểm tra xem người dùng đã đăng nhập và có quyền Admin hay không
            var username = User.Identity.Name;
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return Unauthorized();  // Nếu người dùng không hợp lệ
            }

            // Kiểm tra vai trò của người dùng
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == user.UserId)
                .Include(ur => ur.Role)
                .ToListAsync();

            if (!userRoles.Any(ur => ur.Role.RoleName == "Admin"))
            {
                return Forbid();  // Nếu người dùng không có vai trò Admin
            }

            return Page();  // Nếu người dùng có vai trò Admin, cho phép truy cập trang
        }
    }
}
