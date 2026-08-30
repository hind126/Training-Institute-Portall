using Microsoft.AspNetCore.Mvc;

namespace FinalAssignmentBrief.Controllers
{
    public class ApiStudentDemoController : Controller
    {
        public IActionResult Students()
        {
            return View();
        }
    }
}
