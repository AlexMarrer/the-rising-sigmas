using RisingSigma.Api.DTOs;
using RisingSigma.Database.Entities;

namespace RisingSigma.Api.Test.Logic;

public interface ITimeFreezeVerificationLogic
{
    Task<WeekPlan?> CreateExerciseAtSpecificDay(int daysOffset, string description);
    WeekPlan VerifyWeekNumber(WeekPlan? weekPlan, int expectedWeek, int daysOffset);
    bool VerifyWeekCalculation(int daysOffset, int expectedWeek);
    Task<(bool success, int weekPlansCount, int exercisesCount)> VerifyMultipleExercisesSameWeek();
    Task<(bool week1Success, bool week2Success)> VerifyWeekBoundaryTransition();
    Task<int> VerifyNegativeDaysHandling();
    (bool success, string? failureReason) VerifyMathematicalFormula();
}
