using CourseStripe.Data.InMemory;
using System.Security.Claims;

namespace CourseStripe.UseCases.ShoppingCart
{
	public class GetShoppingCart(IInMemoryShoppingCart inMemoryShoppingCart,
		IHttpContextAccessor httpContextAccessor)
	{
		public ShoppingCartDto Execute()
			=> inMemoryShoppingCart
			.Get(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
	}
}
