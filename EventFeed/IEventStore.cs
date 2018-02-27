using System.Collections.Generic;

namespace ShoppingCart.EventFeed
{
  public interface IEventStore
  {
    IEnumerable<Event> GetEvents(ulong firstEventSequenceNumber, ulong lastEventSequenceNumber);
    void Raise(string eventName, object content);
  }
}