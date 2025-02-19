using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
     
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVm { get; set; }
    
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int orderId)
        {
            orderVm = new()
            {
                orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.OrderDetails.GetAll(u => u.orderHeader.Id == orderId, includeProperties: "Product")
            };
            return View(orderVm);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_User_Admin + "," + SD.Role_User_Employee)]

        public IActionResult UpdateOrderDetails()
        {
            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == orderVm.orderHeader.Id);

            orderHeaderFromDb.Name = orderVm.orderHeader.Name;
            orderHeaderFromDb.PhoneNumber = orderVm.orderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAdress = orderVm.orderHeader.StreetAdress;
            orderHeaderFromDb.City = orderVm.orderHeader.City;
            orderHeaderFromDb.PostalCode = orderVm.orderHeader.PostalCode;

            if (!string.IsNullOrEmpty(orderVm.orderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = orderVm.orderHeader.TrackingNumber;
            }
            if (!string.IsNullOrEmpty(orderVm.orderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = orderVm.orderHeader.Carrier;
            }
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Succesfully";

            return RedirectToAction(nameof(Details),new  { orderId=orderHeaderFromDb.Id});
        }



        [HttpGet]
       public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_User_Admin)|| User.IsInRole(SD.Role_User_Employee))
            {
                orderHeaders= _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderHeaders=_unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId==userId,includeProperties:"ApplicationUser");
            }


            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDeleayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatisShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                deafult:
                    break;
            }


            return Json(new { data = orderHeaders });
        }
        

    }
}
