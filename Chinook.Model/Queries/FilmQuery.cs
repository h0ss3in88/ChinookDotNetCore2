using System;
using System.Collections.Generic;
using System.Linq;
using Chinook.Model.Data;

namespace Chinook.Model.Queries
{
    public class FilmQuery
    {
        private readonly IRunner _runner;

        public FilmQuery(IRunner runner)
        {
            _runner = runner;
        }

        public IEnumerable<dynamic> GetAll()
        {
            var sql = "select film_id as Id,title as Title,description as Description,release_year as ReleaseYear,length,replacement_cost as cost,last_update as ModifiedAt as Length from film;";
            return _runner.ExecuteDynamic(sql, null);
        }

        public IEnumerable<Film> GetAllMovies()
        {
            var sql = "select film_id as Id,title as Title,description as Description,release_year as ReleaseYear,length,replacement_cost as cost,last_update as ModifiedAt , length as Length from film;";
            return _runner.Execute<Film>(sql, null);
        }

        public dynamic GetById(int id)
        {
            var sql = "select * from film where film_id = @0;";
            return _runner.ExecuteToSingleDynamic(sql, id);
        }

        public IEnumerable<Category> GetAllCategories()
        {
            var sql =
                "select category_id as Id,name as Name,last_update as ModifiedAt from category order by category_id;";
            var result = _runner.Execute<Category>(sql, null);
            return result;
        }
        public Film FilmCategories(int filmId)
        {
            var args = new object[] {filmId};
            var filmSql = "select film.film_id as Id,title as Title,description as Description from film where film_id=@0;";
            var categorySql =
                "select film_category.category_id as Id ,category.name as Name from FROM category INNER JOIN film_category ON film_category.category_id = category.category_id WHERE film_category.film_id = @0;";
            var result = _runner.ExecuteToSingle<Film>(filmSql + categorySql, args);
            return result;
        }
        public int GetActorsCount(int filmId)
        {
            var count = 0;
            var sql =
                "SELECT (SELECT * FROM (SELECT count(1) FROM film_actor WHERE film_actor.film_id = film.film_id)x)"
                + "FROM film WHERE film_id = @0;";
            using (var reader = _runner.OpenReader(sql, filmId))
            {
                if (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
            }
            return count;
        }
        public Film GetMoviesDetails(int filmId)
        {
            Film result = new Film();
            var args = new Object[] {filmId};
            var filmSql = @"SELECT film.film_id as film_id,film.title as title ,film.description as Description,length ,release_year as ReleaseYear,replacement_cost::money as Cost FROM film WHERE film_id =@0;";
            var actorSql =
                @"SELECT actor.actor_id as Id ,actor.first_name as FirstName,actor.last_name as LastName FROM actor INNER JOIN film_actor ON actor.actor_id = film_actor.actor_id WHERE film_id = @0;";
            var categorySql =
                @"select film_category.category_id as Id ,category.name as Name , category.last_update as ModifiedAt  FROM category INNER JOIN film_category ON film_category.category_id = category.category_id WHERE film_category.film_id = @0;";
            using (var reader = _runner.OpenReader(filmSql + actorSql + categorySql, args))
            {
                while (reader.Read())
                {
                    result = reader.ToSingle<Film>();
                    reader.NextResult();
                    result.Actors = reader.ToList<Actor>();
                    reader.NextResult();
                    result.Category = reader.ToList<Category>().FirstOrDefault();
                }
            }
            return result;
        }
        public dynamic GetActors(int filmId)
        {
            dynamic result = null;
            var args = new Object[] {filmId};
            var filmSql = @"SELECT film.film_id as film_id,film.title as title ,film.description  FROM film WHERE film_id =@0;";
            var actorSql =
                @"SELECT actor.first_name,actor.last_name FROM actor INNER JOIN film_actor ON actor.actor_id = film_actor.actor_id WHERE film_id = @0;";
            using (var reader = _runner.OpenReader(filmSql+actorSql,args))
            {
                while(reader.Read())
                {
//                    result = reader.ToExpando();
//                    (result as IDictionary<string,object>)["Film"] = reader.ToExpando();
                    reader.NextResult();
                    result = reader.DynamicList();
//                    (result as IDictionary<string,object>)["Actors"] = new List<dynamic>();
//                    (result as IDictionary<string,object>)["Actors"] = reader.DynamicList().ToList<dynamic>();
                }
            }
            return result;
        }
    }
}