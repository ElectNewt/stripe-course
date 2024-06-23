using CourseStripe.UseCases.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseStripe.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AddProduct addProduct) : ControllerBase
    {

        [HttpPost]
        public async Task<bool> InsertProduct(CreateProductRequest request)
            => await addProduct.Execute(request);
    }
}
