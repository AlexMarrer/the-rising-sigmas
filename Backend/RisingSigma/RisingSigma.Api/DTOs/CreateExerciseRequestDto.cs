namespace RisingSigma.Api.DTOs;

public class CreateExerciseRequestDto
{
    public int Reps { get; set; }
    public int Sets { get; set; }
    public double RPE { get; set; }
    public int Day { get; set; }
    public string Notes { get; set; } = "";
    public Guid ExerciseTemplateId { get; set; }
    public List<int> Days { get; set; } = new List<int>(); // For multiple days support
}
