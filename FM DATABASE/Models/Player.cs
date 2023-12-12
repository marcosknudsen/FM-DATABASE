using System.ComponentModel.DataAnnotations;

namespace FM_DATABASE.Models
{
    public class Player
    {
        public int Id { get; set; }
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        public int ClubId { get; set; }
    }
}
