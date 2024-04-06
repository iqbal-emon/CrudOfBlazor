using BlazorApp2.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetCurrentUser()
        {
            // Implement the logic to get the current user based on your authentication mechanism
            // For example, you might retrieve the user based on the authenticated user's ID
            // This method is just a placeholder and should be replaced with your actual logic
            return await _context.Users.FirstOrDefaultAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
