using System;
using System.Diagnostics;
using System.Linq;
using Chinook.Model;
using Chinook.Model.Data;
using Chinook.Model.Queries;
using Xunit;

namespace Chinook.Test
{
    public class ArtistQueries
    {
        private readonly IRunner _runner;

        public ArtistQueries()
        {
            _runner = new Runner("server=localhost;username=Hussein;database=chinook;password=123456;");
        }
        [Fact]
        public void GetAll()
        {
            var result = _runner.Execute<Artist>("select artist_id as Id,name as Name from artist;", null);
            Assert.Equal(275,result.Count());
        }

        [Fact]
        public void GetArtistById()
        {         
            var result =
                _runner.ExecuteToSingle<Artist>("select artist_id as Id , name as Name from artist where artist_id = @0",new object[] { 12 });
            Assert.Equal(result == null,false);
            Assert.Equal(result.Name == "Black Sabbath",true);
        }

        [Fact]
        public void GetArtistAlbums()
        {
            var query = new ArtistQuery(_runner);
            var result =  query.GetByAlbums(2);
            Assert.NotNull(result);
            Assert.Equal(result.Name,"Accept");
            Assert.Equal(result.Albums.Count(),2);
            Assert.NotEmpty(result.Albums);
            Assert.Equal(result.Albums.First().Title,"Balls to the Wall");
        }
    }
}
