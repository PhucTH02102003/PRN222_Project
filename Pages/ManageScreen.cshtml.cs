using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222_Project.Pages
{
    public class ManageScreenModel : PageModel
    {
        private readonly CinemaManagementContext _context;

        public ManageScreenModel(CinemaManagementContext context)
        {
            _context = context;
        }

        public List<Screen> Screens { get; set; }

        // Lấy thông tin tất cả các screen
        public async Task<IActionResult> OnGetAsync()
        {
            Screens = await _context.Screens
                .Include(s => s.Cinema)  // Lấy thông tin Cinema liên kết với Screen
                .Include(s => s.Seats)   // Lấy thông tin Seats của Screen
                .ToListAsync();

            return Page();
        }
    }
}
