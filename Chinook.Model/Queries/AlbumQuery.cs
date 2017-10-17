using System;
using System.Collections.Generic;
using Chinook.Model.Data;

namespace Chinook.Model.Queries
{
    public class AlbumQuery
    {
        private readonly IRunner _runner;

        public AlbumQuery(IRunner runner)
        {
            _runner = runner;
        }

        public IEnumerable<Album> GetAll()
        {
            var sql = "select album_id as Id,title as Title,artist_id as ArtistId from album where album_id=@0";
            return _runner.Execute<Album>(sql, null);
        }
        public Album GetById(int albumId)
        {
            var args = new object[] {albumId};
            var sql = "select album_id as Id,title as Title,artist_id as ArtistId from album where album_id=@0";
            return _runner.ExecuteToSingle<Album>(sql, args);
        }
        public IEnumerable<Album> GetByArtistId(int artistId)
        {
            var args = new Object[] {artistId};
            var sql = "select album_id as Id,title as Title from album where artist_id = @0";
            return _runner.Execute<Album>(sql, args);
        }
    }
}