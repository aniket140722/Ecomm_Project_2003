using Dapper;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;

namespace Ecomm_Project_2003.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork UnitOfWork)
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
            return Json(new { data = _UnitOfWork.SP__CALL.List<CoverType>(SD.SP_GetCoverTypes) });  // Store Procedure Code.
           // return Json(new { data = _UnitOfWork.CoverType.GetAll() }); normal wprk ke liya.
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var CoverTypeInDb = _UnitOfWork.CoverType.Get(id);
           DynamicParameters parameters= new DynamicParameters();    // Store Procedure Code.
            parameters.Add("id", id);
            var CoverTypeInDb=_UnitOfWork.SP__CALL.OneRecord<CoverType>(SD.SP_GetCoverType,parameters);    
           
            if (CoverTypeInDb == null)
                return Json(new { success = true, message = "Something Went Wrong While Delete Data!!!" });
           // _UnitOfWork.CoverType.Remove(CoverTypeInDb);
           // _UnitOfWork.Save();
          _UnitOfWork.SP__CALL.Execute(SD.SP_DeleteCoverType ,parameters); // Store Procedure Delete Code.
            return Json(new
            {
                success = true, // POP UP Message Ke Liya. // Side me Green. Jo Aata ha
                message = " Record Was Deleted Successfully. "
            });

        }
        #endregion
        public IActionResult Upsert(int? id)  // ? Null Allow
        {
            // Create
            CoverType coverType = new CoverType();
            if (id == null) return View(coverType);
            // Edit
          //coverType = _UnitOfWork.CoverType.Get(id.GetValueOrDefault());

          DynamicParameters parameters= new DynamicParameters(); // Store Procedure Code 
            parameters.Add("id", id.GetValueOrDefault());
            coverType=_UnitOfWork.SP__CALL.OneRecord<CoverType>(SD.SP_GetCoverType,parameters);  
            if (coverType == null) return NotFound();
            return View(coverType);
        }
        // Save.
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (coverType == null) return BadRequest();
            if (!ModelState.IsValid) return View(coverType);
            DynamicParameters parameters= new DynamicParameters();  
            parameters.Add("name", coverType.Name); 
            if (coverType.Id == 0)
                //_UnitOfWork.CoverType.Add(coverType);
                _UnitOfWork.SP__CALL.Execute(SD.SP_CreateCoverType, parameters); // SAVE for Store Procedure.
            else
            //   _UnitOfWork.CoverType.Update(coverType);
            // _UnitOfWork.Save();
            {
                parameters.Add("id", coverType.Id);
                _UnitOfWork.SP__CALL.Execute(SD.SP_UpdateCoverType, parameters); 
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
                
    
