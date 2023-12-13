using Dapper;
using FM_DATABASE.Models;
using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace FM_DATABASE.Services
{
    public interface IRepositoryCountries
    {
        Task Create(Country country);
        Task Delete(Country country);
        Task Edit(Country country);
        Task<IEnumerable<Country>> GetAll();
        Task<Country> GetById(int id);
    }
    public class RepositoryCountries:IRepositoryCountries
    {
        private readonly string connectionString;

        public RepositoryCountries(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Country country) 
        { 
            using (var connection=new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Country(Code,Name) VALUES(@Code,@Name); SELECT SCOPE_IDENTITY();",country);
                country.Id= id;
            }
        }

        public async Task Edit(Country country)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($@"UPDATE COUNTRY SET Code=@Code, Name=@Name",country);
            }
        }

        public async Task Delete(Country country)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($@"DELETE FROM COUNTRY WHERE Id=@Id", country);
            }
        }

        public async Task<Country> GetById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Country>(@"SELECT Id,Name,Code FROM Country WHERE Id=@Id", new { id });
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<Country>(@"SELECT Id,Name,Code FROM COUNTRY");
            }
        }
    }
}
