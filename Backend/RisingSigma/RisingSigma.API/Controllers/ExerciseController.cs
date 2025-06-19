using Microsoft.AspNetCore.Mvc;
using RisingSigma.Api.Logic;
using RisingSigma.API.DTOs;

namespace RisingSigma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetAllExercises()
        {
            try
            {
                var exercises = await _exerciseLogic.GetAllExercisesAsync();
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all exercises");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("templates")]
        public async Task<ActionResult<IEnumerable<ExerciseTemplateDto>>> GetAllExerciseTemplates()
        {
            try
            {
                var templates = await _exerciseLogic.GetAllExerciseTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting exercise templates");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("musclegroups")]
        public async Task<ActionResult<IEnumerable<MuscleGroupDto>>> GetAllMuscleGroups()
        {
            try
            {
                var muscleGroups = await _exerciseLogic.GetAllMuscleGroupsAsync();
                return Ok(muscleGroups);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting muscle groups");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateExerciseResponseDto>> CreateExercise([FromBody] CreateExerciseRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Exercise data is required");
                }

                var response = await _exerciseLogic.CreateExerciseAsync(request);
                return CreatedAtAction(nameof(GetAllExercises), new { count = response.Count }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("templates")]
        public async Task<ActionResult<ExerciseTemplateDto>> CreateExerciseTemplate([FromBody] CreateExerciseTemplateRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Exercise template data is required");
                }

                var template = await _exerciseLogic.CreateExerciseTemplateAsync(request);
                return CreatedAtAction(nameof(GetAllExerciseTemplates), new { id = template.Id }, template);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating exercise template");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("musclegroups")]
        public async Task<ActionResult<MuscleGroupDto>> CreateMuscleGroup([FromBody] CreateMuscleGroupRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Muscle group data is required");
                }

                var muscleGroup = await _exerciseLogic.CreateMuscleGroupAsync(request);
                return CreatedAtAction(nameof(GetAllMuscleGroups), new { id = muscleGroup.Id }, muscleGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating muscle group");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExerciseDto>> UpdateExercise(Guid id, [FromBody] UpdateExerciseRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Exercise update data is required");
                }

                var updatedExercise = await _exerciseLogic.UpdateExerciseAsync(id, request);
                return Ok(updatedExercise);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Exercise not found with ID: {Id}", id);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating exercise with ID: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("seed-data")]
        public async Task<ActionResult> SeedData()
        {
            try
            {
                await _exerciseLogic.SeedDataAsync();
                return Ok(new { message = "Seed data created successfully", success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error seeding data");
                return StatusCode(500, new { message = "Internal server error", success = false });
            }
        }
    }
}
