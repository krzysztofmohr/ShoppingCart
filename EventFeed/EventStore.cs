using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ShoppingCart.EventFeed
{
    class EventStore : IEventStore
    {
        private static long currentSequenceNumber = 0;
        private static readonly IList<Event> _events = new List<Event>();

        public IEnumerable<Event> GetEvents(ulong firstEventSequenceNumber = ulong.MinValue, ulong lastEventSequenceNumber = ulong.MaxValue) =>
            _events.Where(e => e.SequenceNumber >= firstEventSequenceNumber &&
                                e.SequenceNumber <= lastEventSequenceNumber).ToArray();
        

        public void Raise(string eventName, object content)
        {
            var seqNumber = (ulong)Interlocked.Increment(ref currentSequenceNumber);
            _events.Add(new Event(
                seqNumber,
                DateTimeOffset.Now,
                eventName,
                content
            ));
        }
    }
}