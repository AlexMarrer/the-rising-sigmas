namespace RisingSigma.API.DTOs;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public int Reps { get; set; }
    public int Sets { get; set; }
    public double RPE { get; set; }
    public int Day { get; set; }
    public string Notes { get; set; } = "";
    public Guid ExerciseTemplateId { get; set; }
    public Guid WeekPlanId { get; set; }
    public ExerciseTemplateDto? ExerciseTemplate { get; set; }
    public WeekPlanDto? WeekPlan { get; set; }
}
