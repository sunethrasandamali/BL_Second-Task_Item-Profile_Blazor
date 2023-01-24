using BlueLotus360.Com.Application.Interfaces.Services;
using System;

namespace BlueLotus360.Com.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}