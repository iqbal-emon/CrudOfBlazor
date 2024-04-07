using BlazorApp2.Data;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp2
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EducationalQualification { get; set; }
        public decimal? BasicSalary { get; set; }
        public DateTime?  JoiningDate { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    }
}
