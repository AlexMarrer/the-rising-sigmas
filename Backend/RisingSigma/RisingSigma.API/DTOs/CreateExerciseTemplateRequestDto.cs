namespace RisingSigma.API.DTOs;

public class CreateExerciseTemplateRequestDto
{
    public string Name { get; set; } = "";
    public Guid? MuscleGroupId { get; set; } // Optional - if null, create new muscle group
    public string? MuscleGroupName { get; set; } // For creating new muscle group
}
