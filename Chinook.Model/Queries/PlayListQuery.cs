using System.Collections.Generic;
using System.Threading.Tasks;
using Chinook.Model.Data;

namespace Chinook.Model.Queries
{
    public class PlayListQuery
    {
        private readonly IRunner _runner;

        public PlayListQuery(IRunner runner)
        {
            _runner = runner;
        }

        public IEnumerable<PlayList> GetAll()
        {
            var palyListSql = @"select playlist_id as Id,name as Name from playlist order by playlist_id;";
            return _runner.Execute<PlayList>(palyListSql, null);
        }

        public PlayList GetById(int playlistId)
        {
            var sql = @"select playlist_id as Id,name as Name from playlist where playlist_id = @0 order by playlist_id;";
            return _runner.ExecuteToSingle<PlayList>(sql, playlistId);
        }

        public async Task<PlayList> GetTracksById(int playlistId)
        {
            var list = new PlayList();
            var playListSql = @"select playlist_id as Id,name as Name from playlist where playlist_id = @0 order by playlist_id;";
            var tracks = @"SELECT track.track_id as Id,track.name as Name ,(track.milliseconds / 60000)::decimal as Duration ,(track.bytes / 1048576 )::decimal(10,3) as Bytes,track.unit_price::decimal as Price,track.composer as Composer from playlist_track inner join track on track.track_id = playlist_track.track_id where playlist_track.playlist_id = @0 order by composer;";
            using (var reader= await _runner.OpenReaderAsync(playListSql+tracks,playlistId))
            {
                if (reader.ReadAsync() != null)
                {
                    list = reader.ToSingle<PlayList>();
                    await reader.NextResultAsync();
                    list.Tracks = reader.ToList<Track>();
                }
            }
            return list;
        }
        
    }
}