
using RisingSigma.Api.Test.Logic;
using Xunit;

namespace RisingSigma.Api.Test;

public class TimeFreezeTests
{
    private readonly ITimeFreezeVerificationLogic _verificationLogic;
    
    public TimeFreezeTests()
    {
        _verificationLogic = new TimeFreezeVerificationLogic();
    }
    #region Basic Week Rotation Tests

    [Fact]
    public async Task Day0_Should_ReturnWeek1()
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(0, "First day of program");
        Assert.NotNull(weekPlan);
        Assert.Equal(1, weekPlan.WeekNumber);
    }

    [Fact]
    public async Task Day7_Should_ReturnWeek2()
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(7, "Start of week 2");
        Assert.NotNull(weekPlan);
        Assert.Equal(2, weekPlan.WeekNumber);
    }

    [Fact]
    public async Task Day14_Should_ReturnWeek3()
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(14, "Start of week 3");
        Assert.NotNull(weekPlan);
        Assert.Equal(3, weekPlan.WeekNumber);
    }

    [Fact]
    public async Task Day21_Should_ReturnWeek4()
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(21, "Start of week 4");
        Assert.NotNull(weekPlan);
        Assert.Equal(4, weekPlan.WeekNumber);
    }

    [Fact]
    public async Task Day28_CycleRestart_Should_ReturnWeek1()
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(28, "4-week cycle restart");
        Assert.NotNull(weekPlan);
        Assert.Equal(1, weekPlan.WeekNumber);
    }

    #endregion

    #region Parametrized Week Calculation Tests

    public static IEnumerable<object[]> WeekRotationTestData()
    {
        yield return new object[] { 0, 1, "Start of program" };
        yield return new object[] { 3, 1, "Mid week 1" };
        yield return new object[] { 6, 1, "Last day of week 1" };
        yield return new object[] { 7, 2, "First day of week 2" };
        yield return new object[] { 10, 2, "Mid week 2" };
        yield return new object[] { 13, 2, "Last day of week 2" };
        yield return new object[] { 14, 3, "First day of week 3" };
        yield return new object[] { 17, 3, "Mid week 3" };
        yield return new object[] { 20, 3, "Last day of week 3" };
        yield return new object[] { 21, 4, "First day of week 4" };
        yield return new object[] { 24, 4, "Mid week 4" };
        yield return new object[] { 27, 4, "Last day of week 4" };
        yield return new object[] { 28, 1, "Second cycle - week 1" };
        yield return new object[] { 35, 2, "Second cycle - week 2" };
        yield return new object[] { 42, 3, "Second cycle - week 3" };
        yield return new object[] { 49, 4, "Second cycle - week 4" };
        yield return new object[] { 56, 1, "Third cycle - week 1" };
        yield return new object[] { 365, 1, "One year later" };
        yield return new object[] { 730, 1, "Two years later" };
    }

    [Theory]
    [MemberData(nameof(WeekRotationTestData))]
    public async Task VariousDaysOffset_Should_ReturnCorrectWeekNumber(int daysOffset, int expectedWeek, string description)
    {
        var weekPlan = await _verificationLogic.CreateExerciseAtSpecificDay(daysOffset, description);
        Assert.NotNull(weekPlan);
        Assert.Equal(expectedWeek, weekPlan.WeekNumber);
        Assert.True(_verificationLogic.VerifyWeekCalculation(daysOffset, expectedWeek));
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public async Task MultipleExercisesSameWeek_Should_ShareWeekPlan()
    {
        var (success, weekPlansCount, exercisesCount) = await _verificationLogic.VerifyMultipleExercisesSameWeek();
        Assert.True(success);
        Assert.Equal(1, weekPlansCount);
        Assert.Equal(2, exercisesCount);
    }

    [Fact]
    public async Task MidnightBoundaryWeekTransition_Should_ReturnCorrectWeek()
    {
        var (week1Success, week2Success) = await _verificationLogic.VerifyWeekBoundaryTransition();
        Assert.True(week1Success);
        Assert.True(week2Success);
    }

    [Fact]
    public async Task NegativeDaysOffset_Should_HandleGracefully()
    {
        var weekNumber = await _verificationLogic.VerifyNegativeDaysHandling();
        Assert.Equal(1, weekNumber);
    }

    #endregion

    #region Verification Tests

    [Fact]
    public void WeekCalculationFormula_Should_BeCorrect()
    {
        var (success, failureReason) = _verificationLogic.VerifyMathematicalFormula();
        Assert.True(success, failureReason ?? "Mathematical formula verification failed");
    }

    #endregion
}

