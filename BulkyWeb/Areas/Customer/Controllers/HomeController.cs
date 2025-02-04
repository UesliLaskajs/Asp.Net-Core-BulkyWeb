using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties: "category").ToList();
            return View(products);
        }
        
        public IActionResult Details(int ProductId)
        {

            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == ProductId, includeProperties: "category"),
                Count = 1,
                ProductId = ProductId
            };
            
          
            return View(cart);
        }

        [HttpPost]
        [Authorize]

        public IActionResult Details(ShoppingCart shoppCart)
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppCart.Id);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppCart.Count;
                _unitOfWork.ShoppingCart.Update(shoppCart);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppCart);
            }

            _unitOfWork.ShoppingCart.Add(shoppCart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
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
