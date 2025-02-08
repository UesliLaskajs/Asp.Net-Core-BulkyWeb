using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product")

            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.TotalPrice = GetPricing(cart);
                ShoppingCartVM.TotalOrder = (cart.TotalPrice * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public double GetPricing(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price; // Regular price for up to 50 items
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50; // Price for 51 to 100 items
            }
            else
            {
                return shoppingCart.Product.Price100; // Price for more than 100 items
            }
        }



        public IActionResult Plus(int CartId)
        {
            var shoppingCartIncrement = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId);
            shoppingCartIncrement.Count += 1;

            _unitOfWork.ShoppingCart.Update(shoppingCartIncrement);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int CartId)
        {
            var shoppingCartDecrement = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId);
            if (shoppingCartDecrement.Count > 0)
            {
                shoppingCartDecrement.Count -= 1;
                _unitOfWork.ShoppingCart.Update(shoppingCartDecrement);

            }
            else
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCartDecrement);
            }


            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int CartId)
        {
            var cartToBeRemoved = _unitOfWork.ShoppingCart.Get(u => u.Id == CartId);
            _unitOfWork.ShoppingCart.Remove(cartToBeRemoved);

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            return View();
        }


    }
}
