using Dapper;
using FM_DATABASE.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace FM_DATABASE.Services
{
    public interface IRepositoryClubs
    {
        Task Create(Club club);
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetById(int id);
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

        public async Task<IEnumerable<Club>> GetAll()
        {
            using (var connection=new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<Club>($@"SELECT Id,Name FROM Club");
            }
        }

        public async Task<Club> GetById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Club>(
                @"SELECT Id, Name FROM Club
                WHERE Id=@Id", new { id });
        }
    }
}
