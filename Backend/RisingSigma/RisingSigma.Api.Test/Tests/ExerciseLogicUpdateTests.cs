using RisingSigma.Api.DTOs;
using RisingSigma.Api.Test.Logic;

namespace RisingSigma.Api.Test.Tests;

/// <summary>
/// White-Box Tests for ExerciseLogic.UpdateExerciseAsync using extracted verification logic (abstraction pattern aligned with TimeFreeze tests)
/// Cyclomatic complexity covered: 12 (all branches) via composed scenarios.
/// </summary>
public class ExerciseLogicUpdateTests : IDisposable
{
  #region Setup
  private readonly IExerciseUpdateVerifier _verificationLogic = new ExerciseUpdateVerifier();

  private UpdateExerciseRequestDto MakeRequest(int reps = 12, int sets = 4, double rpe = 8.0, string? notes = "updated", int day = 3)
      => _verificationLogic.MakeRequest(reps, sets, rpe, notes, day);
  #endregion

  #region Happy Path
  [Fact]
  public async Task UpdateExercise_ValidData_ShouldPersistChanges()
  {
    var req = MakeRequest(reps: 15, sets: 5, rpe: 9.0, notes: "Strength block", day: 2);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(15, dto.Reps);
    Assert.Equal(5, dto.Sets);
    Assert.Equal(9.0, dto.RPE);
    Assert.Equal("Strength block", dto.Notes);
    Assert.Equal(2, dto.Day); // Tuesday

    var entity = _verificationLogic.GetExerciseEntity();

    Assert.Equal(DayOfWeek.Tuesday, entity.Day);
    Assert.Equal(9.0, entity.RPE);
  }

  #endregion

  #region Validation - RPE
  [Theory]
  [InlineData(-0.1)]
  [InlineData(-1)]
  [InlineData(10.1)]
  [InlineData(50)]
  public async Task UpdateExercise_InvalidRPE_ShouldThrow(double badRpe)
  {
    var req = MakeRequest(rpe: badRpe);
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _verificationLogic.UpdateAsync(req));

    Assert.Contains("RPE must be between 0 and 10", ex.Message);
  }

  [Theory]
  [InlineData(0.0)]
  [InlineData(10.0)]
  public async Task UpdateExercise_RPEBoundaries_ShouldSucceed(double boundaryRpe)
  {
    var req = MakeRequest(rpe: boundaryRpe);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(boundaryRpe, dto.RPE);
  }
  #endregion

  #region Day Mapping
  // DTO mapping: Sunday => 7, Monday =>1 ... Saturday =>6
  [Theory]
  [InlineData(0, DayOfWeek.Sunday, 7)]
  [InlineData(1, DayOfWeek.Monday, 1)]
  [InlineData(2, DayOfWeek.Tuesday, 2)]
  [InlineData(3, DayOfWeek.Wednesday, 3)]
  [InlineData(4, DayOfWeek.Thursday, 4)]
  [InlineData(5, DayOfWeek.Friday, 5)]
  [InlineData(6, DayOfWeek.Saturday, 6)]
  public async Task UpdateExercise_AllValidDayMappings_ShouldMap(int inputDay, DayOfWeek expectedEnum, int expectedDtoDay)
  {
    var req = MakeRequest(day: inputDay);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(expectedDtoDay, dto.Day);

    var entity = _verificationLogic.GetExerciseEntity();
    Assert.Equal(expectedEnum, entity.Day);
  }

  [Theory]
  [InlineData(-1)]
  [InlineData(7)]
  [InlineData(10)]
  public async Task UpdateExercise_InvalidDay_ShouldDefaultMonday(int invalidDay)
  {
    var req = MakeRequest(day: invalidDay);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(1, dto.Day); // Monday
    Assert.Equal(DayOfWeek.Monday, _verificationLogic.GetExerciseEntity().Day);
  }

  [Theory]
  [InlineData(-1000)]
  [InlineData(9999)]
  public async Task UpdateExercise_InvalidDay_ExtremeValues_ShouldDefaultMonday(int extremeDay)
  {
    var req = MakeRequest(day: extremeDay);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(1, dto.Day);
    Assert.Equal(DayOfWeek.Monday, _verificationLogic.GetExerciseEntity().Day);
  }
  #endregion

  #region Entity Existence & Sequencing
  [Fact]
  public async Task UpdateExercise_NotFound_ShouldThrow()
  {
    var req = MakeRequest();
    var missingId = Guid.NewGuid();
    var ex = await Assert.ThrowsAsync<ArgumentException>(() => _verificationLogic.UpdateAsync(missingId, req));

    Assert.Contains("not found", ex.Message);
  }

  [Fact]
  public async Task UpdateExercise_SequentialUpdates_LastWins()
  {
    var first = MakeRequest(reps: 8, sets: 3, rpe: 6.5, notes: "Deload", day: 1);
    await _verificationLogic.UpdateAsync(first);
    var second = MakeRequest(reps: 20, sets: 6, rpe: 9.5, notes: "Peak", day: 5);
    var dto = await _verificationLogic.UpdateAsync(second);

    Assert.Equal(20, dto.Reps);
    Assert.Equal(6, dto.Sets);
    Assert.Equal(9.5, dto.RPE);
    Assert.Equal(5, dto.Day);
  }
  #endregion

  #region Template & Notes
  [Fact]
  public async Task UpdateExercise_ChangeTemplate_ShouldPersistNewTemplateId()
  {
    var newTemplateId = _verificationLogic.AddNewTemplate("Incline Bench");
    var req = MakeRequest();
    req.ExerciseTemplateId = newTemplateId;
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.Equal(newTemplateId, dto.ExerciseTemplateId);
    Assert.Equal(newTemplateId, _verificationLogic.GetExerciseEntity().ExerciseTemplateId);
  }

  [Fact]
  public async Task UpdateExercise_NullNotes_ShouldPersistAsEmptyString()
  {
    var req = MakeRequest(notes: null!);
    var dto = await _verificationLogic.UpdateAsync(req);

    Assert.NotNull(dto.Notes);
    Assert.True(dto.Notes == string.Empty || dto.Notes == "");
  }
  #endregion

  public void Dispose() => _verificationLogic.Dispose();
}
