﻿using Nancy;
using Nancy.ModelBinding;
using ShoppingCart.Data;
using ShoppingCart.EventFeed;

namespace ShoppingCart
{
    public class ShoppingCartModule : NancyModule
    {
        public ShoppingCartModule(
            IShoppingCartStore shoppingCartStore,
            IProductCatalogueClient productCatalog,
            IEventStore eventStore
            ) : base("shoppingcart")
        {
            Get("/{userid:int}", parameters =>
            {
                var userId = (int)parameters.userid;
                return shoppingCartStore.Get(userId);
            });

            Post("/{userid:int}/items", async (parameters, _) =>
            {
                var productcatalogIds = this.Bind<int[]>();
                var userId = (int)parameters.userid;

                var shoppingCart = shoppingCartStore.Get(userId);
                var shoppingCartItems = await
                    productCatalog
                        .GetShoppingCartItems(productcatalogIds)
                        .ConfigureAwait(false);
                shoppingCart.AddItems(shoppingCartItems, eventStore);

                shoppingCartStore.Save(shoppingCart);

                return shoppingCart;
            });

            Delete("/{userid:int}/items", parameters =>
            {
                var productcatalogIds = this.Bind<int[]>();
                var userId = (int)parameters.userid;

                var shoppingCart = shoppingCartStore.Get(userId);
                shoppingCart.RemoveItems(productcatalogIds, eventStore);

                shoppingCartStore.Save(shoppingCart);

                return shoppingCart;
            });
        }
    }
}
