using CourseStripe.Data;

namespace CourseStripe.UseCases.Products
{
    public class GetProducts(ApplicationDbContext applicationDbContext)
    {
        public async Task<List<ProductDto>> Execute()
            => applicationDbContext.Products
            .Select(a => new ProductDto(a.Id, a.Name, a.ImageUrl, 
                a.Price, a.StripePriceId))
            .ToList();

    }
}


public record ProductDto(int Id, string Name, string ImageUrl, decimal Price, string StripePriceId);