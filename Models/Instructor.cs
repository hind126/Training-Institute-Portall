using System.ComponentModel.DataAnnotations;

namespace FinalAssignmentBrief.Models
{
    public class Instructor
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
