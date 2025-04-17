using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_Project_2003.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public  CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API's
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Company.GetAll() });
        }
        [HttpDelete] 
        public IActionResult Delete(int id)
        {
            var CompanyInDb= _unitOfWork.Company.Get(id);
            if (CompanyInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Delete Data." });
            _unitOfWork.Company.Remove(CompanyInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Your Data Is Deleted Successfully" });
            
        }
        #endregion
        public IActionResult Upsert(int? id) 
        {
           Company company=new Company();
            if(id ==null) return View(company);
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null)return NotFound();  
            return View(company);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (company == null)return BadRequest();    
            if (!ModelState.IsValid)return View(company); 
            if (company.Id ==0)
                _unitOfWork.Company.Add(company);   
            else
                _unitOfWork.Company.Update(company);    
            _unitOfWork.Save();
            return RedirectToActionPermanent("Index");
        }

    }
}
