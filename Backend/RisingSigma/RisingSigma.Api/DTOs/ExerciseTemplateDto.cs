namespace RisingSigma.Api.DTOs;

public class ExerciseTemplateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public Guid MuscleGroupId { get; set; }
    public MuscleGroupDto? MuscleGroup { get; set; }
}
