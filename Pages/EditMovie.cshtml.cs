using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System.Threading.Tasks;
using System.Linq;

namespace PRN222_Project.Pages
{
    public class EditMovieModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public EditMovieModel(CinemaManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public List<Cinema> Cinemas { get; set; }
        public List<Screen> Screens { get; set; }
        public Showtime Showtime { get; set; }

        public async Task<IActionResult> OnGetAsync(int movieId)
        {
            // Lấy thông tin phim và các showtime liên quan
            Movie = await _context.Movies
                .Include(m => m.Showtimes)
                    .ThenInclude(s => s.Screen)
                    .ThenInclude(sc => sc.Cinema)
                .FirstOrDefaultAsync(m => m.MovieId == movieId);

            if (Movie == null)
            {
                return NotFound();
            }

            // Lấy danh sách Cinemas và Screens cho dropdown list
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

            // Lấy lại Movie từ cơ sở dữ liệu và bao gồm Showtimes liên quan.
            var movieFromDb = await _context.Movies
                .Include(m => m.Showtimes)  // Lấy thông tin showtimes
                .ThenInclude(s => s.Screen)
                .FirstOrDefaultAsync(m => m.MovieId == Movie.MovieId);

            if (movieFromDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin phim
            movieFromDb.Title = Movie.Title;
            movieFromDb.Description = Movie.Description;
            movieFromDb.Duration = Movie.Duration;
            movieFromDb.Genre = Movie.Genre;
            movieFromDb.ReleaseDate = Movie.ReleaseDate;

            // Kiểm tra và cập nhật thông tin showtime cho mỗi showtime của phim
            foreach (var showtime in movieFromDb.Showtimes)
            {
                var startTime = Request.Form["ShowtimeStart_" + showtime.ShowtimeId].ToString();
                var endTime = Request.Form["ShowtimeEnd_" + showtime.ShowtimeId].ToString();

                // Kiểm tra StartTime và EndTime
                if (DateTime.Parse(startTime) > DateTime.Parse(endTime))
                {
                    TempData["ErrorMessage"] = "Start Time cannot be later than End Time."; // Lưu thông báo lỗi vào TempData
                    return RedirectToPage(); // Chuyển hướng lại để hiển thị lỗi
                }

                // Chỉ cập nhật showtime nếu thời gian hợp lệ
                showtime.StartTime = DateTime.Parse(startTime);
                showtime.EndTime = DateTime.Parse(endTime);

                // Không cần cập nhật Cinema và Screen nếu không thay đổi chúng
                _context.Attach(showtime).State = EntityState.Modified;
            }

            _context.Attach(movieFromDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();  // Lưu các thay đổi vào cơ sở dữ liệu

            return RedirectToPage("./ManageMovies");  // Chuyển hướng sau khi cập nhật
        }


    }
}
