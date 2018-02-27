using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using ShoppingCart.EventFeed;

namespace ShoppingCart
{
    public class EventsFeedModule : NancyModule
    {
        public EventsFeedModule(IEventStore eventStore) : base("events")
        {
            Get("/", parameters =>
            {
                if (!ulong.TryParse(this.Request.Query.start, out ulong start))
                    start = 0;
                if (!ulong.TryParse(this.Request.Query.end, out ulong end))
                    end = ulong.MaxValue;

                return eventStore.GetEvents(start, end);
            });
        }        
    }
}
