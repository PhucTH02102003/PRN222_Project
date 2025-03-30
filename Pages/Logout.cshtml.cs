using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace PRN222_Project.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            // Thực hiện đăng xuất người dùng
            await HttpContext.SignOutAsync();

            // Chuyển hướng về trang Login sau khi đăng xuất
            return RedirectToPage("/Login");
        }
    }
}
