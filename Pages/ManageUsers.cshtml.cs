using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Admin")]  // Chỉ cho phép Admin truy cập
    public class ManageUsersModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ManageUsersModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; } = new List<User>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy danh sách người dùng có role là Customer
            Users = await _context.Users
                .Where(u => u.UserRoles.Any(ur => ur.Role!.RoleName == "Customer"))
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostLockAsync(int userId)
        {
            // Lấy người dùng từ cơ sở dữ liệu
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Kiểm tra trạng thái hiện tại và thay đổi
            user.IsLocked = !user.IsLocked;  // Nếu đã khóa thì mở khóa, nếu chưa khóa thì khóa tài khoản
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToPage(); // Quay lại trang quản lý người dùng
        }




    }
}
