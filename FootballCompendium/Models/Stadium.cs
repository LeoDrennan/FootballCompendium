namespace FootballCompendium.Models
{
    public class Stadium
    {
        public int Id { get; set; }
        public string stadium_name { get; set; }
        public string stadium_location { get; set; }
        public int capacity { get; set; }
        public string description { get; set; }

        public Stadium()
        {
            
        }
    }
}
