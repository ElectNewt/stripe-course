using CourseStripe.Data;
using CourseStripe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseStripe.UseCases.User
{


	public class SetPremium(ApplicationDbContext applicationDbContext)
	{
		public async Task Execute(string userId, string subscriptionId)
		{
			if (applicationDbContext.UserClaims.Any(a => a.UserId == userId
			&& a.ClaimType == UserConstants.PREMIUM_CLAIM))
				return;

			applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>()
			{
				UserId = userId,
				ClaimType = UserConstants.PREMIUM_CLAIM,
				ClaimValue = "enabled"
			});

			UserSubscriptionEntity? subscriptionEntity = await applicationDbContext.userSubscriptions
				.FirstOrDefaultAsync(a => a.SubscriptionId == subscriptionId && a.UserId == userId);

			if (subscriptionEntity is not null)
			{
				subscriptionEntity.IsActive = true;
				applicationDbContext.userSubscriptions.Update(subscriptionEntity);
			}else
			{
				await applicationDbContext.userSubscriptions
					.AddAsync(new UserSubscriptionEntity()
					{
						IsActive = true,
						SubscriptionId = subscriptionId,
						UserId = userId,
						ValidUntil = null
					});
			}



			await applicationDbContext.SaveChangesAsync();
		}
	}
}
