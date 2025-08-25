using System;

namespace RisingSigma.Api.Logic;

/// <summary>
/// Provides strength estimation and training load suggestions using the Brzycki formula.
/// Formula: 1RM = weight * (36 / (37 - reps))  (valid for reps 1..36)
/// Rounding policy (per provided example): Suggested training weights are rounded to the nearest whole kilogram.
/// </summary>
internal static class BrzyckiCalculator
{
  /// <summary>
  /// Estimates one-repetition maximum (1RM) based on lifted weight and repetitions.
  /// </summary>
  /// <param name="weightKg">Lifted weight in kg (must be > 0)</param>
  /// <param name="reps">Completed repetitions (1..36)</param>
  public static double EstimateOneRepMax(double weightKg, int reps)
  {
    if (weightKg <= 0) throw new ArgumentException("Weight must be greater than 0", nameof(weightKg));
    if (reps <= 0 || reps >= 37) throw new ArgumentException("Reps must be between 1 and 36 for Brzycki formula", nameof(reps));
    return weightKg * (36.0 / (37 - reps));
  }

  /// <summary>
  /// Rounds a raw suggested training weight to the nearest whole kilogram.
  /// </summary>
  private static double RoundToNearestKg(double weight) => Math.Round(weight, MidpointRounding.AwayFromZero);

  /// <summary>
  /// Suggests a training weight for next week given a recent performance and desired intensity percent of estimated 1RM.
  /// </summary>
  /// <param name="performedWeightKg">Weight lifted</param>
  /// <param name="performedReps">Reps performed</param>
  /// <param name="targetPercentOfOneRm">Target intensity as decimal (e.g. 0.8 for 80%)</param>
  public static double SuggestTrainingWeight(double performedWeightKg, int performedReps, double targetPercentOfOneRm)
  {
    if (targetPercentOfOneRm <= 0 || targetPercentOfOneRm > 1) throw new ArgumentException("Target percent must be (0,1]", nameof(targetPercentOfOneRm));
    var est = EstimateOneRepMax(performedWeightKg, performedReps);
    var raw = est * targetPercentOfOneRm;
    return RoundToNearestKg(raw);
  }

  /// <summary>
  /// Returns an array of suggested training weights for multiple percentages.
  /// </summary>
  public static double[] SuggestTrainingWeights(double performedWeightKg, int performedReps, params double[] targetPercents)
  {
    if (targetPercents == null || targetPercents.Length == 0) return Array.Empty<double>();
    var results = new double[targetPercents.Length];
    var est = EstimateOneRepMax(performedWeightKg, performedReps);
    for (int i = 0; i < targetPercents.Length; i++)
    {
      var p = targetPercents[i];
      if (p <= 0 || p > 1) throw new ArgumentException($"Target percent at index {i} must be in (0,1]", nameof(targetPercents));
      results[i] = RoundToNearestKg(est * p);
    }
    return results;
  }
}
