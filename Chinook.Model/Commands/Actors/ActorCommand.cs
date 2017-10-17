using System;
using System.Collections.Generic;
using System.Linq;
using Chinook.Model.Data;
using Npgsql;

namespace Chinook.Model.Commands.Actors
{
    public class ActorCommand
    {
        private readonly IRunner _runner;

        public ActorCommand(IRunner runner)
        {
            _runner = runner;
        }

        public int AddActor(Actor actor)
        {
            var args = new object[] {actor.FirstName, actor.LastName};
            var insertSql = "insert into actor(first_name,last_name,last_update) values(@0,@1,now())";
            var cmd = _runner.BuildCommand(insertSql, args);
            return _runner.Transact(cmd).First();
        }

        public int BulkSave(IEnumerable<Actor> actors)
        {
            var sql = "insert into actor(first_name,last_name,last_update) values(@0,@1,@2);";
            var commands = new List<NpgsqlCommand>();
            foreach (var actor in actors)
            {
                commands.Add(_runner.BuildCommand(sql,actor.FirstName,actor.LastName,DateTime.Today));
            }
            var result = _runner.Transact(commands.ToArray());
            return result.Sum();
        }
        public int UpdateActor(Actor actor)
        {
            var args = new object[] {actor.FirstName, actor.LastName, actor.ModifiedAt, actor.Id};
            var sql = "update actor set first_name =@0, last_name =@1 ,last_update=@2 where actor.actor_id = @3";
            var cmd = _runner.BuildCommand(sql,args);
            return _runner.Transact(cmd).First();
        }

        public int Remove(int actorId)
        {
            var args = new object[] {actorId};
            var sql = "delete from actor where actor_id=@0;";
            var result =  _runner.Transact(new NpgsqlCommand[] { _runner.BuildCommand(sql, args) } );
            return result.Sum();
        }
    }
}