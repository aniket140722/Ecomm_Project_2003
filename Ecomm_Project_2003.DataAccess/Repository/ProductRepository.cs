using Ecomm_Project_2003.DataAccess.Data;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Ecomm_Project_2003.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository
{
    public class ProductRepository:Repository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context) : base(context) 
        { this.context = context; 
        }
        
            
        
    }
}
