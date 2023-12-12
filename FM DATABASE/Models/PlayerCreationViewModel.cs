using Microsoft.AspNetCore.Mvc.Rendering;

namespace FM_DATABASE.Models
{
    public class PlayerCreationViewModel:Player
    {
        public IEnumerable<SelectListItem>? Clubs { get; set; }
    }
}
