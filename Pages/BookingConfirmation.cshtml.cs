using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Project.Pages
{
    public class BookingConfirmationModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public BookingConfirmationModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public Booking Booking { get; set; }
        public Movie Movie { get; set; }
        public Cinema Cinema { get; set; }
        public Showtime Showtime { get; set; }
        public List<Seat> Seats { get; set; }
        public string Username { get; set; }

        [BindProperty(SupportsGet = true)]
        public int BookingId { get; set; }

        

        public async Task OnGetAsync()
        {
            // Lấy thông tin booking từ database dựa trên bookingId
            Booking = await _context.Bookings
                .Include(b => b.Showtime)
                .Include(b => b.Showtime.Movie)
                .Include(b => b.Showtime.Screen.Cinema)
                .Include(b => b.SeatBookings)  // Lấy các SeatBooking
                .ThenInclude(sb => sb.Seat)  // Lấy thông tin ghế từ SeatBooking
                .FirstOrDefaultAsync(b => b.BookingId == BookingId);

            if (Booking != null)
            {
                Movie = Booking.Showtime.Movie;
                Cinema = Booking.Showtime.Screen.Cinema;
                Showtime = Booking.Showtime;
                Seats = Booking.SeatBookings.Select(sb => sb.Seat).ToList();
            }
            Username = User.Identity.Name!;
        }
    }
}