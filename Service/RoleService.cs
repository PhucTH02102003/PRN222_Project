using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

public class RoleService
{
    private readonly CinemaManagementContext _context;

    public RoleService(CinemaManagementContext context)
    {
        _context = context;
    }

    public async Task<bool> UserHasRole(int userId, string roleName)
    {
        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Role)
            .ToListAsync();

        return userRoles.Any(ur => ur.Role.RoleName == roleName);
    }
}
