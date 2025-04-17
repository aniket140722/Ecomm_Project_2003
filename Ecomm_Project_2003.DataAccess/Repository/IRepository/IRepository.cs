using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;              //its InterfaceRepository,Member declaration etc.

using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository.IRepository
{
public interface IRepository<T> where T : class
    {
        void Add(T entity);         //  Save
        void Update(T entity);       //  Update
        void Remove(T entity);        //  Delete 
        void Remove(int id);           //  Delete ka alag code.
        void RemoveRange(IEnumerable<T> entities);    // Record all Data Delete 
        T Get(int id); //It is a find Code.
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,// null ka matlav hota ha ki entry na kran to kuch bhi na aaya
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null  // Category,CoverType Table.

            );
        T FirstOrDefault(
             Expression<Func<T, bool>> filter = null, //Dublicate Entry ni hogi ka code jase E-mail,Phone-no., etc.
              string includeProperties = null
            );


    }
}
