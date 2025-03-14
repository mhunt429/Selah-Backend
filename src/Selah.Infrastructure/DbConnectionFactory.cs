using System.Threading.Tasks;
using System.Data;
using Npgsql;
namespace Selah.Infrastructure
{
    
    public interface IDbConnectionFactory
    {
        public Task<IDbConnection> CreateConnectionAsync();
    }
    
    public class SelahDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SelahDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}



