using ShoppingCart.EventFeed;

namespace ShoppingCart.Data
{
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart
    {
        private readonly HashSet<ShoppingCartItem> _items = new HashSet<ShoppingCartItem>();

        public int UserId { get; }
        public IEnumerable<ShoppingCartItem> Items => _items;

        public ShoppingCart(int userId)
        {
            this.UserId = userId;
        }

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems, IEventStore eventStore)
        {
            foreach (var item in shoppingCartItems)
                if (this._items.Add(item))
                    eventStore.Raise("ShoppingCartItemAdded", new { UserId, item });
        }

        public void RemoveItems(int[] productCatalogueIds, IEventStore eventStore)
        {
            var count = _items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogueId));
            if(count > 0) 
                eventStore.Raise("ShoppingCartItemRemoved", new {UserId, productCatalogueIds});
        }
    }
}
