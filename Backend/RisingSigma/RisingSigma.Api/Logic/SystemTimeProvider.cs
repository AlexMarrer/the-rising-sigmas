namespace RisingSigma.Api.Logic;

/// <summary>
/// Default implementation of ITimeProvider that returns the actual current time.
/// </summary>
public class SystemTimeProvider : ITimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
