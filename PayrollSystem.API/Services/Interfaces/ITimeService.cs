namespace PayrollSystem.API.Services.Interfaces
{
    public interface ITimeService
    {
        DateTime UtcNow { get; }

        DateTime LocalNow { get; }
    }
}
