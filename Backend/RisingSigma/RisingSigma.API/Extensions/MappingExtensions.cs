using RisingSigma.API.DTOs;
using RisingSigma.Database.Entities;

namespace RisingSigma.API.Extensions;

public static class MappingExtensions
{
    public static ExerciseDto ToDto(this Exercise entity)
    {
        // Convert DayOfWeek enum to day integer (0=Sunday, 1=Monday, etc.)
        int day = entity.Day switch
        {
            DayOfWeek.Sunday => 0,
            DayOfWeek.Monday => 1,
            DayOfWeek.Tuesday => 2,
            DayOfWeek.Wednesday => 3,
            DayOfWeek.Thursday => 4,
            DayOfWeek.Friday => 5,
            DayOfWeek.Saturday => 6,
            _ => 1 // Default to Monday
        };

        return new ExerciseDto
        {
            Id = entity.Id,
            Reps = entity.Reps,
            Sets = entity.Sets,
            RPE = entity.RPE,
            Day = day,
            Notes = entity.notes ?? "",
            ExerciseTemplateId = entity.ExerciseTemplateId,
            WeekPlanId = entity.WeekPlanId,
            ExerciseTemplate = entity.ExerciseTemplate?.ToDto(),
            WeekPlan = entity.WeekPlan?.ToDto()
        };
    }

    public static ExerciseTemplateDto ToDto(this ExerciseTemplate entity)
    {
        return new ExerciseTemplateDto
        {
            Id = entity.Id,
            Name = entity.Name ?? "",
            MuscleGroupId = entity.MuscleGroupId,
            MuscleGroup = entity.MuscleGroup?.ToDto()
        };
    }

    public static MuscleGroupDto ToDto(this MuscleGroup entity)
    {
        return new MuscleGroupDto
        {
            Id = entity.Id,
            Name = entity.Name ?? ""
        };
    }

    public static WeekPlanDto ToDto(this WeekPlan entity)
    {        return new WeekPlanDto
        {
            Id = entity.Id,
            WeekNumber = entity.WeekNumber,
            Version = entity.Version,
            TrainingPlanId = entity.TrainingPlanId
        };
    }

    public static Exercise ToEntity(this CreateExerciseRequestDto dto, Guid exerciseTemplateId, Guid weekPlanId, int day)
    {
        // Convert day integer to DayOfWeek enum (0=Sunday, 1=Monday, etc.)
        DayOfWeek dayOfWeek = day switch
        {
            0 => DayOfWeek.Sunday,
            1 => DayOfWeek.Monday,
            2 => DayOfWeek.Tuesday,
            3 => DayOfWeek.Wednesday,
            4 => DayOfWeek.Thursday,
            5 => DayOfWeek.Friday,
            6 => DayOfWeek.Saturday,
            _ => DayOfWeek.Monday
        };

        return new Exercise
        {
            Id = Guid.NewGuid(),
            Reps = dto.Reps,
            Sets = dto.Sets,
            RPE = dto.RPE,
            Day = dayOfWeek,
            notes = dto.Notes,
            ExerciseTemplateId = exerciseTemplateId,
            WeekPlanId = weekPlanId
        };
    }

    public static ExerciseTemplate ToEntity(this CreateExerciseTemplateRequestDto dto, Guid muscleGroupId)
    {
        return new ExerciseTemplate
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            MuscleGroupId = muscleGroupId
        };
    }

    public static MuscleGroup ToEntity(this CreateMuscleGroupRequestDto dto)
    {
        return new MuscleGroup
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };
    }
}
