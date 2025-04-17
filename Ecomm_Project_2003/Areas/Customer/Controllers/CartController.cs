 using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Models.ViewModels;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stripe;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace Ecomm_Project_2003.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private static bool isEmailConfirm=false; // conform email than passes for ctor
        private readonly IEmailSender _emailSender; // 
        private readonly UserManager<IdentityUser> _userManager;//
        
        public CartController(IUnitOfWork unitOfWork,IEmailSender emailSender,UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork; 
            _emailSender = emailSender;
            _userManager = userManager;
        }
        [BindProperty]
        public ShoopingCartVM ShoopingCartVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var claims= claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims==null)
            {
                ShoopingCartVM = new ShoopingCartVM()
                {
                    ListCart=new List<ShoopingCart>()

                };
                return View(ShoopingCartVM);
               
            }
            ShoopingCartVM = new ShoopingCartVM()
            {
                ListCart = _unitOfWork.ShoopingCart.GetAll(sc => sc.ApplicationUserId == claims.Value, includeProperties: "Product"),
                OrderHeader = new OrderHeader()
            };
            ShoopingCartVM.OrderHeader.OrderTotal = 0;
            ShoopingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.
                FirstOrDefault(au => au.Id == claims.Value);
            foreach(var list in ShoopingCartVM.ListCart)
            {
                list.Price=SD.GetPriceBasedOnQuantity(list.Count,list.Product.Price,list.Product.Price50,list.Product.Price100);
                ShoopingCartVM.OrderHeader.OrderTotal += (list.Count * list.Price);
                if (list.Product.Description.Length>100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 99) + "....";
                }
            }
            // Email Verification
            if (!isEmailConfirm)
            {
                ViewBag.EmailMessage = "Email must be Confirmed for authorized customer";
                ViewBag.EmailCSS = "text-danger";
                isEmailConfirm = false;
            }
            else
            {
                ViewBag.EmailMessage = "Email has been sent Please verify your email";
                ViewBag.EmailCSS = "text-success";
               
            }
             return View(ShoopingCartVM);
        }
        // this Code for a confirmation email.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public async Task <IActionResult> IndexPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = _unitOfWork.ApplicationUser.FirstOrDefault(au=>au.Id== claims.Value);   
            if (user != null)
            {
                ModelState.AddModelError(string.Empty, "Email Empty");
            }
            else
            //Email 
            {

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



            }


            return RedirectToAction(nameof  (Index));    
        }
        public IActionResult plus (int id)
        {
            var cart = _unitOfWork.ShoopingCart.Get(id);
            if (cart == null)return NotFound();
            cart.Count += 1;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int id)
        {
            var cart = _unitOfWork.ShoopingCart.Get(id);
            if (cart == null) return NotFound();
           if (cart.Count== 1)
            {
                cart.Count =1;
            }
            else
            {
                cart.Count -= 1;
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult delete (int id)
        {
            var cart = _unitOfWork.ShoopingCart.Get(id);
            if (cart == null) return NotFound();    
            _unitOfWork.ShoopingCart.Remove(cart);
            _unitOfWork.Save();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();
            var count = _unitOfWork.ShoopingCart.GetAll
                (sc => sc.ApplicationUserId == claims.Value).ToList().Count;
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, cart.Count);
            return RedirectToAction(nameof(Index));       
            
        } 
        // Mix CheckBox, Description , Add , minus
        public IActionResult summary(string checkBoxInput)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);   
            var checkedBoxes= checkBoxInput.Split(',',StringSplitOptions.RemoveEmptyEntries);
           // var userAddresses = _unitOfWork.address.GetAll(a => a. == claims.Value).ToList();



            //_unitOfWork.OrderHeader.Add(ShoopingCartVM.OrderHeader);
            //_unitOfWork.Save();

            //  if (claims == null) return NotFound();
            ShoopingCartVM = new ShoopingCartVM()
            {
                ListCart = _unitOfWork.ShoopingCart.GetAll
                    (sc => sc.ApplicationUserId == claims.Value && checkedBoxes.Contains(sc.Id.ToString()),
                    includeProperties: "Product"),
                    OrderHeader = new OrderHeader()
            };
            ShoopingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au=>au.Id==claims.Value); 

            foreach (var list in ShoopingCartVM.ListCart) 
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count,list.Product.Price,
                    list.Product.Price50,list.Product.Price100);
                ShoopingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                if (list.Product.Description.Length>100)
                {
                    list.Product.Description = list.Product.Description.Substring(0, 100) + ".......";
                }

            }
            ShoopingCartVM.OrderHeader.Name= ShoopingCartVM.OrderHeader.ApplicationUser.Name;
            ShoopingCartVM.OrderHeader.StreetAddress = ShoopingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoopingCartVM.OrderHeader.State = ShoopingCartVM.OrderHeader.ApplicationUser.State;
            ShoopingCartVM.OrderHeader.City = ShoopingCartVM.OrderHeader.ApplicationUser.City;
            ShoopingCartVM.OrderHeader.PhoneNumber = ShoopingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoopingCartVM.OrderHeader.PostalCode = ShoopingCartVM.OrderHeader.ApplicationUser.PostalCode;
            return View(ShoopingCartVM);
                
            

        }
        // Place Order
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("summary")]
        public IActionResult summarypost(string stripeToken, string checkBoxInput)// stripeToken Add Was Payment Gateway.
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claims == null) return NotFound();
            ShoopingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.FirstOrDefault(au => au.Id == claims.Value);
            ShoopingCartVM.ListCart = _unitOfWork.ShoopingCart.GetAll(sc => sc.ApplicationUserId == claims.Value, includeProperties: "Product");
            ShoopingCartVM.OrderHeader.OrderStatus = SD.OrderStatusPending;
            ShoopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            ShoopingCartVM.OrderHeader.OrderDate = DateTime.Now;
            _unitOfWork.OrderHeader.Add(ShoopingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var list in ShoopingCartVM.ListCart)
            {
                list.Price = SD.GetPriceBasedOnQuantity(list.Count, list.Product.Price, list.Product.Price50, list.Product.Price100);
                OrderDetail orderDetail = new OrderDetail()
                {
                    ProductId = list.ProductId,
                    OrderHeaderId = ShoopingCartVM.OrderHeader.Id,
                    Price = list.Price,
                    Count = list.Count,
                };
                ShoopingCartVM.OrderHeader.OrderTotal += (list.Price * list.Count);
                _unitOfWork.orderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            _unitOfWork.ShoopingCart.RemoveRange(ShoopingCartVM.ListCart);
            _unitOfWork.Save();
            // Session Count
            HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, 0);
            // Stripe Payment.
            if (stripeToken == null)
            {
                ShoopingCartVM.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                ShoopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayPayment;
                ShoopingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;

            }
            else
            {
                var options = new ChargeCreateOptions()
                {
                    Amount = Convert.ToInt32(ShoopingCartVM.OrderHeader.OrderTotal),
                    Currency = "usd",
                    Description = "Order Id:" + ShoopingCartVM.OrderHeader.Id.ToString(),
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.BalanceTransactionId == null)
                    ShoopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                else
                    ShoopingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                if (charge.Status.ToLower() == "succeeded")
                {
                    ShoopingCartVM.OrderHeader.OrderStatus = SD.OrderStatusApproved;
                    ShoopingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                    ShoopingCartVM.OrderHeader.OrderDate = DateTime.Now;

                }
                _unitOfWork.Save(); // This Is Payment Code.

            }

            //*****************
            return RedirectToAction("OrderConfirmation", "Cart", new { id = ShoopingCartVM.OrderHeader.Id });
        }
        public IActionResult OrderConfirmation(int id)
        {
            return View(id);

        }


    }

}


