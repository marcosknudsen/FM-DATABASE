using Microsoft.AspNetCore.Mvc.Rendering;

namespace FM_DATABASE.Models
{
    public class ClubCreationViewModel:Club
    {
        public IEnumerable<SelectListItem>? Leagues { get; set; }
    }
}
