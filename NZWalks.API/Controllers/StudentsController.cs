using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        [HttpGet]
       public IActionResult GetStudents()
        {
            string[] studentNames = new string[] { "saava", "denis" };

            return Ok(studentNames);

        }
    }
}
