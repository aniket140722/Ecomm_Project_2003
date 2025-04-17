using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository.IRepository
{
    public interface ISP__CALL : IDisposable
    {
        void Execute(string procedureName, DynamicParameters param = null);  // it is passes store procedure name.
        T Single<T>(string procedureName, DynamicParameters param = null); // Find store procedure.
        T OneRecord<T>(string procedureName, DynamicParameters param = null); // Find
        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null); // Multiple query Are used by Tuple.

    }
}
