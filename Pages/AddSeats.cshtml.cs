using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class AddSeatsModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AddSeatsModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Seat Seat { get; set; }

        public Screen Screen { get; set; }

        // Lấy thông tin screen qua ScreenId
        public async Task<IActionResult> OnGetAsync(int screenId)
        {
            Screen = await _context.Screens.FindAsync(screenId);
            if (Screen == null)
            {
                return NotFound();
            }

            return Page();
        }

        // Thêm một ghế cho Screen
        public async Task<IActionResult> OnPostAsync(int screenId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Seat.ScreenId = screenId;
            Seat.Status = "Available";
            _context.Seats.Add(Seat);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageScreen");  // Sau khi thêm ghế, quay lại trang quản lý screen
        }
    }
}
