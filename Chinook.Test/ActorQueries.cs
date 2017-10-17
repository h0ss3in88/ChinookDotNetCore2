using System.Globalization;
using System.Linq;
using Chinook.Model.Data;
using Chinook.Model.Queries;
using Xunit;

namespace Chinook.Test
{
    public class ActorQueries
    {
        private readonly IRunner _runner;

        public ActorQueries()
        {
            _runner = new Runner("server=localhost;username=Hussein;database=chinook;password=123456;");
        }

        [Fact]
        public void GetAllActors()
        {
            var query = new ActorQuery(_runner);
            var result = query.GetAll();
            Assert.Equal(result.Count(),203);
        }

        [Fact]
        public void GetActorById()
        {
            var query = new ActorQuery(_runner);
            var result = query.GetById(12);
            Assert.Equal(result != null,true);
            Assert.Equal(result.FirstName == "Karl" && result.LastName == "Berry",true);
        }

        [Fact]
        public void GetActorByIdWithDynamic()
        {
            var query = new ActorQuery(_runner);
            var result = query.GetOneDynamic(2);
            Assert.Equal(result != null ,true);
            Assert.Equal(result.FirstName,"Nick");
            
        }
        [Fact]
        public void SnakeToTitle()
        {
            var snake = "this_is_text";
            TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
            var replaced = snake.Replace("_", " ");
            var titled = textInfo.ToTitleCase(replaced).Replace(" ", string.Empty);
            Assert.Equal(titled,"ThisIsText");

        }
    }
}