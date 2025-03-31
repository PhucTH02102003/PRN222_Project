using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class BookingHistoryModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public BookingHistoryModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public List<Booking> Bookings { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy UserId từ User đang đăng nhập
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null)
            {
                UserId = user.UserId;

                // Lấy thông tin lịch sử đặt vé của người dùng từ database
                Bookings = await _context.Bookings
                    .Include(b => b.Showtime)
                    .Include(b => b.Showtime.Movie)
                    .Include(b => b.Showtime.Screen.Cinema)
                    .Include(b => b.SeatBookings)  // Lấy các SeatBooking
                    .ThenInclude(sb => sb.Seat)  // Lấy thông tin ghế từ SeatBooking
                    .Where(b => b.UserId == UserId)  // Lọc theo UserId và trạng thái successful
                    .ToListAsync();
            }
            else
            {
                // Nếu không tìm thấy người dùng, có thể là người dùng chưa đăng nhập
                Bookings = new List<Booking>();
            }
        }
    }
}
