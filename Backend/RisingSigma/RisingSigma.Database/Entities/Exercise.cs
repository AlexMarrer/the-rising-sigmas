using System.ComponentModel.DataAnnotations;

namespace RisingSigma.Database.Entities;

public class Exercise
{
    [Key]
    public Guid Id { get; set; }
    public int Reps { get; set; }
    public int Sets { get; set; }
    public double RPE { get; set; }
    public DayOfWeek Day { get; set; }
    public string notes { get; set; } = "";

    public Guid WeekPlanId { get; set; }
    public Guid ExerciseTemplateId { get; set; }

    public WeekPlan? WeekPlan { get; set; }
    public ExerciseTemplate? ExerciseTemplate { get; set; }
}
