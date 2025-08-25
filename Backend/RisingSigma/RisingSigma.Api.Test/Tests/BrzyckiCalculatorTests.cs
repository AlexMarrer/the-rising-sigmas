using RisingSigma.Api.Logic;

namespace RisingSigma.Api.Test.Tests;

/// <summary>
/// TDD Start: Tests for BrzyckiCalculator (intentionally created BEFORE wider integration usage).
/// Includes one deliberate failing expectation (marked with TODO) to simulate TDD red phase / defect capture.
/// </summary>
public class BrzyckiCalculatorTests
{
  #region EstimateOneRepMax
  [Fact]
  public void EstimateOneRepMax_Example_ShouldMatchReference()
  {
    // Example: 100kg x 5 reps -> 112.5
    var oneRm = BrzyckiCalculator.EstimateOneRepMax(100, 5);
    Assert.Equal(112.5, Math.Round(oneRm, 1));
  }

  [Theory]
  [InlineData(50, 1)] // 1RM should be ~50
  [InlineData(80, 3)]
  [InlineData(120, 8)]
  public void EstimateOneRepMax_BasicRanges_NoThrow(double weight, int reps)
  {
    var est = BrzyckiCalculator.EstimateOneRepMax(weight, reps);
    Assert.True(est >= weight); // 1RM always >= performed weight
  }

  [Theory]
  [InlineData(0, 5)]
  [InlineData(-10, 5)]
  public void EstimateOneRepMax_InvalidWeight_ShouldThrow(double weight, int reps)
  {
    Assert.Throws<ArgumentException>(() => BrzyckiCalculator.EstimateOneRepMax(weight, reps));
  }

  [Theory]
  [InlineData(100, 0)]
  [InlineData(100, -1)]
  [InlineData(100, 37)]
  [InlineData(100, 100)]
  public void EstimateOneRepMax_InvalidReps_ShouldThrow(double weight, int reps)
  {
    Assert.Throws<ArgumentException>(() => BrzyckiCalculator.EstimateOneRepMax(weight, reps));
  }
  #endregion

  #region SuggestSingle
  [Fact]
  public void SuggestTrainingWeight_80Percent_RoundsAsExpected()
  {
    // From example: 1RM ≈112.5 -> 80% = 90 -> should round to 90
    var w = BrzyckiCalculator.SuggestTrainingWeight(100, 5, 0.8);
    Assert.Equal(90, w);
  }

  [Fact]
  public void SuggestTrainingWeight_70Percent_RoundsAsExpected()
  {
    var w = BrzyckiCalculator.SuggestTrainingWeight(100, 5, 0.7);
    Assert.Equal(79, w); // 112.5 * 0.7 = 78.75 -> rounds to 79
  }

  [Fact]
  public void SuggestTrainingWeight_InvalidPercent_ShouldThrow()
  {
    Assert.Throws<ArgumentException>(() => BrzyckiCalculator.SuggestTrainingWeight(100, 5, 0));
    Assert.Throws<ArgumentException>(() => BrzyckiCalculator.SuggestTrainingWeight(100, 5, 1.5));
  }
  #endregion

  #region SuggestMultiple
  [Fact]
  public void SuggestTrainingWeights_MultiplePercents_ReturnsArray()
  {
    var arr = BrzyckiCalculator.SuggestTrainingWeights(100, 5, 0.8, 0.7, 0.6);
    Assert.Equal(new[] { 90d, 79d, 68d }, arr); // 112.5 * 0.6 = 67.5 -> rounds away from zero -> 68
  }

  [Fact]
  public void SuggestTrainingWeights_EmptyPercents_ReturnsEmpty()
  {
    var arr = BrzyckiCalculator.SuggestTrainingWeights(100, 5);
    Assert.Empty(arr);
  }

  [Fact]
  public void SuggestTrainingWeights_InvalidPercent_ShouldThrow()
  {
    Assert.Throws<ArgumentException>(() => BrzyckiCalculator.SuggestTrainingWeights(100, 5, 0.8, 0));
  }
  #endregion
}
