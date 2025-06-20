using Microsoft.EntityFrameworkCore;
using RisingSigma.Database;
using RisingSigma.Database.Entities;
using RisingSigma.API.DTOs;
using RisingSigma.API.Extensions;

namespace RisingSigma.Api.Logic;

public class ExerciseLogic : IExerciseLogic
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly IVerificationLogic _verificationLogic;

    public ExerciseLogic(ApplicationDbContext applicationDbContext, IVerificationLogic verificationLogic, IConfiguration configuration)
    {
        this._applicationDbContext = applicationDbContext;
        this._verificationLogic = verificationLogic;
        this._configuration = configuration;
    }

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
    }    public async Task<CreateExerciseResponseDto> CreateExerciseAsync(CreateExerciseRequestDto request)
    {
        // Get or create default week plan (week 1)
        var weekPlan = await GetOrCreateDefaultWeekPlan();

        var exercises = new List<Exercise>();

        // Use selected days if any, otherwise use the single day
        var daysToCreate = request.Days?.Any() == true ? request.Days : new List<int> { request.Day };

        foreach (var day in daysToCreate)
        {
            var exercise = request.ToEntity(request.ExerciseTemplateId, weekPlan.Id, day);
            exercises.Add(exercise);
        }

        _applicationDbContext.Exercise.AddRange(exercises);
        await _applicationDbContext.SaveChangesAsync();

        // Return exercises with includes
        var exerciseIds = exercises.Select(e => e.Id).ToList();
        var savedExercises = await _applicationDbContext.Exercise
            .Include(e => e.ExerciseTemplate)
            .ThenInclude(et => et!.MuscleGroup)
            .Include(e => e.WeekPlan)
            .Where(e => exerciseIds.Contains(e.Id))
            .ToListAsync();

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

        // Check if we need to create a new muscle group
        if (request.MuscleGroupId.HasValue)
        {
            muscleGroupId = request.MuscleGroupId.Value;
        }
        else if (!string.IsNullOrEmpty(request.MuscleGroupName))
        {
            // Create new muscle group
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

        // Return template with includes
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
        // Check if data already exists
        if (await _applicationDbContext.MuscleGroup.AnyAsync())
        {
            return; // Data already seeded
        }

        // Create muscle groups
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

        // Create exercise templates
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
        var weekPlan = await _applicationDbContext.WeekPlan.FirstOrDefaultAsync();

        if (weekPlan == null)
        {
            // First, ensure we have a default user
            var user = await _applicationDbContext.User.FirstOrDefaultAsync();
            
            if (user == null)
            {
                // Create a default user
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Role = "DefaultUser"
                };
                
                _applicationDbContext.User.Add(user);
                await _applicationDbContext.SaveChangesAsync();
            }

            // Then, ensure we have a default training plan
            var trainingPlan = await _applicationDbContext.TrainingPlan.FirstOrDefaultAsync();
            
            if (trainingPlan == null)
            {
                // Create a default training plan
                trainingPlan = new TrainingPlan
                {
                    Id = Guid.NewGuid(),
                    Name = "Default Training Plan",
                    Description = "Auto-generated default training plan",
                    StartTime = DateTime.UtcNow,
                    CycleWeeks = 4,
                    UserId = user.Id // Reference the existing user
                };
                
                _applicationDbContext.TrainingPlan.Add(trainingPlan);
                await _applicationDbContext.SaveChangesAsync();
            }

            weekPlan = new WeekPlan
            {
                Id = Guid.NewGuid(),
                WeekNumber = 1,
                Version = 1,
                TrainingPlanId = trainingPlan.Id
            };

            _applicationDbContext.WeekPlan.Add(weekPlan);
            await _applicationDbContext.SaveChangesAsync();
        }

        return weekPlan;
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

        // Update exercise properties
        exercise.Reps = request.Reps;
        exercise.Sets = request.Sets;
        exercise.RPE = request.RPE;
        exercise.notes = request.Notes;
        exercise.ExerciseTemplateId = request.ExerciseTemplateId;        // Convert day integer to DayOfWeek enum (same logic as in CreateExercise)  
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
        // Delete all exercises first (due to foreign key constraints)
        var exercises = await _applicationDbContext.Exercise.ToListAsync();
        _applicationDbContext.Exercise.RemoveRange(exercises);

        // Delete all exercise templates
        var exerciseTemplates = await _applicationDbContext.ExerciseTemplate.ToListAsync();
        _applicationDbContext.ExerciseTemplate.RemoveRange(exerciseTemplates);

        // Delete all muscle groups
        var muscleGroups = await _applicationDbContext.MuscleGroup.ToListAsync();
        _applicationDbContext.MuscleGroup.RemoveRange(muscleGroups);

        // Delete all week plans
        var weekPlans = await _applicationDbContext.WeekPlan.ToListAsync();
        _applicationDbContext.WeekPlan.RemoveRange(weekPlans);

        // Delete all training plans
        var trainingPlans = await _applicationDbContext.TrainingPlan.ToListAsync();
        _applicationDbContext.TrainingPlan.RemoveRange(trainingPlans);

        await _applicationDbContext.SaveChangesAsync();
    }
}
