using Ecomm_Project_2003.DataAccess.Data;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using Ecomm_Project_2003.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            CoverType= new CoverTypeRepository(_context);
            SP__CALL= new SP__CALL(_context);
            Product = new ProductRepository(_context);
            Company = new CompanyRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);   
            ShoopingCart = new ShoopingCartRepository(_context);
            OrderHeader= new OrderHeaderRepository(_context);
            orderDetail= new OrderDetailRepository(_context);
            address = new AddAddress(_context);


        }

        public ICategoryRepository Category { private set; get; }         //    Private set is a Read Property.

        public ICoverTypeRepository CoverType { private set; get; }    //    Private set is a Read Property .
        public ISP__CALL SP__CALL { private set; get; }
        public IProductRepository Product { private set; get; }
        public ICompanyRepository Company { private set; get; }
        public IApplicationUserRepository ApplicationUser { set; get; }
        public IShoopingCartRepository ShoopingCart { set; get; }   

        public IOrderDetailRepository orderDetail { set; get; } 
        public IOrderHeaderRepository OrderHeader { set; get; } 
        public IAddressRepository address { set; get; }   
      //  IApplicationUserRepository IUnitOfWork.AppllicationUser => throw new NotImplementedException();

        public void Save()
        {
            _context.SaveChanges(); 
        }
    }
}
       