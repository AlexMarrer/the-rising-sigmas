using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RisingSigma.Database.Entities;

public class WeekPlan
{
    [Key]
    public Guid Id { get; set; }
    public int WeekNumber { get; set; }
    public int Version { get; set; }
    public Guid TrainingPlanId { get; set; }

    public TrainingPlan? TrainingPlan { get; set; }
    public List<Exercise>? Exercise { get; set; }
}
