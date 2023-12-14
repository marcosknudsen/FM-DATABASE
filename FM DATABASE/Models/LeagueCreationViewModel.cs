using Microsoft.AspNetCore.Mvc.Rendering;

namespace FM_DATABASE.Models
{
    public class LeagueCreationViewModel:League
    {
        public IEnumerable<SelectListItem>? Ranks {  get; set; }
        public IEnumerable<SelectListItem>? Countries { get; set; }
    }
}
