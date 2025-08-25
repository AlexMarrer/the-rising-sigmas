using RisingSigma.Api.DTOs;
using RisingSigma.Database.Entities;

namespace RisingSigma.Api.Test.Logic;

public interface IExerciseUpdateVerifier : IDisposable
{
  Guid ExerciseId { get; }
  Guid TemplateId { get; }
  UpdateExerciseRequestDto MakeRequest(int reps = 12, int sets = 4, double rpe = 8.0, string? notes = "updated", int day = 3, Guid? templateId = null);
  Task<ExerciseDto> UpdateAsync(UpdateExerciseRequestDto request);
  Task<ExerciseDto> UpdateAsync(Guid exerciseId, UpdateExerciseRequestDto request);
  Exercise GetExerciseEntity();
  Guid AddNewTemplate(string name = "New Template", string muscleGroupName = "Chest");
}
