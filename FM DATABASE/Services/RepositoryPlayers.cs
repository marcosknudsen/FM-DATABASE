using Dapper;
using FM_DATABASE.Models;
using Microsoft.Data.SqlClient;

namespace FM_DATABASE.Services
{
    public interface IRepositoryPlayers
    {
        Task Create(Player player);
        Task Delete(int id);
        Task Edit(PlayerCreationViewModel player);
        Task<IEnumerable<Player>> GetAll();
        Task<Player> GetById(int id);
    }
    public class RepositoryPlayers:IRepositoryPlayers
    {
        private readonly string connectionString;

        public RepositoryPlayers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Player player)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Player(FirstName,LastName,ClubId) VALUES(@FirstName,@LastName,@ClubId);SELECT SCOPE_IDENTITY()", player);
                player.Id = id;
            }
        }

        public async Task<IEnumerable<Player>> GetAll()
        {
            using(var connection=new SqlConnection(connectionString))
            {
                return await connection.QueryAsync<Player>($@"SELECT Id,FirstName,LastName,ClubId FROM Player");
            }
        }

        public async Task<Player> GetById(int id)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Player>($@"SELECT Id,FirstName,LastName,ClubId FROM Player WHERE Id=@Id",new {id});
            }
        }

        public async Task Edit(PlayerCreationViewModel player)
        {
            using(var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync($@"UPDATE Player SET FirstName=@FirstName,LastName=@LastName,ClubId=@ClubId WHERE Id=@Id",player);
            }
        }

        public async Task Delete(int id)
        {
            using (var connection=new SqlConnection(connectionString))
            {
                await connection.ExecuteAsync(@"DELETE FROM Player WHERE Id=@Id",new {id});
            }
        }
    }
}
