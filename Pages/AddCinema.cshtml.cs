using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class AddCinemaModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AddCinemaModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cinema Cinema { get; set; }

        // Thêm một cinema mới vào cơ sở dữ liệu
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Thêm cinema vào cơ sở dữ liệu
            _context.Cinemas.Add(Cinema);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageCinema");  // Quay lại trang quản lý cinema
        }
    }
}
