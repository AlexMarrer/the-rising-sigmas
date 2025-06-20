using System.ComponentModel.DataAnnotations;

namespace RisingSigma.Database.Entities;

public class MuscleGroup
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
}
