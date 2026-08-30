using System.ComponentModel.DataAnnotations;

namespace FinalAssignmentBrief.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public int? Mark { get; set;  }
        public DateTime EnrollmentDate { get; set; }

        [Range(0.0, 100.0, ErrorMessage = "Grade must be between 0.0 and 100.0")]
        public double Grade { get; set; }

        [Required]
        [RegularExpression("Active|Completed|Dropped", ErrorMessage = "Invalid status")]
        public string Status { get; set; } = string.Empty;
    }
}
