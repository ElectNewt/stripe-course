using CourseStripe.Data;
using Stripe;

namespace CourseStripe.UseCases.Products
{
    public class AddProduct(ApplicationDbContext _dbContext)
    {
        public async Task<bool> Execute(CreateProductRequest request)
        {
            //TODO: validate name, url, price

            string priceId = await CreateStripePriceId(request);


			_dbContext.Products.Add(new Data.Entities.ProductEntity()
            {
                ImageUrl = request.ImageUrl,
                Name = request.Name,
                Price = request.Price,
                StripePriceId = priceId
			});

            return await _dbContext.SaveChangesAsync() > 0;
        }


		private async Task<string> CreateStripePriceId(CreateProductRequest createProductRequest)
		{
			var options = new ProductCreateOptions()
			{
				Name = createProductRequest.Name,
				Images = new List<string>
			{
				createProductRequest.ImageUrl
			}
			};

			var productService = new ProductService();
			Product product = await productService.CreateAsync(options);

			var priceOptions = new PriceCreateOptions()
			{
				Active = true,
				Currency = "eur",
				UnitAmount = Convert.ToInt64(createProductRequest.Price * 100),
				Product = product.Id,
			};

			var priceService = new PriceService();
			var price = await priceService.CreateAsync(priceOptions);
			return price.Id;
		}

	}

   

}

public record CreateProductRequest(string Name, string ImageUrl, decimal Price);