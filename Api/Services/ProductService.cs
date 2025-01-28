using AutoMapper;
using Dto.Stripe.Product;
using Microsoft.Extensions.Caching.Memory;
using Stripe;
using StripeGateway;

namespace ASG.ApiService.Services
{
    public class ProductService(IStripeProduct stripeProduct, IMemoryCache memoryCache, IMapper mapper)
    {
        public async Task<StripeList<ProductResponse>> GetAllAsync(ProductListOptions options, CancellationToken cancellationToken = default)
        {
            //var key = "products";
            //if (options != null)
            //{
            //    key += $"-{options.Active}-{options.Ids}-{options.StartingAfter}-{options.EndingBefore}-{options.Limit}";
            //}
            //return await memoryCache.GetOrCreateAsync(
            //    key,
            //    entry =>
            //    {
            //        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var listProducts = await stripeProduct.GetAllAsync(options, cancellationToken);
            StripeList<ProductResponse> responses = new StripeList<ProductResponse>()
            {
                HasMore = listProducts.HasMore,
                Object = listProducts.Object,
                Url = listProducts.Url,
            };
            responses.Data = mapper.Map<List<ProductResponse>>(listProducts);
            return responses;
            //}
            //);
        }

        public async Task<ProductResponse> GetOneAsync(string id, CancellationToken cancellationToken = default)
        {
            //return await memoryCache.GetOrCreateAsync(
            //    $"product-{id}",
            //    entry =>
            //    {
            //        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
            var product = await stripeProduct.GetByIdAsync(id, cancellationToken);
            ProductResponse response = mapper.Map<ProductResponse>(product);
            return response;
            //});
        }

        public async Task<StripeList<ProductResponse>> SearchAsync(ProductSearchOptions options, CancellationToken cancellationToken = default)
        {
            var product = await stripeProduct.SearchAsync(options, cancellationToken);
            return mapper.Map<StripeList<ProductResponse>>(product);
        }

        public async Task<ProductResponse> CreateAsync(ProductCreate productCreate, CancellationToken cancellationToken = default)
        {
            var options = mapper.Map<ProductCreate, ProductCreateOptions>(productCreate);
            var product = await stripeProduct.CreateAsync(options, cancellationToken);
            ProductResponse response = mapper.Map<ProductResponse>(product);
            return response;
        }

        public Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            return stripeProduct.DeleteAsync(id, cancellationToken);
        }


        public async Task<ProductResponse> UpdateAsync(string id, ProductUpdate productUpdate, CancellationToken cancellationToken = default)
        {
            var options = new ProductUpdateOptions
            {
                Name = productUpdate.Name,
                Active = productUpdate.Active,
                Description = productUpdate.Description,
                Shippable = productUpdate.Shippable,

                //Metadata = new Dictionary<string, string> { { "order_id", "6735" } },
            };
            var product = await stripeProduct.UpdateAsync(id, options, cancellationToken);
            return mapper.Map<ProductResponse>(product);
        }
    }
}
