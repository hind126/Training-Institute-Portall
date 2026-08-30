namespace FinalAssignmentBrief.DTOs
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double GPA { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
