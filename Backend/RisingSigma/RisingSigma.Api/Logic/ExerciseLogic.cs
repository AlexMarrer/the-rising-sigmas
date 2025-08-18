using Microsoft.EntityFrameworkCore;
using RisingSigma.Database;
using RisingSigma.Database.Entities;
using RisingSigma.Api.DTOs;
using RisingSigma.Api.Extensions;
using System;

namespace RisingSigma.Api.Logic;

public class ExerciseLogic : IExerciseLogic
{
    #region Constants
    private const int DEFAULT_CYCLE_WEEKS = 4;
    private const int DAYS_PER_WEEK = 7;
    private const int MIN_WEEK_NUMBER = 1;
    private const string DEFAULT_USER_ROLE = "DefaultUser";
    private const string DEFAULT_TRAINING_PLAN_NAME = "Default Training Plan";
    private const string DEFAULT_TRAINING_PLAN_DESCRIPTION = "Auto-generated default training plan";
    #endregion

    #region Fields
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly ITimeProvider _timeProvider;
    #endregion

    #region Constructor
    public ExerciseLogic(ApplicationDbContext context, IConfiguration config, ITimeProvider timeProvider)
    {
        _applicationDbContext = context ?? throw new ArgumentNullException(nameof(context));
        _configuration = config ?? throw new ArgumentNullException(nameof(config));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }
    #endregion

    #region Public Methods
    
    public async Task<IEnumerable<ExerciseDto>> GetAllExercisesAsync()
    {
        var exercises = await _applicationDbContext.Exercise
            .Include(e => e.ExerciseTemplate)
            .ThenInclude(et => et!.MuscleGroup)
            .Include(e => e.WeekPlan)
            .ToListAsync();

        return exercises.Select(e => e.ToDto());
    }

    public async Task<IEnumerable<ExerciseTemplateDto>> GetAllExerciseTemplatesAsync()
    {
        var templates = await _applicationDbContext.ExerciseTemplate
            .Include(et => et.MuscleGroup)
            .ToListAsync();

        return templates.Select(et => et.ToDto());
    }

    public async Task<IEnumerable<MuscleGroupDto>> GetAllMuscleGroupsAsync()
    {
        var muscleGroups = await _applicationDbContext.MuscleGroup.ToListAsync();
        return muscleGroups.Select(mg => mg.ToDto());
    }

    public async Task<CreateExerciseResponseDto> CreateExerciseAsync(CreateExerciseRequestDto request)
    {
        ValidateCreateExerciseRequest(request);
        
        var weekPlan = await GetOrCreateDefaultWeekPlan();
        var exercises = new List<Exercise>();

        var daysToCreate = request.Days?.Any() == true ? request.Days : new List<int> { request.Day };

        foreach (var day in daysToCreate)
        {
            ValidateDayNumber(day);
            var exercise = request.ToEntity(request.ExerciseTemplateId, weekPlan.Id, day);
            exercises.Add(exercise);
        }

        _applicationDbContext.Exercise.AddRange(exercises);
        await _applicationDbContext.SaveChangesAsync();

        var savedExercises = await GetSavedExercisesWithIncludes(exercises.Select(e => e.Id).ToList());

        return new CreateExerciseResponseDto
        {
            Exercises = savedExercises.Select(e => e.ToDto()).ToList(),
            Count = savedExercises.Count,
            Message = $"{savedExercises.Count} exercise(s) created successfully"
        };
    }

    public async Task<ExerciseTemplateDto> CreateExerciseTemplateAsync(CreateExerciseTemplateRequestDto request)
    {
        Guid muscleGroupId;

        if (request.MuscleGroupId.HasValue)
        {
            muscleGroupId = request.MuscleGroupId.Value;
        }
        else if (!string.IsNullOrEmpty(request.MuscleGroupName))
        {
            var newMuscleGroup = new MuscleGroup
            {
                Id = Guid.NewGuid(),
                Name = request.MuscleGroupName
            };

            _applicationDbContext.MuscleGroup.Add(newMuscleGroup);
            await _applicationDbContext.SaveChangesAsync();
            muscleGroupId = newMuscleGroup.Id;
        }
        else
        {
            throw new ArgumentException("Either MuscleGroupId or MuscleGroupName must be provided");
        }

        var template = request.ToEntity(muscleGroupId);

        _applicationDbContext.ExerciseTemplate.Add(template);
        await _applicationDbContext.SaveChangesAsync();

        var result = await _applicationDbContext.ExerciseTemplate
            .Include(et => et.MuscleGroup)
            .FirstOrDefaultAsync(et => et.Id == template.Id);

        return result?.ToDto() ?? template.ToDto();
    }

    public async Task<MuscleGroupDto> CreateMuscleGroupAsync(CreateMuscleGroupRequestDto request)
    {
        var muscleGroup = request.ToEntity();

        _applicationDbContext.MuscleGroup.Add(muscleGroup);
        await _applicationDbContext.SaveChangesAsync();

        return muscleGroup.ToDto();
    }

    public async Task SeedDataAsync()
    {
        if (await _applicationDbContext.MuscleGroup.AnyAsync())
        {
            return;
        }

        var muscleGroups = new[]
        {
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Brust" },
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Rücken" },
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Beine" },
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Schultern" },
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Arme" },
            new MuscleGroup { Id = Guid.NewGuid(), Name = "Bauch" },
        };

        _applicationDbContext.MuscleGroup.AddRange(muscleGroups);

        var exerciseTemplates = new[]
        {
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Bankdrücken", MuscleGroupId = muscleGroups[0].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Fliegende", MuscleGroupId = muscleGroups[0].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Klimmzüge", MuscleGroupId = muscleGroups[1].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Rudern", MuscleGroupId = muscleGroups[1].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Kniebeugen", MuscleGroupId = muscleGroups[2].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Beinpresse", MuscleGroupId = muscleGroups[2].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Schulterdrücken", MuscleGroupId = muscleGroups[3].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Seitheben", MuscleGroupId = muscleGroups[3].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Bizep Curls", MuscleGroupId = muscleGroups[4].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Trizep Dips", MuscleGroupId = muscleGroups[4].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Crunches", MuscleGroupId = muscleGroups[5].Id },
            new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Plank", MuscleGroupId = muscleGroups[5].Id },
        };

        _applicationDbContext.ExerciseTemplate.AddRange(exerciseTemplates);

        await _applicationDbContext.SaveChangesAsync();
    }    private async Task<WeekPlan> GetOrCreateDefaultWeekPlan()
    {
        var trainingPlan = await GetOrCreateDefaultTrainingPlan();
        var currentWeekNumber = CalculateCurrentWeekNumber(trainingPlan.StartTime, _timeProvider.UtcNow, trainingPlan.CycleWeeks);
        
        var weekPlan = await _applicationDbContext.WeekPlan
            .Where(wp => wp.TrainingPlanId == trainingPlan.Id && wp.WeekNumber == currentWeekNumber)
            .FirstOrDefaultAsync();

        if (weekPlan == null)
        {
            weekPlan = new WeekPlan
            {
                Id = Guid.NewGuid(),
                WeekNumber = currentWeekNumber,
                Version = 1,
                TrainingPlanId = trainingPlan.Id
            };

            _applicationDbContext.WeekPlan.Add(weekPlan);
            await _applicationDbContext.SaveChangesAsync();
        }

        return weekPlan;
    }

    private async Task<TrainingPlan> GetOrCreateDefaultTrainingPlan()
    {
        var trainingPlan = await _applicationDbContext.TrainingPlan.FirstOrDefaultAsync();
        
        if (trainingPlan == null)
        {
            var user = await _applicationDbContext.User.FirstOrDefaultAsync();
            
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Role = DEFAULT_USER_ROLE
                };
                
                _applicationDbContext.User.Add(user);
                await _applicationDbContext.SaveChangesAsync();
            }

            trainingPlan = new TrainingPlan
            {
                Id = Guid.NewGuid(),
                Name = DEFAULT_TRAINING_PLAN_NAME,
                Description = DEFAULT_TRAINING_PLAN_DESCRIPTION,
                StartTime = _timeProvider.UtcNow,
                CycleWeeks = DEFAULT_CYCLE_WEEKS,
                UserId = user.Id
            };
            
            _applicationDbContext.TrainingPlan.Add(trainingPlan);
            await _applicationDbContext.SaveChangesAsync();
        }

        return trainingPlan;
    }

    /// <summary>
    /// Calculates the current week number based on training start time and cycle configuration.
    /// Implements a robust 4-week cycling algorithm with proper boundary handling.
    /// </summary>
    /// <param name="trainingStartTime">The start date of the training plan</param>
    /// <param name="currentTime">The current date/time</param>
    /// <param name="cycleWeeks">Number of weeks in one training cycle</param>
    /// <returns>Week number (1-based) within the current cycle</returns>
    private int CalculateCurrentWeekNumber(DateTime trainingStartTime, DateTime currentTime, int cycleWeeks)
    {
        ValidateCalculationInputs(trainingStartTime, currentTime, cycleWeeks);
        
        var daysSinceStart = (currentTime.Date - trainingStartTime.Date).Days;
        
        // Handle negative days (current time before start time)
        if (daysSinceStart < 0)
        {
            return MIN_WEEK_NUMBER;
        }
        
        var totalWeeksSinceStart = (daysSinceStart / DAYS_PER_WEEK) + MIN_WEEK_NUMBER;
        
        // Apply cyclic rotation for weeks beyond the cycle length
        if (totalWeeksSinceStart > cycleWeeks)
        {
            return ((totalWeeksSinceStart - MIN_WEEK_NUMBER) % cycleWeeks) + MIN_WEEK_NUMBER;
        }
        
        return totalWeeksSinceStart;
    }
    #endregion

    #region Validation Methods
    
    private static void ValidateCreateExerciseRequest(CreateExerciseRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
            
        if (request.Reps <= 0)
            throw new ArgumentException("Reps must be greater than 0", nameof(request.Reps));
            
        if (request.Sets <= 0)
            throw new ArgumentException("Sets must be greater than 0", nameof(request.Sets));
            
        if (request.RPE < 0 || request.RPE > 10)
            throw new ArgumentException("RPE must be between 0 and 10", nameof(request.RPE));
    }
    
    private static void ValidateDayNumber(int day)
    {
        if (day < 0 || day > 6)
            throw new ArgumentException($"Day must be between 0 and 6, but was {day}", nameof(day));
    }
    
    private static void ValidateCalculationInputs(DateTime trainingStartTime, DateTime currentTime, int cycleWeeks)
    {
        if (cycleWeeks <= 0)
            throw new ArgumentException("Cycle weeks must be greater than 0", nameof(cycleWeeks));
            
        if (trainingStartTime.Kind != DateTimeKind.Utc || currentTime.Kind != DateTimeKind.Utc)
            throw new ArgumentException("All DateTime parameters must be in UTC");
    }
    #endregion
    
    #region Helper Methods
    
    private async Task<List<Exercise>> GetSavedExercisesWithIncludes(List<Guid> exerciseIds)
    {
        return await _applicationDbContext.Exercise
            .Include(e => e.ExerciseTemplate)
            .ThenInclude(et => et!.MuscleGroup)
            .Include(e => e.WeekPlan)
            .Where(e => exerciseIds.Contains(e.Id))
            .ToListAsync();
    }    public async Task<ExerciseDto> UpdateExerciseAsync(Guid id, UpdateExerciseRequestDto request)
    {
        var exercise = await _applicationDbContext.Exercise
            .Include(e => e.ExerciseTemplate)
            .ThenInclude(et => et!.MuscleGroup)
            .Include(e => e.WeekPlan)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (exercise == null)
        {
            throw new ArgumentException($"Exercise with ID {id} not found");
        }

        exercise.Reps = request.Reps;
        exercise.Sets = request.Sets;
        exercise.RPE = request.RPE;
        exercise.notes = request.Notes;
        exercise.ExerciseTemplateId = request.ExerciseTemplateId;
        
        DayOfWeek dayOfWeek = request.Day switch
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
        exercise.Day = dayOfWeek;

        await _applicationDbContext.SaveChangesAsync();

        return exercise.ToDto();
    }

    public async Task ClearAllTrainingDataAsync()
    {
        var exercises = await _applicationDbContext.Exercise.ToListAsync();
        _applicationDbContext.Exercise.RemoveRange(exercises);

        var exerciseTemplates = await _applicationDbContext.ExerciseTemplate.ToListAsync();
        _applicationDbContext.ExerciseTemplate.RemoveRange(exerciseTemplates);

        var muscleGroups = await _applicationDbContext.MuscleGroup.ToListAsync();
        _applicationDbContext.MuscleGroup.RemoveRange(muscleGroups);

        var weekPlans = await _applicationDbContext.WeekPlan.ToListAsync();
        _applicationDbContext.WeekPlan.RemoveRange(weekPlans);

        var trainingPlans = await _applicationDbContext.TrainingPlan.ToListAsync();
        _applicationDbContext.TrainingPlan.RemoveRange(trainingPlans);

        await _applicationDbContext.SaveChangesAsync();
    }
    #endregion
}