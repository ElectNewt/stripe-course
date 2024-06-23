using CourseStripe.UseCases.User;
using System.Security.Claims;

namespace CourseStripe.Extensions
{
	public static class UserExtensions
	{

		public static bool IsPremium(this ClaimsPrincipal claimsPrincipal)
		{
			Claim? result = claimsPrincipal.FindFirst(UserConstants.PREMIUM_CLAIM);
			return result is not null;
		}

		public static bool IsAuthenticated(this ClaimsPrincipal claims)
		{
			return claims.Identity is { IsAuthenticated: true };
		}

	}
}
