using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        ISP__CALL SP__CALL { get; } 
        IProductRepository Product { get; }
        ICompanyRepository Company { get; } 
        IApplicationUserRepository ApplicationUser { get; }  
        IShoopingCartRepository ShoopingCart { get; }   
        IOrderHeaderRepository OrderHeader { get; } 
        IOrderDetailRepository orderDetail { get; } 
        IAddressRepository address { get; }
        void Save();
    }
}

