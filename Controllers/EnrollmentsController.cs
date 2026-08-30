using FinalAssignmentBrief.Data;
using FinalAssignmentBrief.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace TrainingPortal__AB_.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Index()
        {
            var enrollments = _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course);

            return View(await enrollments.ToListAsync());
        }
        [Authorize(Roles = "Admin,Instructor")]
        public IActionResult Create()
        {
            ViewBag.Students = new SelectList(_context.Students, "Id", "FullName");
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "Title");

            ViewBag.Statuses = new List<string>
            {
                "Active",
                "Completed",
                "Dropped"
            };

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var exists = await _context.Enrollments.AnyAsync(e =>
                    e.StudentId == enrollment.StudentId &&
                    e.CourseId == enrollment.CourseId);

                if (exists)
                {
                    ModelState.AddModelError("", "This student is already enrolled in this course.");
                }
                else
                {
                    _context.Add(enrollment);
                    await _context.SaveChangesAsync();

                    var student = await _context.Students
                        .Include(s => s.Enrollments)
                        .FirstOrDefaultAsync(s => s.Id == enrollment.StudentId);

                    if (student != null)
                    {
                        student.GPA = student.Enrollments
                            .Average(e => e.Grade) / 25.0;

                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Students = new SelectList(_context.Students, "Id", "FullName");
            ViewBag.Courses = new SelectList(_context.Courses, "Id", "Title");

            ViewBag.Statuses = new List<string>
            {
                "Active",
                "Completed",
                "Dropped"
            };

            return View(enrollment);
        }

        [Authorize(Roles = "Admin,Instructor,Student")]
        public async Task<IActionResult> Details(int studentId, int courseId)
        {
            var enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e =>
                    e.StudentId == studentId &&
                    e.CourseId == courseId);

            if (enrollment == null)
            {
                return NotFound();
            }

            return View(enrollment);
        }
    }
}