using BlazorApp2.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Mobile { get; set; }
        public string? Password { get; set; } // Consider storing hashed passwords only
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
        public virtual ICollection<Skill> Skill { get; set; } = new List<Skill>();
        public virtual ICollection<Employee> Employee { get; set; } = new List<Employee>();



    }
}
