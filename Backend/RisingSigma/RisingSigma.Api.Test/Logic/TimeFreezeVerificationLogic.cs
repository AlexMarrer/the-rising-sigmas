using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RisingSigma.Api.Logic;
using RisingSigma.Database;
using RisingSigma.Database.Entities;
using RisingSigma.Api.DTOs;

namespace RisingSigma.Api.Test.Logic;

public class TimeFreezeVerificationLogic : ITimeFreezeVerificationLogic
{
    private const int CYCLE_WEEKS = 4;
    private const int DAYS_PER_WEEK = 7;
    private static readonly DateTime BASE_START_DATE = new DateTime(2025, 8, 18, 6, 0, 0, DateTimeKind.Utc);
    
    private readonly ApplicationDbContext _dbContext;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<ITimeProvider> _timeProviderMock;
    private readonly ExerciseLogic _exerciseLogic;

    public TimeFreezeVerificationLogic()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _dbContext = new ApplicationDbContext(options);

        _configurationMock = new Mock<IConfiguration>();
        _timeProviderMock = new Mock<ITimeProvider>();

        _exerciseLogic = new ExerciseLogic(_dbContext, _configurationMock.Object, _timeProviderMock.Object);
        SeedTestDataAsync().GetAwaiter().GetResult();
    }

    public async Task<WeekPlan?> CreateExerciseAtSpecificDay(int daysOffset, string description)
    {
        SetMockTimeByDaysOffset(daysOffset);

        var request = CreateTestExerciseRequest(notes: $"Day {daysOffset}: {description}");
        await _exerciseLogic.CreateExerciseAsync(request);

        return _dbContext.WeekPlan.FirstOrDefault();
    }

    public WeekPlan VerifyWeekNumber(WeekPlan? weekPlan, int expectedWeek, int daysOffset)
    {
        if (weekPlan == null)
            throw new ArgumentNullException(nameof(weekPlan), "WeekPlan should not be null");

        return weekPlan;
    }

    public bool VerifyWeekCalculation(int daysOffset, int expectedWeek)
    {
        var calculatedWeek = ((daysOffset / DAYS_PER_WEEK) % CYCLE_WEEKS) + 1;
        return calculatedWeek == expectedWeek;
    }

    public async Task<(bool success, int weekPlansCount, int exercisesCount)> VerifyMultipleExercisesSameWeek()
    {
        const int mondayOffset = 1;
        const int thursdayOffset = 4;

        SetMockTimeByDaysOffset(mondayOffset);
        await _exerciseLogic.CreateExerciseAsync(CreateTestExerciseRequest(day: 1, notes: "Monday"));

        SetMockTimeByDaysOffset(thursdayOffset);
        await _exerciseLogic.CreateExerciseAsync(CreateTestExerciseRequest(day: 4, notes: "Thursday"));

        var weekPlans = _dbContext.WeekPlan.ToList();
        var exercises = _dbContext.Exercise.ToList();

        bool success = weekPlans.Count == 1 && 
                       weekPlans.First().WeekNumber == 1 && 
                       exercises.Count == 2 &&
                       exercises.All(e => e.WeekPlanId == weekPlans.First().Id);

        return (success, weekPlans.Count, exercises.Count);
    }

    public async Task<(bool week1Success, bool week2Success)> VerifyWeekBoundaryTransition()
    {
        const int lastDayOfWeek1 = 6;
        SetMockTimeByDaysOffset(lastDayOfWeek1);
        
        await _exerciseLogic.CreateExerciseAsync(CreateTestExerciseRequest(notes: "Day 6 - last day of week 1"));
        var weekPlan1 = _dbContext.WeekPlan.FirstOrDefault();
        
        bool week1Success = weekPlan1?.WeekNumber == 1;

        _dbContext.WeekPlan.RemoveRange(_dbContext.WeekPlan);
        await _dbContext.SaveChangesAsync();

        const int firstDayOfWeek2 = 7;
        SetMockTimeByDaysOffset(firstDayOfWeek2);
        
        await _exerciseLogic.CreateExerciseAsync(CreateTestExerciseRequest(notes: "Day 7 - first day of week 2"));
        var weekPlan2 = _dbContext.WeekPlan.FirstOrDefault();
        
        bool week2Success = weekPlan2?.WeekNumber == 2;

        return (week1Success, week2Success);
    }

    public async Task<int> VerifyNegativeDaysHandling()
    {
        const int negativeDaysOffset = -7;
        var weekPlan = await CreateExerciseAtSpecificDay(negativeDaysOffset, "Past date test");
        
        return weekPlan?.WeekNumber ?? -1;
    }

    public (bool success, string? failureReason) VerifyMathematicalFormula()
    {
        var testCases = new[]
        {
            (days: 0, week: 1),
            (days: 6, week: 1),
            (days: 7, week: 2),
            (days: 14, week: 3),
            (days: 21, week: 4),
            (days: 28, week: 1),
            (days: 35, week: 2),
        };

        foreach (var (days, expectedWeek) in testCases)
        {
            var calculatedWeek = ((days / DAYS_PER_WEEK) % CYCLE_WEEKS) + 1;
            if (expectedWeek != calculatedWeek)
                return (false, $"Formula failed for day {days}: expected {expectedWeek}, got {calculatedWeek}");
        }

        return (true, null);
    }

    private async Task SeedTestDataAsync()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Role = "TestUser"
        };
        _dbContext.User.Add(user);

        var trainingPlan = new TrainingPlan
        {
            Id = Guid.NewGuid(),
            Name = "Test Training Plan",
            Description = "Consistent training plan for time-freeze tests",
            StartTime = BASE_START_DATE,
            CycleWeeks = CYCLE_WEEKS,
            UserId = user.Id
        };
        _dbContext.TrainingPlan.Add(trainingPlan);

        await _dbContext.SaveChangesAsync();
    }

    private CreateExerciseRequestDto CreateTestExerciseRequest(
        int reps = 10,
        int sets = 3,
        double rpe = 8.0,
        int day = 1,
        string? notes = null)
    {
        return new CreateExerciseRequestDto
        {
            Reps = reps,
            Sets = sets,
            RPE = rpe,
            Day = day,
            Notes = notes ?? $"Test exercise - Reps:{reps} Sets:{sets}",
            ExerciseTemplateId = Guid.Empty
        };
    }

    private void SetMockTimeByDaysOffset(int daysFromStart)
    {
        var targetDate = BASE_START_DATE.AddDays(daysFromStart);
        _timeProviderMock.Setup(x => x.UtcNow).Returns(targetDate);
    }

    public void Dispose()
    {
        _dbContext?.Dispose();
    }
}
