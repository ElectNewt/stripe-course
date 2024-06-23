using CourseStripe.Data;
using CourseStripe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStripe.UseCases.User
{
	public class SetPremiumEnd(ApplicationDbContext applicationDbContext)
	{

		public async Task Execute(string subscriptionId, DateTime subscriptionEndDate)
		{
			UserSubscriptionEntity subscription = await applicationDbContext.userSubscriptions
				.SingleAsync(a => a.SubscriptionId == subscriptionId);

			subscription.ValidUntil = subscriptionEndDate;
			applicationDbContext.userSubscriptions.Update(subscription);

			await applicationDbContext.SaveChangesAsync();
		}

	}
}
