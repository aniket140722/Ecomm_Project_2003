using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Models.ViewModels;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;

namespace Ecomm_Project_2003.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)] // Jiska Role Admin Ha Wahi Isko Use kae Sakta ha
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Product.GetAll() });

        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productInDb = _unitOfWork.Product.Get(id);
            if (productInDb == null)
                return Json(new { success = false, message = "Unable To Delete Data" });
            // Image Delete Code
            var webRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, productInDb.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            //Delete ApiS
            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data Is Deleted Succesfully" });
        }
        #endregion

        // Upsert 
        public IActionResult Upsert(int? id)
        {
            ProductVM ProductVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(cl => new SelectListItem()
                {
                    Text = cl.Name,
                    Value = cl.Id.ToString()

                }),
            };
            if (id == null) return View(ProductVM);
            ProductVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (ProductVM.Product == null) return NotFound();
            return View(ProductVM);
        }
        // Save.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productvm)

        {
            if (ModelState.IsValid)
            {
                var webrootpath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;          // files upload ke liya
                if (files.Count() > 0)                              // file sellect ki ha 
                {
                    var filename = Guid.NewGuid().ToString();        // File Name
                    var extension = Path.GetExtension(files[0].FileName); // Extenshion
                    var uploads = Path.Combine(webrootpath, @"Image\Products"); //Path
                    if (productvm.Product.ID != 0)     // Null
                    {
                        var imageExists = _unitOfWork.Product.Get(productvm.Product.ID).ImageUrl;
                        productvm.Product.ImageUrl = imageExists;

                    }
                    if (productvm.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webrootpath, productvm.Product.ImageUrl.Trim('\\')); // Delete Image
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStream = new FileStream(Path.Combine(uploads, filename + extension), FileMode.Create)) // Update image.
                    {
                        files[0].CopyTo(filesStream);
                    }
                    productvm.Product.ImageUrl = @"\Image\Products\" + filename + extension;

                }
                else
                {
                    if (productvm.Product.ID != 0)
                    {
                        var imagePath = _unitOfWork.Product.Get(productvm.Product.ID).ImageUrl;
                        productvm.Product.ImageUrl = imagePath;
                    }
                }
                {
                    if (productvm.Product.ID == 0)
                        _unitOfWork.Product.Add(productvm.Product);
                    else
                        _unitOfWork.Product.Update(productvm.Product);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }

            }
            else
            {
                productvm = new ProductVM()
                {

                    Product = new Product(),
                    CategoryList = _unitOfWork.Category.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()
                    }),
                    CoverTypeList = _unitOfWork.CoverType.GetAll().Select(cl => new SelectListItem()
                    {
                        Text = cl.Name,
                        Value = cl.Id.ToString()

                    })
                };
                // Edit Code.
                if (productvm.Product.ID != 0)
                {
                    productvm.Product = _unitOfWork.Product.Get(productvm.Product.ID);
                }
                return View(productvm);
            }

        }
    }
}

