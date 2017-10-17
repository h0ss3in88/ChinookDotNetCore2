namespace Chinook.Model
{
    public class Track
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Composer { get; set; }
        public decimal Price { get; set; }
        public decimal Duration { get; set; }
        public decimal Bytes { get; set; }
    }
}