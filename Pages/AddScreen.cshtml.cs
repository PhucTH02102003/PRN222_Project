using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class AddScreenModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AddScreenModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Screen Screen { get; set; }

        public Cinema Cinema { get; set; }

        // Lấy CinemaId từ query string
        public async Task<IActionResult> OnGetAsync(int cinemaId)
        {
            Cinema = await _context.Cinemas.FindAsync(cinemaId);
            if (Cinema == null)
            {
                return NotFound();
            }
            return Page();
        }

        // Thêm một Screen mới vào Cinema
        public async Task<IActionResult> OnPostAsync(int cinemaId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var cinema = await _context.Cinemas.FindAsync(cinemaId);
            if (cinema == null)
            {
                return NotFound();
            }

            Screen.CinemaId = cinemaId;
            _context.Screens.Add(Screen);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageCinema");  // Quay lại trang quản lý cinema
        }
    }
}
