using Azure.Core;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using PRN222_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Admin")]
    public class ManageShowtimeModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ManageShowtimeModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public Movie Movie { get; set; }
        public List<Screen> Screens { get; set; }
        public List<Cinema> Cinemas { get; set; }

        public async Task<IActionResult> OnGetAsync(int movieId)
        {
            Movie = await _context.Movies
                .Include(m => m.Showtimes)
                .ThenInclude(s => s.Screen)
                .ThenInclude(sc => sc.Cinema)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (Movie == null)
            {
                return NotFound();
            }

            Screens = await _context.Screens.ToListAsync();
            Cinemas = await _context.Cinemas.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int movieId)
        {
            Movie = await _context.Movies
                .Include(m => m.Showtimes)
                .ThenInclude(s => s.Screen)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (Movie == null)
            {
                return NotFound();
            }

            foreach (var showtime in Movie.Showtimes)
            {
                var screenId = Request.Form["ShowtimeScreen_" + showtime.ShowtimeId];
                if (!string.IsNullOrEmpty(screenId))
                {
                    showtime.ScreenId = int.Parse(screenId);
                }

                var startTime = Request.Form["ShowtimeStart_" + showtime.ShowtimeId].ToString();
                var endTime = Request.Form["ShowtimeEnd_" + showtime.ShowtimeId].ToString();

                showtime.StartTime = DateTime.Parse(startTime);
                showtime.EndTime = DateTime.Parse(endTime);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./ManageMovies", new { movieId = movieId });
        }
    }

}