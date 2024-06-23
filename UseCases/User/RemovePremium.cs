using CourseStripe.Data;
using CourseStripe.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace CourseStripe.UseCases.User
{
	public class RemovePremium(ApplicationDbContext applicationDbContext)
    {
        public async Task Execute(string subcriptionId)
        {
            UserSubscriptionEntity? subscritpion = applicationDbContext.userSubscriptions
                .FirstOrDefault(a => a.SubscriptionId == subcriptionId);

            if (subscritpion is null)
                return;

            subscritpion.IsActive = false;
            applicationDbContext.userSubscriptions.Update(subscritpion);


            IdentityUserClaim<string>? claim = applicationDbContext.UserClaims
                .FirstOrDefault(a => a.UserId == subscritpion.UserId
				&& a.ClaimType == UserConstants.PREMIUM_CLAIM);

            if (claim is not null)
                applicationDbContext.UserClaims.Remove(claim);

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
