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
    public class CompanyRepository:Repository<Company>,ICompanyRepository
    {
        private readonly ApplicationDbContext _Context;
        public CompanyRepository(ApplicationDbContext context)
            :base(context) 
        {
            _Context = context;
        }   
    }
}
