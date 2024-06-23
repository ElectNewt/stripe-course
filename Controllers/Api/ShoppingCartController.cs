using CourseStripe.UseCases.ShoppingCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseStripe.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class ShoppingCartController(AddToShoppingCart addToShoppingCart) 
		: ControllerBase
	{

		[HttpPost]
		public async Task<IActionResult> Post(AddItemRequest request)
		{
			if (addToShoppingCart.Execute(request.StripePriceId))
			{
				return Ok();
			}

			return UnprocessableEntity();
		}

		public record AddItemRequest(string StripePriceId);

	}
}
