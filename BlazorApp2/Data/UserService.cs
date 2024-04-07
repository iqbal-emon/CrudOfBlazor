using BlazorApp2.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJSRuntime _jsRuntime;
        public UserService(ApplicationDbContext applicationDbContext, IJSRuntime jsRuntime)
        {
            _context = applicationDbContext;
            _jsRuntime = jsRuntime;
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


        public async Task<string> GetUserRoleAsync()
        {
            // Retrieve the user ID from localStorage
            var userIdString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");

            // Parse the user ID string to an integer
            if (int.TryParse(userIdString, out int userId))
            {
                // User ID successfully parsed, now you can use it
                Console.WriteLine("User ID: " + userId);

                // Find the user in the database using the parsed user ID
                var user = await _context.Users.FindAsync(userId);

                // Return the user's role
                return user?.Role;
            }
            else
            {
                // Unable to parse the user ID string to an integer
                Console.WriteLine("Failed to parse user ID: " + userIdString);
                // Handle the error or provide a default value
                return "DefaultRole"; // Provide a default role or handle the error as needed
            }
        }

        public async Task<List<User>> GetUsersBySkillIdAsync(int skillId)
        {
            // This query assumes that your User entity's Skill collection
            // is properly configured to represent the many-to-many relationship between Users and Skills
            return await _context.Users
                                 .Where(u => u.Skill.Any(s => s.id == skillId))
                                 .ToListAsync();
        }

    }
}
