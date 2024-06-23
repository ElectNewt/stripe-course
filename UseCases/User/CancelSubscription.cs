using CourseStripe.Data;
using CourseStripe.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace CourseStripe.UseCases.User
{
	public class CancelSubscription(ApplicationDbContext applicationDbContext)
	{

		public async Task Execute(string userId)
		{
			UserSubscriptionEntity subscription = await applicationDbContext.userSubscriptions
				.SingleAsync(a => a.UserId == userId && a.IsActive == true);

			var options = new SubscriptionUpdateOptions()
			{
				CancelAtPeriodEnd = true
			};

			var service = new SubscriptionService();
			await service.UpdateAsync(subscription.SubscriptionId, options);
		}
	}
}
