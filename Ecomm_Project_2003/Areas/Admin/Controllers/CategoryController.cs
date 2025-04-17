using Dapper;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project_2003.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
       public IActionResult GetAll()
        { 
           return Json(new {data= _UnitOfWork .SP__CALL.List<Category>(SD2.SP__GetCategories) });  // Store Procedure Code .
        
          //var categoryList=_UnitOfWork.Category.GetAll(); // GetAll Covertype or Category dono me chlta ha
          //  return Json(new {data=categoryList});   // APIs data also receive as a Json.
        }
        [HttpDelete]
        public IActionResult Delete (int id)
            {
          //  var CategoryInDb = _UnitOfWork.Category.Get(id);
         DynamicParameters parameters = new DynamicParameters();    // Store Procedure Code. 
            parameters.Add("id", id);  
            var CategoryInDb=_UnitOfWork.SP__CALL.OneRecord<Category>(SD2.SP__GetCategory,parameters);   
            if (CategoryInDb == null)
                return Json(new { success = true, message = "Something Went Wrong While Delete Data!!!" });
            //_UnitOfWork.Category.Remove(CategoryInDb);
            _UnitOfWork.SP__CALL.Execute(SD2.SP__DeleteCategory, parameters);
            _UnitOfWork.Save();
            return Json(new { success = true, // POP UP Message Ke Liya. // Side me Green. Jo Aata ha
                message = " Record Was Deleted Successfully. " });

        }
        #endregion
        public IActionResult Upsert(int? id)  // ? Null Allow
        { 
            // Create
            Category category = new Category();
            if (id == null) return View(category);
            // Edit
           category=_UnitOfWork.Category.Get(id.GetValueOrDefault());
           DynamicParameters parameters= new DynamicParameters();    // Store Procedure Code.
            parameters.Add("id", id.GetValueOrDefault());
            category = _UnitOfWork.SP__CALL.OneRecord<Category>(SD2.SP__GetCategory, parameters);    // Store Procedure Code.

            if (category == null) return NotFound();
            return View(category);
        }
        // Save .
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult Upsert(Category category) 
        {
            if (category == null) return NotFound();
            if (!ModelState.IsValid) return View(category);
            DynamicParameters parameters= new DynamicParameters();  // Store Proceure Code.
            parameters.Add("name", category.Name);
            if (category.Id == 0)
                // _UnitOfWork.Category.Add(category); 
                _UnitOfWork.SP__CALL.Execute(SD2.SP__CreatetCategory, parameters);
            else
            //  _UnitOfWork.Category.Update(category);
            // _UnitOfWork.Save();
            {
                parameters.Add("id", category.Id);  
                _UnitOfWork.SP__CALL.Execute(SD2.SP__UpdateCategory, parameters);   
            }
            return RedirectToAction("Index");
                    { 

            }
        }
    }
}
