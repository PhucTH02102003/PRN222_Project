using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PRN222_Project.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PRN222_Project.Pages
{
    [Authorize(Roles = "Customer")]
    public class CustomerPageModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public CustomerPageModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public List<Movie> Movies { get; set; }
        public List<Cinema> Cinemas { get; set; }

        [BindProperty]
        public int MovieId { get; set; }
        [BindProperty]
        public int CinemaId { get; set; }

        public async Task OnGetAsync()
        {
            // Lấy danh sách các bộ phim và rạp chiếu
            Movies = await _context.Movies.ToListAsync();
            Cinemas = await _context.Cinemas.ToListAsync();
        }
    }
}
