using System;
using System.Collections.Generic;
using Chinook.Model.Queries;

namespace Chinook.Model
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ReleaseYear { get; set; }
        public int Length { get; set; }
        public decimal Cost { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IList<Actor> Actors { get; set; }
        public Category Category { get; set; }
        
    }
}