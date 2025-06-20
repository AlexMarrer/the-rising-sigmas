using System.ComponentModel.DataAnnotations;

namespace RisingSigma.Database.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string Role { get; set; } = "";

    public List<TrainingPlan>? Plan { get; set; }
}
