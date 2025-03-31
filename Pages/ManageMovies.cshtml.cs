using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

namespace PRN222_Project.Pages
{
    public class ManageMoviesModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ManageMoviesModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public IList<Movie> Movies { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy tất cả các bộ phim cùng với thông tin showtimes, screens, cinemas
            Movies = await _context.Movies
                .Include(m => m.Showtimes)
                    .ThenInclude(s => s.Screen)
                        .ThenInclude(sc => sc.Cinema) // Lấy thông tin rạp và màn chiếu
                .ToListAsync();

            return Page();
        }
    }
}
