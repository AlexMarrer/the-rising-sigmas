using Microsoft.EntityFrameworkCore;
using Moq;
using RisingSigma.Api.DTOs;
using RisingSigma.Api.Logic;
using RisingSigma.Database;
using RisingSigma.Database.Entities;

namespace RisingSigma.Api.Test.Logic;

public class ExerciseUpdateVerifier : IExerciseUpdateVerifier
{
  #region Fields & Dependencies
  private readonly ApplicationDbContext _db;
  private readonly ExerciseLogic _logic;
  private readonly Mock<ITimeProvider> _timeMock = new();
  private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _configMock = new();
  #endregion

  #region Public State
  public Guid ExerciseId { get; private set; }
  public Guid TemplateId { get; private set; }
  #endregion

  #region Construction & Seeding
  public ExerciseUpdateVerifier()
  {
    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .EnableSensitiveDataLogging()
      .Options;
    _db = new ApplicationDbContext(options);
    _timeMock.Setup(t => t.UtcNow).Returns(DateTime.UtcNow);
    SeedBaseData();
    _logic = new ExerciseLogic(_db, _configMock.Object, _timeMock.Object);
  }

  private void SeedBaseData()
  {
    var user = new User { Id = Guid.NewGuid(), Role = "TestUser" }; _db.User.Add(user);
    var trainingPlan = new TrainingPlan { Id = Guid.NewGuid(), Name = "TP", Description = "Test", StartTime = DateTime.UtcNow.Date, CycleWeeks = 4, UserId = user.Id }; _db.TrainingPlan.Add(trainingPlan);
    var weekPlan = new WeekPlan { Id = Guid.NewGuid(), TrainingPlanId = trainingPlan.Id, WeekNumber = 1, Version = 1 }; _db.WeekPlan.Add(weekPlan);
    var muscleGroup = new MuscleGroup { Id = Guid.NewGuid(), Name = "Chest" }; _db.MuscleGroup.Add(muscleGroup);
    var template = new ExerciseTemplate { Id = Guid.NewGuid(), Name = "Bench", MuscleGroupId = muscleGroup.Id }; _db.ExerciseTemplate.Add(template);
    TemplateId = template.Id;
    var exercise = new Exercise { Id = Guid.NewGuid(), Day = DayOfWeek.Monday, ExerciseTemplateId = template.Id, Reps = 10, Sets = 3, RPE = 7.5, WeekPlanId = weekPlan.Id, notes = "orig" };
    ExerciseId = exercise.Id;
    _db.Exercise.Add(exercise);
    _db.SaveChanges();
  }
  #endregion

  #region API
  public UpdateExerciseRequestDto MakeRequest(int reps = 12, int sets = 4, double rpe = 8.0, string? notes = "updated", int day = 3, Guid? templateId = null)
    => new() { Reps = reps, Sets = sets, RPE = rpe, Notes = notes ?? string.Empty, Day = day, ExerciseTemplateId = templateId ?? TemplateId };

  public Task<ExerciseDto> UpdateAsync(UpdateExerciseRequestDto request) => UpdateAsync(ExerciseId, request);

  public async Task<ExerciseDto> UpdateAsync(Guid exerciseId, UpdateExerciseRequestDto request)
    => await _logic.UpdateExerciseAsync(exerciseId, request);

  public Exercise GetExerciseEntity() => _db.Exercise.First(e => e.Id == ExerciseId);

  public Guid AddNewTemplate(string name = "New Template", string muscleGroupName = "Chest")
  {
    var mg = _db.MuscleGroup.FirstOrDefault(m => m.Name == muscleGroupName) ?? new MuscleGroup { Id = Guid.NewGuid(), Name = muscleGroupName };
    if (_db.Entry(mg).State == EntityState.Detached)
      _db.MuscleGroup.Add(mg);
    var template = new ExerciseTemplate { Id = Guid.NewGuid(), Name = name, MuscleGroupId = mg.Id };
    _db.ExerciseTemplate.Add(template);
    _db.SaveChanges();
    return template.Id;
  }
  #endregion

  #region Disposal
  public void Dispose() => _db.Dispose();
  #endregion
}
