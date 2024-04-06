using BlazorApp2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp2.Data
{
    public class EmployeeServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IJSRuntime _jsRuntime;

        public EmployeeServices(ApplicationDbContext applicationDbContext, IJSRuntime jsRuntime)
        {
            _context = applicationDbContext;
            _jsRuntime = jsRuntime;
        }

        public async Task<IActionResult> RegisterUser(string fullName, string email, string password, string Mobile, string role)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                FullName = fullName,
                Password = hashedPassword,
                IsActive = true,
                Mobile = Mobile,
                Role = role
            };

            var userEmail = new Email
            {
                EmailAddress = email,
                IsPrimary = true
            };

            user.Emails = new List<Email> { userEmail };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new OkObjectResult("User registered successfully.");
        }

        public async Task SetUserIdInLocalStorage(int userId)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "userId", userId);
        }

        public async Task<User> GetUserByEmailOrMobile(string emailOrMobile)
        {
            return await _context.Users.FirstOrDefaultAsync(u =>
                (u.Emails.Any(e => e.EmailAddress == emailOrMobile && e.IsPrimary) && u.IsActive) ||
                (u.Mobile == emailOrMobile && u.IsActive) || (u.FullName == emailOrMobile && u.IsActive));
        }

        public async Task<bool> ValidateUser(User user, string password)
        {
            if (user == null || !user.IsActive)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
    }
}
