using Tasks1and2_SimpleApiWithDI.Interfaces;

namespace Tasks1and2_SimpleApiWithDI.Services
{
    public class TimeService : ITimeService
    {
        public DateTime GetTimeUtc3()
        {
            return DateTime.UtcNow.ToLocalTime();
        }
    }
}
