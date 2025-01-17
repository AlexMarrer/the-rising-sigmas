using RisingSigma.Database;

namespace RisingSigma.Api.Logic;

public class ExerciseLogic : IExerciseLogic
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly IVerificationLogic _verificationLogic;

    public ExerciseLogic(ApplicationDbContext applicationDbContext, IVerificationLogic verificationLogic, IConfiguration configuration)
    {
        this._applicationDbContext = applicationDbContext;
        this._verificationLogic = verificationLogic;
        this._configuration = configuration;
    }
}
