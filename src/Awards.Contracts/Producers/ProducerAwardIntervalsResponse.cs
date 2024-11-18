using System.Collections.Generic;

namespace Awards.Contracts.Producers
{
    public sealed class ProducerAwardIntervalsResponse
    {
        public List<AwardIntervalResponse> Min { get; set; } = new List<AwardIntervalResponse>();
        public List<AwardIntervalResponse> Max { get; set; } = new List<AwardIntervalResponse>();
    }
}
