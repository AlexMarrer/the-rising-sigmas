using RisingSigma.Api.Logic;
using System;
using System.Globalization;
using Xunit;

namespace RisingSigma.Api.Test.Tests;

/// <summary>
/// White-Box Test 1: Pure logic (no DB) covering all branches of WeekNumberCalculator.Calculate
/// Decision points: cycleWeeks<=0, UTC kind check, daysSinceStart<0, totalWeeksSinceStart>cycleWeeks
/// => 5+ logical paths (>5 -> max points)
/// </summary>
public class WeekNumberCalculatorTests
{
  #region Helpers
  private static DateTime D(int y, int m, int d) => new(y, m, d, 0, 0, 0, DateTimeKind.Utc);
  #endregion

  #region First Cycle
  [Theory]
  // exact week boundaries inside first cycle
  [InlineData("2025-01-01", "2025-01-01", 4, 1)] // start day
  [InlineData("2025-01-01", "2025-01-08", 4, 2)] // exactly 7 days difference -> week 2
  [InlineData("2025-01-01", "2025-01-15", 4, 3)] // 14 days diff -> week 3
  [InlineData("2025-01-01", "2025-01-22", 4, 4)] // 21 days diff -> week 4
  public void FirstCycleWeeks(string start, string current, int cycleWeeks, int expected)
  {
    var startUtc = DateTime.SpecifyKind(
        DateTime.Parse(start, CultureInfo.InvariantCulture),
        DateTimeKind.Utc);
    var currentUtc = DateTime.SpecifyKind(
        DateTime.Parse(current, CultureInfo.InvariantCulture),
        DateTimeKind.Utc);

    var result = WeekNumberCalculator.Calculate(startUtc, currentUtc, cycleWeeks);
    Assert.Equal(expected, result);
  }

  #endregion

  #region Relative Position Cases
  [Fact]
  public void NegativeDays_ReturnsWeek1()
  {
    var result = WeekNumberCalculator.Calculate(D(2025, 1, 10), D(2025, 1, 5), 4);
    Assert.Equal(1, result);
  }

  [Fact]
  public void Wraps_AfterFullCycle()
  {
    // 5th week -> wraps to week 1
    var result = WeekNumberCalculator.Calculate(D(2025, 1, 1), D(2025, 1, 29), 4);
    Assert.Equal(1, result);
  }

  [Fact]
  public void Wraps_IntoWeek2()
  {
    var result = WeekNumberCalculator.Calculate(D(2025, 1, 1), D(2025, 2, 5), 4); // 35 days -> week 2
    Assert.Equal(2, result);
  }
  #endregion

  #region Validation
  [Theory]
  [InlineData(0)]
  [InlineData(-1)]
  public void InvalidCycleWeeks_Throws(int cycleWeeks)
  {
    Assert.Throws<ArgumentException>(() => WeekNumberCalculator.Calculate(D(2025, 1, 1), D(2025, 1, 2), cycleWeeks));
  }

  [Fact]
  public void NonUtcDates_Throw()
  {
    var startLocal = new DateTime(2025, 1, 1); // Unspecified -> not Utc
    var currentUtc = D(2025, 1, 2);
    Assert.Throws<ArgumentException>(() => WeekNumberCalculator.Calculate(startLocal, currentUtc, 4));
  }
  #endregion
}
