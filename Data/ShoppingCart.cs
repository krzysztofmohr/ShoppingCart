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

        public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems)
        {
            foreach (var item in shoppingCartItems)
                this._items.Add(item);
        }

        public void RemoveItems(int[] productCatalogueIds)
        {
            _items.RemoveWhere(i => productCatalogueIds.Contains(i.ProductCatalogueId));
        }
    }
}
