using Microsoft.AspNetCore.Mvc;

namespace RisingSigma.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ILogger<ExerciseController> _logger;

        public ExerciseController(ILogger<ExerciseController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public String GetHi()
        {
            return "Hi";
        }
    }
}
