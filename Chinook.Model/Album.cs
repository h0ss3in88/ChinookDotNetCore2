using System.Collections.Generic;

namespace Chinook.Model
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IList<Track> Tracks { get; set; }
    }
}