using System.Collections.Concurrent;

namespace CourseStripe.Data.InMemory
{

	public interface IInMemoryShoppingCart
	{
		bool Add(string userEmail, string stripePriceId);
		ShoppingCartDto Get(string userEmail);
		void Clean(string userEmail);
	}

	public class InMemoryShoppingCart : IInMemoryShoppingCart
	{
		private readonly ConcurrentDictionary<string, ShoppingCartDto> _carts;


		public InMemoryShoppingCart()
		{
			_carts = new ConcurrentDictionary<string, ShoppingCartDto>(StringComparer.OrdinalIgnoreCase);
		}


		public bool Add(string userEmail, string stripePriceId)
		{
			_carts.AddOrUpdate(userEmail, new ShoppingCartDto()
			{
				StripePriceIds = new List<string>() { stripePriceId }
			},
			(key, existingCart) =>
			{
				existingCart.StripePriceIds.Add(stripePriceId);
				return existingCart;
			}
			);
			return true;
		}

		public ShoppingCartDto Get(string userEmail)
		{

			if(_carts.TryGetValue(userEmail, out var cart)) return cart;

			return new ShoppingCartDto();
		}


		public void Clean(string userEmail)
		{
			_carts.TryRemove(userEmail, out _);
		}

	}


	public class ShoppingCartDto
	{
		public List<string> StripePriceIds { get; set; } = new List<string>();
	}
}
