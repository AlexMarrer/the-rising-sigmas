using System.ComponentModel.DataAnnotations;

namespace RisingSigma.Database.Entities;

public class ExerciseTemplate
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public Guid MuscleGroupId { get; set; }

    public MuscleGroup? MuscleGroup { get; set; }
}
