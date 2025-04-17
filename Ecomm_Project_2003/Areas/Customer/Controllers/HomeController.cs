using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;

using Ecomm_Project_2003.Models.ViewModles;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Ecomm_Project_2003.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
           
        {
            _logger = logger;
            _unitOfWork= unitOfWork;    
        }

        public IActionResult Index()
        { 
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var claim=claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);  
            if (claim!=null)
            {
                var count = _unitOfWork.ShoopingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);    
            }
            var productList=_unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");   
            return View(productList);
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
        public IActionResult Details (int id )
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoopingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            } 
            //****

            var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.ID == id, 
                includeProperties: "Category,CoverType");
            if (productInDb == null) return NotFound();
            var shoopingCart = new ShoopingCart()
            {
                Product = productInDb,
               ProductId = productInDb.ID,
            };
            return View(shoopingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Add to Login
        public IActionResult Details(ShoopingCart shoopingCart)
        {
            shoopingCart.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claims == null) return NotFound();
                shoopingCart.ApplicationUserId = claims.Value;

                var shoopingCartInDb = _unitOfWork.ShoopingCart.FirstOrDefault
                    (sc=>sc.ApplicationUserId==claims.Value&&sc.ProductId ==shoopingCart.ProductId);

                if (shoopingCartInDb == null)
                    _unitOfWork.ShoopingCart.Add(shoopingCart);
                else
                    shoopingCartInDb.Count += shoopingCart.Count;
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));


            }

            //Not Valid
            else
            {
                var productInDb = _unitOfWork.Product.FirstOrDefault(p => p.ID == shoopingCart.ProductId,    
                includeProperties:"Category,CoverType");
                if (productInDb == null)return NotFound();
                var shoopingCartEdit = new ShoopingCart()
                {
                    Product = productInDb,
                    ProductId = productInDb.ID
                };
                return View(shoopingCartEdit);
            }

        }
    }
}