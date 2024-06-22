using System.Diagnostics;
using AshokPMS.web.Models;
using AshokPMS.web.Services.Irepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace AshokPMS.web.Controllers
{
    

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICrudServices<Product> _product;
        private readonly ICrudServices<Category> _categoryInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger,
            ICrudServices<Product> product,
            ICrudServices<Category> categoryInfo,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _product = product;
            _categoryInfo = categoryInfo;
            _userManager = userManager;
        }
        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<Product> productList = _product.GetAll();
                return View(productList);
            }
            else
            {
                // User is not authenticated, show a photo-only view
                return View("PhotoOnly");
            }
        }


        [Authorize(Roles = "ADMIN, PUBLIC")]

        public IActionResult Details(int ProductId)
		{
			Product productdet = _product.Get(p => p.Id== ProductId);
			return View(productdet);
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
