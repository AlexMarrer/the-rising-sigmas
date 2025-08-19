namespace RisingSigma.Api.DTOs;

public class UpdateExerciseRequestDto
{
    public int Reps { get; set; }
    public int Sets { get; set; }
    public double RPE { get; set; }
    public int Day { get; set; }
    public string Notes { get; set; } = "";
    public Guid ExerciseTemplateId { get; set; }
}
