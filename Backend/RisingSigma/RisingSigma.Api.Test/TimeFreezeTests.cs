using RisingSigma.Api.Test.Logic;
using Xunit;

namespace RisingSigma.Api.Test;

public class TimeFreezeTests
{
    #region Basic Week Rotation Tests

    [Fact]
    public async Task Day0_Should_ReturnWeek1()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(0, "First day of program");
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek: 1, daysOffset: 0);
        Assert.NotNull(result);
        Assert.Equal(1, result.WeekNumber);
    }

    [Fact]
    public async Task Day7_Should_ReturnWeek2()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(7, "Start of week 2");
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek: 2, daysOffset: 7);
        Assert.NotNull(result);
        Assert.Equal(2, result.WeekNumber);
    }

    [Fact]
    public async Task Day14_Should_ReturnWeek3()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(14, "Start of week 3");
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek: 3, daysOffset: 14);
        Assert.NotNull(result);
        Assert.Equal(3, result.WeekNumber);
    }

    [Fact]
    public async Task Day21_Should_ReturnWeek4()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(21, "Start of week 4");
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek: 4, daysOffset: 21);
        Assert.NotNull(result);
        Assert.Equal(4, result.WeekNumber);
    }

    [Fact]
    public async Task Day28_CycleRestart_Should_ReturnWeek1()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(28, "4-week cycle restart");
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek: 1, daysOffset: 28);
        Assert.NotNull(result);
        Assert.Equal(1, result.WeekNumber);
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
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekPlan = await verificationLogic.CreateExerciseAtSpecificDay(daysOffset, description);
        
        var result = verificationLogic.VerifyWeekNumber(weekPlan, expectedWeek, daysOffset);
        Assert.NotNull(result);
        Assert.Equal(expectedWeek, result.WeekNumber);
        
        Assert.True(verificationLogic.VerifyWeekCalculation(daysOffset, expectedWeek));
    }

    #endregion

    #region Edge Cases and Boundary Tests

    [Fact]
    public async Task MultipleExercisesSameWeek_Should_ShareWeekPlan()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var (success, weekPlansCount, exercisesCount) = await verificationLogic.VerifyMultipleExercisesSameWeek();
        
        Assert.True(success);
        Assert.Equal(1, weekPlansCount);
        Assert.Equal(2, exercisesCount);
    }

    [Fact]
    public async Task MidnightBoundaryWeekTransition_Should_ReturnCorrectWeek()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var (week1Success, week2Success) = await verificationLogic.VerifyWeekBoundaryTransition();
        
        Assert.True(week1Success);
        Assert.True(week2Success);
    }

    [Fact]
    public async Task NegativeDaysOffset_Should_HandleGracefully()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var weekNumber = await verificationLogic.VerifyNegativeDaysHandling();
        
        Assert.Equal(1, weekNumber);
    }

    #endregion

    #region Verification Tests

    [Fact]
    public void WeekCalculationFormula_Should_BeCorrect()
    {
        var verificationLogic = new TimeFreezeVerificationLogic();
        var (success, failureReason) = verificationLogic.VerifyMathematicalFormula();
        
        Assert.True(success, failureReason ?? "Mathematical formula verification failed");
    }

    #endregion
}
