using CourseStripe.UseCases.User;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace CourseStripe.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebhookController(IConfiguration configuration, SetPremium setPremium, 
		RemovePremium removePremium, SetPremiumEnd setPremiumEnd) 
		: ControllerBase
	{

		[HttpPost]
		public async Task<IActionResult> Post()
		{
			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
			try
			{
				Event? stripeEvent = EventUtility.ConstructEvent(json,
						Request.Headers["Stripe-Signature"], configuration["StripeWebhookSecret"]);

				// Handle the event
				switch (stripeEvent.Type)
				{
					case Events.CheckoutSessionCompleted:
						await HandleSeessionCompleted(stripeEvent.Data.Object as Session);
						break;
					case Events.CustomerSubscriptionUpdated:
						await HandleSubscriptionUpdated(stripeEvent.Data.Object as Subscription);
						break;
						case Events.CustomerSubscriptionDeleted:
						await HandleSubscriptionDeleted(stripeEvent.Data.Object as Subscription);
						break;
					default:
						Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
						break;
				}
				return Ok();
			}
			catch (StripeException e)
			{
				return BadRequest();
			}
		}

		private async Task HandleSubscriptionUpdated(Subscription subscription)
		{
			if(subscription.CancelAtPeriodEnd && subscription.CancelAt is not null)
			{
				await setPremiumEnd.Execute(subscription.Id, (DateTime)subscription.CancelAt);
			}
		}


		private async Task HandleSubscriptionDeleted(Subscription subscription)
		{
			await removePremium.Execute(subscription.Id);
		}

		private async Task HandleSeessionCompleted(Session session)
		{
			Console.WriteLine("EVENTO");

			Console.WriteLine($"EMAIL: {session.CustomerDetails.Email}");

			var options = new SessionGetOptions();
			options.AddExpand("line_items");
			var service = new SessionService();

			Session sessionWtihLineItems = await service.GetAsync(session.Id, options);
			foreach(var item in sessionWtihLineItems.LineItems) 
			{
				Console.WriteLine($"PriceId {item.Price.Id} - QTY: {item.Quantity}");

				if (item.Price.Id == "price_1PQlYfL78xk9bMx0XxBZMSQO")
				{
					if(session.Metadata.TryGetValue("userid", out string userId))
					{
						Console.WriteLine("HERE IT GOES A SUBSCRIPTION");
						await setPremium.Execute(userId, session.SubscriptionId);
					}

				}
				else
				{
					Console.WriteLine("HERE IT SHOULD SEND THE BOOK BY EMAIL");
				}


			}


			
		}
	}
}
