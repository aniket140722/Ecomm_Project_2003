using Ecomm_Project_2003.DataAccess.Data;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Ecomm_Project_2003.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _context= context;  
            _unitOfWork= unitOfWork;    
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()  
        {
            var userList=_context.ApplicationUsers.ToList(); //Aspnet User
            var roles=_context.Roles.ToList();  //AspnetRole
            var userRoles=_context.UserRoles.ToList();  //AspNetUserRole
            foreach(var user in userList) 
            {
                var roleId=userRoles.FirstOrDefault(u=>u.UserId==user.Id).RoleId;
                user.Role=roles.FirstOrDefault(r=>r.Id==roleId).Name;   
                if (user.CompanyId==null)
                {
                    user.Company=new Company()
                    {
                        Name=""
                    };
                }
                if (user.CompanyId!=null)
                
                {
                    user.Company = new Company()
                    {
                        Name = _unitOfWork.Company.Get(Convert.ToInt32(user.CompanyId)).Name,
                    };
                }
            }
            //Remove Admin User
            var adminUser=userList.FirstOrDefault(u=>u.Role==SD.Role_Admin);
            if (adminUser!=null)
            {
                userList.Remove(adminUser);
            }
            return Json(new {data=userList});

        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            bool isLocked = false;  
            var UserInDb = _context.ApplicationUsers.FirstOrDefault(u=>u.Id==id);
            if (UserInDb == null)
                return Json(new { success = false, message = "Something Went Wrong While Lock And Unlock User ! ! !" });
              if (UserInDb !=null && UserInDb.LockoutEnd>DateTime.Now)
            {
                UserInDb.LockoutEnd = DateTime.Now; 
                isLocked = false;
            }
            else 
            {
                UserInDb.LockoutEnd = DateTime.Now.AddYears(100);
                isLocked = true;
            }
              _context.SaveChanges();
            return Json(new { success=true,message = isLocked ==true?"User Successfully locked":"User Successfully Unloclked"});
            
                
            
        }
        #endregion
    }
}
