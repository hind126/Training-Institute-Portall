using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalAssignmentBrief.Data;
using FinalAssignmentBrief.Models;
using FinalAssignmentBrief.DTOs;

namespace FinalAssignmentBrief.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StudentsApiController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var students = await _context.Students
                 .Include(s => s.StudentProfile)
                 .Select(s => new StudentDTO
                 {
                     Id = s.Id,
                     FullName = s.FullName,
                     Email = s.Email,
                     GPA = s.GPA,
                     PhoneNumber = s.StudentProfile != null ? s.StudentProfile.PhoneNumber : null,
                     Address = s.StudentProfile != null ? s.StudentProfile.Address : null
                 })
                .ToListAsync();


            return Ok(students);
        }
    }
}
