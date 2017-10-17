using System.Collections;
using System.Collections.Generic;

namespace Chinook.Model
{
    public class Artist
    {
        public Artist(){ }
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Album> Albums { get; set; }
    }
}