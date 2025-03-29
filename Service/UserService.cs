using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using PRN222_Project.Models;

public class UserService
{
    private readonly CinemaManagementContext _context;

    public UserService(CinemaManagementContext context)
    {
        _context = context;
    }

    public async Task RegisterUser(string username, string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Username = username,
            Password = hashedPassword,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateUser(string username, string password)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

        if (user == null)
        {
            return false; 
        }   

        return BCrypt.Net.BCrypt.Verify(password, user.Password); 
    }

}
