using CourseStripe.Models;
using CourseStripe.UseCases.Products;
using CourseStripe.UseCases.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CourseStripe.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly GetProducts _getProducts;
		private readonly CancelSubscription _cancelSubscription;

		public HomeController(ILogger<HomeController> logger, GetProducts getProducts,
			CancelSubscription cancelSubscription)
		{
			_logger = logger;
			_getProducts = getProducts;
			_cancelSubscription = cancelSubscription;
		}

		public async Task<IActionResult> Index()
		{

			List<ProductDto> productDtos = await _getProducts.Execute();
			return View(new IndexViewModel()
			{
				Products = productDtos
			});
		}

		[HttpGet("payment-completed")]
		public IActionResult PaymentCompleted()
		{
			return View();
		}

		[Authorize]
		[HttpGet("cancel-subscription")]
		public async Task<IActionResult> SubscriptionCancelled()
		{
			await _cancelSubscription.Execute(User.FindFirstValue(ClaimTypes.NameIdentifier));
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
