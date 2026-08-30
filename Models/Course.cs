using System.ComponentModel.DataAnnotations;
namespace FinalAssignmentBrief.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, 100)]
        public int DurationHours { get; set; }

        [Range(0, 10000)]
        public decimal Fees { get; set; }

        [Required]
        [RegularExpression("Beginner|Intermediate|Advanced", ErrorMessage = "Invalid level.")]
        public string Level { get; set; } = string.Empty;

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
