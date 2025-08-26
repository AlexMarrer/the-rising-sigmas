using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RisingSigma.Database;

namespace RisingSigma.Api.Test.Logic;

/// <summary>
/// Test-DbContext, der Aufrufe von SaveChangesAsync mitzählt.
/// </summary>
public class TestApplicationDbContext : ApplicationDbContext
{
  public int SaveChangesCalls { get; private set; }

  public TestApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {
  }

  public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    SaveChangesCalls++;
    return base.SaveChangesAsync(cancellationToken);
  }
}
