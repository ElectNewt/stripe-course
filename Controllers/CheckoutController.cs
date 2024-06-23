using CourseStripe.Data.InMemory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace CourseStripe.Controllers
{
	[Route("[controller]")]
	public class CheckoutController(IInMemoryShoppingCart inMemoryShoppingCart)
		: Controller
	{

		[HttpGet("{stripePriceId}")]
		public async Task<IActionResult> Checkout(string stripePriceId)
		{
			SessionCreateOptions options = new SessionCreateOptions()
			{
				SuccessUrl = "https://localhost:7111/payment-completed",
				LineItems = new List<SessionLineItemOptions>()
				{
					new SessionLineItemOptions()
					{
						Quantity = 1,
						Price = stripePriceId
					}
				},

				CustomerEmail = User?.FindFirstValue(ClaimTypes.Email) ?? null,
				Mode = "payment"
			};

			SessionService sessionService = new SessionService();
			Session session = await sessionService.CreateAsync(options);

			return Redirect(session.Url);
		}

		[HttpGet("")]
		[Authorize]
		public async Task<IActionResult> CheckoutShoppingCart()
		{
			ShoppingCartDto cart = inMemoryShoppingCart.Get(User.FindFirstValue(ClaimTypes.Email));

			SessionCreateOptions options = new SessionCreateOptions()
			{
				SuccessUrl = "https://localhost:7111/payment-completed",
				LineItems = cart.StripePriceIds.Select(p =>
				new SessionLineItemOptions()
				{
					Quantity = 1,
					Price = p
				}).ToList(),
				CustomerEmail = User?.FindFirstValue(ClaimTypes.Email) ?? null,
				Mode = "payment",
				Discounts = cart.StripePriceIds.Count >= 2
					? new List<SessionDiscountOptions>()
					{
						new SessionDiscountOptions()
						{
							Coupon = "47gajTZY"
						}
					} : null

			};

			SessionService sessionService = new SessionService();
			Session session = await sessionService.CreateAsync(options);

			return Redirect(session.Url);
		}


		[HttpGet("subscription")]
		[Authorize]
		public async Task<IActionResult> Subscription()
		{
			SessionCreateOptions options = new SessionCreateOptions()
			{
				SuccessUrl = "https://localhost:7111/payment-completed",
				LineItems = new List<SessionLineItemOptions>()
				{
					new SessionLineItemOptions()
					{
						Quantity = 1,
						Price = "price_1PQlYfL78xk9bMx0XxBZMSQO"
					}
				},

				CustomerEmail = User?.FindFirstValue(ClaimTypes.Email),
				Mode = "subscription",
				Metadata = new Dictionary<string, string>()
				{
					{"userid", User?.FindFirstValue(ClaimTypes.NameIdentifier)!}
				}


			};

			SessionService sessionService = new SessionService();
			Session session = await sessionService.CreateAsync(options);

			return Redirect(session.Url);
		}


	}
}
