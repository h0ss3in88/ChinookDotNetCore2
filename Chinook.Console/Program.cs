using System;
using System.Linq;
using Chinook.Model.Data;
using Chinook.Model.Queries;

namespace Chinook.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            var runner = new Runner("server=localhost;username=Hussein;password=123456;database=chinook;");
            var query = new FilmQuery(runner);
            var result = query.GetActors(1);
//            System.Console.WriteLine(result.Title);
            foreach (var actor in Enumerable.ToList<dynamic>(result.Actors))
            {
                System.Console.WriteLine(actor.FirstName);
                System.Console.WriteLine(actor.LastName);
            }
            System.Console.ReadLine();
        }
    }
}