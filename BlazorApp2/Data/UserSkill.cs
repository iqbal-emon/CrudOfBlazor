using System.ComponentModel.DataAnnotations;

namespace BlazorApp2
{
    public class UserSkill
    {
        [Key]
        public int UserId { get; set; }
        public int SkillId { get; set; }
        public virtual User User { get; set; }
        public virtual Skill Skill { get; set; }
    }

}
