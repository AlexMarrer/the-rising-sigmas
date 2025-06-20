using RisingSigma.API.DTOs;

namespace RisingSigma.Api.Logic;

public interface IExerciseLogic
{    Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync();
    Task<IEnumerable<ExerciseTemplateDto>> GetAllExerciseTemplatesAsync();
    Task<IEnumerable<MuscleGroupDto>> GetAllMuscleGroupsAsync();
    Task<CreateExerciseResponseDto> CreateExerciseAsync(CreateExerciseRequestDto request);
    Task<ExerciseTemplateDto> CreateExerciseTemplateAsync(CreateExerciseTemplateRequestDto request);
    Task<MuscleGroupDto> CreateMuscleGroupAsync(CreateMuscleGroupRequestDto request);
    Task SeedDataAsync();
    Task<ExerciseDto> UpdateExerciseAsync(Guid id, UpdateExerciseRequestDto request);
    Task ClearAllTrainingDataAsync();
}
