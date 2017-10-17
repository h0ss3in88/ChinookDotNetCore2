using System;
using System.Collections.Generic;
using System.Linq;
using Chinook.Model.Data;
using Npgsql;

namespace Chinook.Model.Commands.Artists
{
    public class ArtistCommand
    {
        private readonly IRunner _runner;
        public ArtistCommand(IRunner runner)
        {
            _runner = runner;
        }
        public int AddArtist(Artist artist)
        {
            var args = new object[] { artist.Name };
            var insertSql = "insert into artist(name) values(@0)";
            var cmd = _runner.BuildCommand(insertSql, args);
            return _runner.Transact(cmd).First();
        }

        public int BulkSave(IEnumerable<Artist> artists)
        {
            var sql = "insert into artist(name) values(@0);";
            var commands = new List<NpgsqlCommand>();
            foreach (var artist in artists)
            {
                commands.Add(_runner.BuildCommand(sql,artist.Name));
            }
            var result = _runner.Transact(commands.ToArray());
            return result.Sum();
        }
        public int UpdateArtist(Artist artist)
        {
            var args = new object[] {artist.Name , artist.Id };
            var sql = "update artist set name =@0 where artist.artist_id = @1";
            var cmd = _runner.BuildCommand(sql,args);
            return _runner.Transact(cmd).First();
        }
        public int Remove(int artistId)
        {
            var args = new object[] {artistId};
            var sql = "delete from artist where artist_id=@0;";
            var result =  _runner.Transact(new NpgsqlCommand[] { _runner.BuildCommand(sql, args) } );
            return result.Sum();
        }
    }
}