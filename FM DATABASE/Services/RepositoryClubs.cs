using Dapper;
using FM_DATABASE.Models;
using Microsoft.Data.SqlClient;

namespace FM_DATABASE.Services
{
    public interface IRepositoryClubs
    {
        Task Create(Club club);
    }
    public class RepositoryClubs:IRepositoryClubs
    {
        private readonly string connectionString;
        public RepositoryClubs(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Club club)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Club(Name) VALUES(@Name);SELECT SCOPE_IDENTITY()",club);
                club.Id = id;
            }
        }
    }
}
