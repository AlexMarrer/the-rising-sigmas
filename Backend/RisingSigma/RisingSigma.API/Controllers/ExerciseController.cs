using Microsoft.AspNetCore.Mvc;
using RisingSigma.Api.Logic;

namespace RisingSigma.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ILogger<ExerciseController> _logger;
        private readonly IExerciseLogic _exerciseLogic;

        public ExerciseController(ILogger<ExerciseController> logger, IExerciseLogic exerciseLogic)
        {
            _logger = logger;
            _exerciseLogic = exerciseLogic;
        }

        [HttpGet]
        public String GetHi()
        {
            return "Hi";
        }
    }
}
