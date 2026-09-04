using PayrollSystem.API.Services.Interfaces;

namespace PayrollSystem.API.Services
{
    public class TimeService : ITimeService
    {
        private readonly TimeZoneInfo _timeZone;

        public TimeService()
        {
            _timeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
        }

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime LocalNow => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);
    }
}
