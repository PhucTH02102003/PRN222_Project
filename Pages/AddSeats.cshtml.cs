using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System.Linq;
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
            Screen = await _context.Screens
                .Include(s => s.Seats)  // Lấy danh sách ghế của screen
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);

            if (Screen == null)
            {
                return NotFound();
            }

            return Page();
        }

        // Thêm một ghế mới vào Screen
        public async Task<IActionResult> OnPostAsync(int screenId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Screen = await _context.Screens
                .Include(s => s.Seats)  // Lấy danh sách ghế của screen
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);

            if (Screen == null)
            {
                return NotFound();
            }

            // Kiểm tra nếu số ghế đã đạt hoặc vượt quá capacity
            if (Screen.Seats.Count >= Screen.Capacity)
            {
                // Nếu số ghế đã đầy, không cho phép thêm ghế mới và hiển thị thông báo lỗi
                TempData["ErrorMessage"] = "Cannot add more seats, screen capacity is full.";
                return RedirectToPage("./AddSeats", new { screenId = screenId });
            }

            // Gán ScreenId cho ghế mới
            Seat.ScreenId = screenId;

            // Đặt trạng thái của ghế là "Available"
            Seat.Status = "Available";

            // Thêm ghế vào cơ sở dữ liệu
            _context.Seats.Add(Seat);
            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageScreen");  // Sau khi thêm ghế, quay lại trang quản lý Screen
        }
    }
}
