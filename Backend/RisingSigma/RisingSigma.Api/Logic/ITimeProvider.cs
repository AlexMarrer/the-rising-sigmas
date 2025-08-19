namespace RisingSigma.Api.Logic;

/// <summary>
/// Interface for providing current time functionality.
/// This allows for easier testing by mocking time-related operations.
/// </summary>
public interface ITimeProvider
{
    DateTime UtcNow { get; }
}
