namespace RisingSigma.Api.DTOs;

public class WeekPlanDto
{
    public Guid Id { get; set; }
    public int WeekNumber { get; set; }
    public int Version { get; set; }
    public Guid TrainingPlanId { get; set; }
}
