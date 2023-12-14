using AutoMapper;
using FM_DATABASE.Models;

namespace FM_DATABASE.Services
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Player, PlayerCreationViewModel>();
            CreateMap<League, LeagueCreationViewModel>();
        }
    }
}
