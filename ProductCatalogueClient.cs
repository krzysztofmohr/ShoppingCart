using ShoppingCart.Data;

namespace ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Net.Http;
    using Newtonsoft.Json;

    public class ProductCatalogueClient : IProductCatalogueClient
    {
        private static string productCatalogueBaseUrl =
          @"http://private-05cc8-chapter2productcataloguemicroservice.apiary-mock.com";
        private static string getProductPathTemplate =
          "/products?productIds=[{0}]";
        public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds)
        {
            return await GetItemsFromCatalogueService(productCatalogueIds).ConfigureAwait(false);
        }
        private async Task<IEnumerable<ShoppingCartItem>> GetItemsFromCatalogueService(int[] productCatalogueIds)
        {
            var response = await
              RequestProductFromProductCatalogue(productCatalogueIds).ConfigureAwait(false);
            return await ConvertToShoppingCartItems(response).ConfigureAwait(false);
        }

        private static async Task<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
        {
            var productsResource = string.Format(
              getProductPathTemplate, string.Join(",", productCatalogueIds));
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(productCatalogueBaseUrl);
                return await httpClient.GetAsync(productsResource).ConfigureAwait(false);
            }
        }

        private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var products =
              JsonConvert.DeserializeObject<List<ProductCatalogueProduct>>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            return
              products
                .Select(p => new ShoppingCartItem(
                  int.Parse(p.ProductId),
                  p.ProductName,
                  p.ProductDescription,
                  p.Price
              ));
        }

        private class ProductCatalogueProduct
        {
            public string ProductId { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public Money Price { get; set; }
        }
    }

    public class Money
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}