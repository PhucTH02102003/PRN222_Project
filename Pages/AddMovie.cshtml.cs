using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class AddMovieModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public AddMovieModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public List<Cinema> Cinemas { get; set; }
        public List<Screen> Screens { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Movie = new Movie();
            Cinemas = await _context.Cinemas.ToListAsync();
            Screens = await _context.Screens.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Movie.Showtimes == null)
            {
                Movie.Showtimes = new List<Showtime>();
            }
            // Thêm Movie vào cơ sở dữ liệu
            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            var startTimes = Request.Form["ShowtimeStart[]"];
            var endTimes = Request.Form["ShowtimeEnd[]"];

            for (int i = 0; i < startTimes.Count; i++)
            {
                var startTime = DateTime.Parse(startTimes[i]);
                var endTime = DateTime.Parse(endTimes[i]);

                var showtime = new Showtime
                {
                    MovieId = Movie.MovieId,
                    StartTime = startTime,
                    EndTime = endTime
                };

                _context.Showtimes.Add(showtime);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./ManageShowtime", new { movieId = Movie.MovieId });
        }
    }
}
