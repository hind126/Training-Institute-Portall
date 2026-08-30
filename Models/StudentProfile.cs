namespace FinalAssignmentBrief.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string EmergencyContact { get; set; } = string.Empty;

        public int StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
