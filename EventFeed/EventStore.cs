using System.Collections.Generic;

namespace ShoppingCart.EventFeed
{
    class EventStore : IEventStore
    {
        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            throw new System.NotImplementedException();
        }

        public void Raise(string eventName, object content)
        {
            throw new System.NotImplementedException();
        }
    }
}