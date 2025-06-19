namespace RisingSigma.API.DTOs;

public class CreateExerciseResponseDto
{
    public List<ExerciseDto> Exercises { get; set; } = new List<ExerciseDto>();
    public string Message { get; set; } = "";
    public int Count { get; set; }
}
