using System;

namespace RisingSigma.Api.Logic;

/// <summary>
/// Pure, single-source week number calculator for cyclic training plans.
/// Used directly by <see cref="ExerciseLogic"/> to avoid duplication and exercised by
/// both pure white-box tests (no DB) and integration-style tests (with DB/time freeze).
/// Guarantees identical behavior across contexts and prevents logic drift.
/// </summary>
internal static class WeekNumberCalculator
{
  #region Constants
  private const int DAYS_PER_WEEK = 7;
  private const int MIN_WEEK_NUMBER = 1;
  #endregion

  #region Public API
  public static int Calculate(DateTime trainingStartUtc, DateTime currentUtc, int cycleWeeks)
  {
    if (cycleWeeks <= 0) 
    {
        throw new ArgumentException("Cycle weeks must be greater than 0", nameof(cycleWeeks));
    }

    if (trainingStartUtc.Kind != DateTimeKind.Utc || currentUtc.Kind != DateTimeKind.Utc)
    {
        throw new ArgumentException("All DateTime parameters must be in UTC");

    }

    var daysSinceStart = (currentUtc.Date - trainingStartUtc.Date).Days;

    if (daysSinceStart < 0) { return MIN_WEEK_NUMBER; }

    var totalWeeksSinceStart = (daysSinceStart / DAYS_PER_WEEK) + MIN_WEEK_NUMBER;
    if (totalWeeksSinceStart > cycleWeeks)
    {
        return ((totalWeeksSinceStart - MIN_WEEK_NUMBER) % cycleWeeks) + MIN_WEEK_NUMBER;
    }

    return totalWeeksSinceStart;
  }
  #endregion
}
