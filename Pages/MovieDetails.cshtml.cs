using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Customer")]
    public class MovieDetailsModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public MovieDetailsModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }
        public List<Showtime> Showtimes { get; set; }
        public List<Seat> Seats { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CinemaId { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy thông tin Movie và Cinema từ các tham số URL
            Movie = await _context.Movies.FindAsync(MovieId);
            Cinema = await _context.Cinemas.FindAsync(CinemaId);

            // Lấy danh sách Showtimes cho bộ phim và rạp đã chọn
            Showtimes = await _context.Showtimes
                .Where(s => s.MovieId == MovieId && s.Screen.CinemaId == CinemaId)
                .ToListAsync();

            // Lấy danh sách ghế có sẵn cho rạp và phim đã chọn
            Seats = await _context.Seats
                .Where(s => s.Screen.CinemaId == CinemaId)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy các thông tin cần thiết từ cơ sở dữ liệu
            var showtime = await _context.Showtimes.FindAsync(MovieId);
            var seat = await _context.Seats.FindAsync(CinemaId);

            var booking = new Booking
            {
                UserId = 5,  // Giả sử người dùng có ID 5
                ShowtimeId = showtime.ShowtimeId,
                BookingTime = DateTime.Now,
                Status = "Pending",
                TotalAmount = 15.00m // Số tiền giả sử cho mỗi vé
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            var seatBooking = new SeatBooking
            {
                BookingId = booking.BookingId,
                SeatId = seat.SeatId
            };

            _context.SeatBookings.Add(seatBooking);
            await _context.SaveChangesAsync();

            // Chuyển hướng đến trang xác nhận hoặc trang quản lý vé
            return RedirectToPage("/BookingConfirmation");
        }
    }
}
