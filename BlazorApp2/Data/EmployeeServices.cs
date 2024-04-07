using BlazorApp2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> RegisterUser(string fullName, string email, string password, string mobile, string role)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                FullName = fullName,
                Password = hashedPassword,
                IsActive = true,
                Mobile = mobile,
                Role = role
            };

            var userEmail = new Email
            {
                EmailAddress = email,
                IsPrimary = true
            };

            user.Emails = new List<Email> { userEmail };



            var userEmployee = new Employee
            {
                BasicSalary = 1000
                
            };
            user.Employee=new List<Employee> { userEmployee };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Save the user to get the generated Id

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
        public async Task<List<User>> GetEmployeesAsync()
        {
            return await _context.Users.Include(u => u.Emails).ToListAsync();
        }


        public async Task<string> GetPrimaryEmailAsync(User user)
        {
            var primaryEmail = await _context.Emails
                                              .Where(e => e.UserId == user.Id && e.IsPrimary)
                                              .Select(e => e.EmailAddress)
                                              .FirstOrDefaultAsync();
            return primaryEmail;
        }



        public async Task UpdateEmployeeAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task<List<Skill>> GetSkillsByUserIdAsync(int userId)
        {
            var userSkills = await _context.Skills
                                            .Where(s => s.UserId == userId)
                                            .ToListAsync();
            return userSkills;
        }
    }
}
