using System;
using System.Collections.Generic;

namespace Chinook.Model
{
    public class Actor
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime ModifiedAt { get; set; }
        public IEnumerable<Film> Films { get; set; }
    }
}