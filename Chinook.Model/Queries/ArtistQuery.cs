using System.Collections.Generic;
using System.Linq;
using Chinook.Model.Data;

namespace Chinook.Model.Queries
{
    public class ArtistQuery
    {
        private readonly IRunner _runner;
        public ArtistQuery(IRunner runner)
        {
            _runner = runner;
        }

        public IEnumerable<Artist> GetAll()
        {
            var query = @"select artist_id as Id , name as Name from artist;";
            return  _runner.Execute<Artist>(query);
        }

        public Artist GetById(int id)
        {
            var args = new object[] { id };
            var query = @"select artist_id as Id , name as Name from artist where artist_id = @0";
            return _runner.ExecuteToSingle<Artist>(query,args);
        }

        public Artist GetByAlbums(int id)
        {
            var result = new Artist();
            var artistSql = @"select artist_id as Id , name as Name from artist where artist_id = @0;";
            var albumSql = @"select album_id as Id , title as Title from album where artist_id = @0;";
            using (var reader = _runner.OpenReader(artistSql + albumSql,new object[] { id }))
            {
                if (reader.Read())
                {
                    result = reader.ToSingle<Artist>();
                    reader.NextResult();
                    result.Albums = reader.ToList<Album>();
                }
                reader.Dispose();
            }
            return result;
        }

        public Artist GetArtistFullAlbum(int artist_id)
        {
            var artist = new Artist();
            var artistSql = @"select artist_id as Id , name as Name from artist where artist_id = @0;";
            var albumSql = @"select album_id as Id , title as Title from album where artist_id = @0;";
            var trackSql = @"select * from track where album_id=@1";
            using (var reader = _runner.OpenReader(artistSql + albumSql,new object[] { artist }))
            {
                if (reader.Read())
                {
                    artist = reader.ToSingle<Artist>();
                    reader.NextResult();
                    artist.Albums = reader.ToList<Album>();
                    reader.NextResult();
                    foreach (var track in reader.ToList<Track>())
                    {
                        foreach (var album in artist.Albums)
                        {
                            album.Tracks.Add(track);
                        }
                    }
                }
                reader.Dispose();
            }
            return artist;
        }
    }
}