using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PRN222_Project.Hubs;
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

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty]
        public int SeatId { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy thông tin Movie và Cinema từ các tham số URL
            Movie = await _context.Movies.FindAsync(MovieId);
            Cinema = await _context.Cinemas.FindAsync(CinemaId);

            // Lấy danh sách Showtimes cho bộ phim và rạp đã chọn
            Showtimes = await _context.Showtimes
            .Where(s => s.MovieId == MovieId && s.Screen.CinemaId == CinemaId)
            .ToListAsync();

            // Kiểm tra nếu có showtime được chọn
            if (ShowtimeId > 0)
            {
                // Lấy danh sách ghế cho showtime và màn hình tương ứng
                var selectedShowtime = Showtimes.FirstOrDefault(s => s.ShowtimeId == ShowtimeId);
                if (selectedShowtime != null)
                {
                    Seats = await _context.Seats
                        .Where(s => s.ScreenId == selectedShowtime.ScreenId)
                        .ToListAsync();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Lấy thông tin người dùng từ Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return RedirectToPage("/Login");  // Nếu không có người dùng đăng nhập, chuyển hướng đến trang đăng nhập
            }

            // Kiểm tra nếu SeatId hoặc ShowtimeId không hợp lệ
            if (ShowtimeId == 0 || SeatId == 0)
            {
                return BadRequest("Invalid Showtime or Seat ID.");
            }

            var showtime = await _context.Showtimes.FindAsync(ShowtimeId);
            var seat = await _context.Seats.FindAsync(SeatId);

            if (showtime == null || seat == null || seat.Status == "Booked")
            {
                return NotFound();
            }

            var booking = new Booking
            {
                ShowtimeId = showtime.ShowtimeId,
                BookingTime = DateTime.Now,
                Status = "Pending",
                TotalAmount = 15.00m,
                UserId = int.Parse(userId)
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            seat.Status = "Booked"; // Đặt ghế thành Booked
            await _context.SaveChangesAsync();

            var seatBooking = new SeatBooking
            {
                BookingId = booking.BookingId,
                SeatId = seat.SeatId
            };

            _context.SeatBookings.Add(seatBooking);
            await _context.SaveChangesAsync();

            // Gửi thông báo qua SignalR khi ghế được đặt
            var hubContext = _context.GetService<IHubContext<BookingHub>>();
            await hubContext.Clients.All.SendAsync("ReceiveSeatUpdate", seat.SeatId);

            return RedirectToPage("/BookingConfirmation", new { bookingId = booking.BookingId });
        }

    }

}