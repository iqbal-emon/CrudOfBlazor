using BlazorApp2.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class EmailService
    {
        private readonly ApplicationDbContext _context;

        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Email>> GetUserEmails(int userId)
        {
            return await _context.Emails.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task AddEmail(int userId, string emailAddress)
        {
            var newEmail = new Email { UserId = userId, EmailAddress = emailAddress };
            _context.Emails.Add(newEmail);
            await _context.SaveChangesAsync();
        }

        public async Task MakePrimary(int emailId)
        {
            var email = await _context.Emails.FindAsync(emailId);
            if (email != null)
            {
                email.IsPrimary = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEmail(int emailId)
        {
            var email = await _context.Emails.FindAsync(emailId);
            if (email != null)
            {
                _context.Emails.Remove(email);
                await _context.SaveChangesAsync();
            }
        }
    }
}
