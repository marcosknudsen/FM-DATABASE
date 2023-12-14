namespace FM_DATABASE.Models
{
    public class League
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int Rank { get; set; }

        public int CountryId { get; set; }
    }
}
