using System;
using System.Collections.Generic;
using Chinook.Model.Data;

namespace Chinook.Model.Queries
{
    public class ActorQuery
    {
        private readonly IRunner _runner;
        public ActorQuery(IRunner runner)
        {
            _runner = runner;
        }

        public IEnumerable<Actor> GetAll()
        {
            var sql = "select actor_id as Id ,first_name as FirstName,last_name as LastName,last_update as ModifiedAt from actor order by actor_id;";
            return _runner.Execute<Actor>(sql, null);
        }

        public Actor GetById(int id)
        {
            var args = new Object[] {id};
            var sql =
                "select actor_id as Id ,first_name as FirstName,last_name as LastName,last_update as LastUpdate from actor where actor_id = @0;";
            return _runner.ExecuteToSingle<Actor>(sql,args);
        }

        public dynamic GetOneDynamic(int id)
        {
            dynamic result = null;
            var args = new object[] { id };
            var sql =
                "select * from actor where actor_id=@0;";
            using (var reader = _runner.OpenReader(sql,args))
            {
                if (reader.Read())
                {
                    result = reader.ToExpando();
                }
            }
            return result;
        }

        public Actor GetActorsMovies(int actorId)
        {
            Actor actor = new Actor();
            var actorSql = @"select actor_id as Id, first_name as FirstName , last_name as LastName from actor where actor_id = @0;";
            var filmsSql =
                @"select film.film_id as Id , film.title as title , film.description as Description , film.length as Length  from film inner join film_actor on film.film_id = film_actor.film_id where film_actor.actor_id = @0 order by film.film_id;";
            using (var reader= _runner.OpenReader(actorSql+filmsSql,new object[] { actorId }))
            {
                while (reader.Read())
                {
                    actor = reader.ToSingle<Actor>();
                    reader.NextResult();
                    actor.Films = reader.ToList<Film>();
                }
            }
            return actor;
        }
    }    
}