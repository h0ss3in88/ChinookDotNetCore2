using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Chinook.Model;
using Chinook.Model.Data;
using Chinook.Model.Queries;
using Xunit;

namespace Chinook.Test
{
    public class MovieQueries
    {
        private readonly IRunner _runner;
        private readonly FilmQuery _query;
        public MovieQueries()
        {
            _runner = new Runner("server=localhost;username=Hussein;password=123456;database=chinook;");
            _query = new FilmQuery(_runner);

        }
        [Fact]
        public void GetAllFilm()
        {
            var result = _query.GetAll();
            Assert.Equal(result.Count(),1000);
        }

        [Fact]
        public void GetFilmById()
        {
            var result = _query.GetById(2);
            Assert.Equal(result != null,true);
            Assert.Equal(result.FilmId,2);            
        }

        [Fact]
        public void FilmCount()
        {
            var count = _query.GetActorsCount(1);
            Assert.Equal(10,count);
        }

        [Fact]
        public void GetFilmAndActors()
        {
            var film = _query.GetActors(1);
//            Assert.Equal(film.FilmId != null , true );
//            Assert.Equal(film.FilmId,1);
//            Assert.Equal(film.Title,"Academy Dinosaur");
            Assert.NotNull(film);
            Assert.NotNull(film.Actors);
            Assert.Equal(typeof(List<dynamic>),Enumerable.ToList(film.Actors).GetType());
            int count = 0;
            for (var i = 0; i < Enumerable.ToList(film.Actors).Count; i++ )
            {
                count++;
            }    
            Assert.Equal(10,count);
        }
    }
}