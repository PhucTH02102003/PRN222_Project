using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System.Linq;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminPageModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AdminPageModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public int TotalCustomers { get; set; }
        public int TotalMovies { get; set; }
        public int TotalCinemas { get; set; }
        public int TotalSeats { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy tổng số lượng Customer
            TotalCustomers = await _context.Users.CountAsync();

            // Lấy tổng số lượng Movies
            TotalMovies = await _context.Movies.CountAsync();

            // Lấy tổng số lượng Cinemas
            TotalCinemas = await _context.Cinemas.CountAsync();

            // Lấy tổng số ghế
            TotalSeats = await _context.Seats.CountAsync();

            return Page();
        }
    }
}
