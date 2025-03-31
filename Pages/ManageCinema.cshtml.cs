using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class ManageCinemaModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ManageCinemaModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public List<Cinema> Cinemas { get; set; }

        // Hiển thị danh sách các cinema và các screen tương ứng
        public async Task<IActionResult> OnGetAsync()
        {
            Cinemas = await _context.Cinemas
                .Include(c => c.Screens)  // Lấy tất cả screens của cinema
                .ToListAsync();

            return Page();
        }
    }
}
