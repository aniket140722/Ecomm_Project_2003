using Dapper;
using Ecomm_Project_2003.DataAccess.Data;
using Ecomm_Project_2003.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm_Project_2003.DataAccess.Repository
{
    public class SP__CALL : ISP__CALL
    {
         
        private readonly ApplicationDbContext _context;
        private static string ConnectionString = "";
        public SP__CALL(ApplicationDbContext context)
        {
            _context = context;
            ConnectionString = _context.Database.GetDbConnection().
          ConnectionString;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(procedureName, param, commandType:CommandType.StoredProcedure);

            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                return sqlConnection.Query<T>(procedureName, param,
                    commandType: CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                var result = sqlConnection.QueryMultiple(procedureName, param,
                    commandType: CommandType.StoredProcedure);
                var iteam1 = result.Read<T1>();
                var iteam2 = result.Read<T2>();
                if (iteam1 != null && iteam2 != null)
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(iteam1, iteam2);
                return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
                { }

            }
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                var value = sqlCon.Query<T>(procedureName, param,
                commandType: CommandType.StoredProcedure);
                return value.FirstOrDefault();

            }
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();
                return sqlConnection.ExecuteScalar<T>(procedureName, param,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
