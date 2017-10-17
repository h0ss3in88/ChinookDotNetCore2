using System.Collections.Generic;

namespace Chinook.Model
{
    public class PlayList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Track> Tracks { get; set; }
        
    }
}