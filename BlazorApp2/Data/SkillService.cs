using BlazorApp2.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class SkillService
    {
        private readonly ApplicationDbContext _context;
        private readonly IJSRuntime _jsRuntime;

        public SkillService(ApplicationDbContext applicationDbContext, IJSRuntime jsRuntime)
        {
            _context = applicationDbContext;
            _jsRuntime = jsRuntime;
        }

        public async Task<List<Skill>> GetSkillsAsync()
        {
            int userId = await GetUserIdFromLocalStorage();
            return await _context.Skills.Where(s => s.UserId == userId).ToListAsync();
        }

        public async Task AddSkillAsync(Skill skill)
        {
            int userId = await GetUserIdFromLocalStorage();
            skill.UserId = userId;
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSkillAsync(Skill skill)
        {
            int userId = await GetUserIdFromLocalStorage();
            if (skill.UserId == userId)
            {
                _context.Skills.Remove(skill);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<int> GetUserIdFromLocalStorage()
        {
            string userIdString = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "userId");
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            // Return a default value or handle error appropriately
            return 0;
        }

    }

}
