using Dapper;
using FM_DATABASE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace FM_DATABASE.Services
{
    public interface IRepositoryLeagues
    {
        Task Create(LeagueCreationViewModel league);
        Task Delete(int id);
        Task Edit(LeagueCreationViewModel league);
        Task<IEnumerable<LeagueCreationViewModel>> GetAll();
        Task<League> GetById(int id);
    }
    public class RepositoryLeagues:IRepositoryLeagues
    {
        private readonly string connectionString;

        public RepositoryLeagues(IConfiguration configuration)
        {
            connectionString=configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(LeagueCreationViewModel league)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                league.Id = await connection.QuerySingleAsync<int>($@"INSERT INTO League(Name,Code,Rank,CountryId) VALUES(@Name,@Code,@Rank,@CountryId);SELECT SCOPE_IDENTITY();", league);
            }
        }

        public async Task Edit(LeagueCreationViewModel league)
        {
            using(var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($@"UPDATE League SET Name=@Name, Code=@Code, Rank=@Rank, CountryId=@CountryId",league);
            }
        }

        public async Task Delete(int id)
        {
            using(var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($@"DELETE FROM League WHERE Id=@Id",new {id});
            }
        }

        public async Task<IEnumerable<LeagueCreationViewModel>> GetAll()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<LeagueCreationViewModel>($@"SELECT Id,Name,Code,CountryId,Rank FROM League");
            }
        }

        public async Task<League> GetById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<League>(
                @"SELECT Id, Name,Code,CountryId,Rank FROM League
                WHERE Id=@Id", new { id });
        }
    }
}
