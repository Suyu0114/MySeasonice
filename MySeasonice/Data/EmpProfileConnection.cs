using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySeasonice.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MySeasonice.Data
{
    public interface EmpProfileConnectionFactory 
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
    public class EmpProfileConnection: EmpProfileConnectionFactory
    {
        private readonly string _connectionString;

        public EmpProfileConnection(string connectString) => _connectionString = connectString ??
            " Data Source=192.168.2.61;User ID=sa;Password=DSC@dsc;Database=EmpServer;MultipleActiveResultSets=true ";
            //throw new ArgumentNullException(nameof(connectString));

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var sqlConnection = new SqlConnection(_connectionString);
            await sqlConnection.OpenAsync();
            return sqlConnection;
            //throw new NotImplementedException();
        }

        //public DbSet<MySeasonice.Model.EmpProfile> EmpProfile { get; set; }

    }
}
