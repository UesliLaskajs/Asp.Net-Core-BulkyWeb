using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Bulky.Utilities;
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
        [BindProperty]
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
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.TotalPrice = GetPricing(cart);
                ShoppingCartVM.OrderHeader.OrderTotal = (cart.TotalPrice * cart.Count);
            }

            return View(ShoppingCartVM);
        }


        public IActionResult Summary()
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            if (ShoppingCartVM.OrderHeader.ApplicationUser != null)
            {
                ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
                ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
                ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
                ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
                ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
                ShoppingCartVM.OrderHeader.StreetAdress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAdress;

            }
            else
            {
                return NotFound();
            }

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.TotalPrice = GetPricing(cart);
                ShoppingCartVM.OrderHeader.OrderTotal = (cart.TotalPrice * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]

        public IActionResult SummaryPost()
        {
            var userClaims = (ClaimsIdentity)User.Identity;
            var userId = userClaims.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;
           ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.OrderTotal = 0; // Initialize

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.TotalPrice = GetPricing(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.TotalPrice * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDeleayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var items in ShoppingCartVM.ShoppingCartList)
            {
               OrderDetails orderDetails = new()
                {
                    Count = items.Count,
                    ProductId = items.ProductId,
                    orderHeader = ShoppingCartVM.OrderHeader, // Fixed case
                    Price = items.TotalPrice
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();
            }

            return RedirectToAction(nameof(OrderConfirmation), new {id=ShoppingCartVM.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
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

        

    }
}
