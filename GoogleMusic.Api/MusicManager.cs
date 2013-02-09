using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMusic.Api.Helpers;
using GoogleMusic.Api.Models;

namespace GoogleMusic.Api
{
    public class MusicManager : IMusicManager
    {
        private Client _client;
        public static string BaseUrl = "https://play.google.com/music/";
        public static string ServiceUrl = BaseUrl + "services/";

        public MusicManager()
        {
            _client = new Client();
        }

        /// <summary>
        /// Authenticates the api with the given credentials.
        /// </summary>
        /// <param name="email">eg `test@gmail.com` or just `test`.</param>
        /// <param name="password">plaintext password. It will not be stored and is sent over ssl.</param>
        /// <returns>Returns True on success, False on failure.</returns>
        /// <remarks>Users of two-factor authentication will need to set an application-specific password to log in.</remarks>
        public async Task<bool> Login(string email, string password)
        {
            var fields = new Dictionary<string, string>
                             {
                                 {"service", "sj"},
                                 {"Email", email},
                                 {"Passwd", password},
                             };

            var form = new FormBuilder(fields, true);
            var tokenContent = await _client.UploadDataAsync(new Uri("https://www.google.com/accounts/ClientLogin"), form);

            var tokens = tokenContent.Split('\n');
            foreach (var token in tokens)
            {
                if (token.StartsWith("SID=")) _client.SetSidToken(token.Replace("SID=", ""));
                if (token.StartsWith("LSID=")) _client.SetLsidToken(token.Replace("LSID=", ""));
                if (token.StartsWith("Auth=")) _client.SetAuthorizationToken(token.Replace("Auth=", ""));
            }

            var content = await _client.UploadDataAsync(new Uri(BaseUrl + "listen?hl=en&u=0"), FormBuilder.Empty, true);
            return !string.IsNullOrEmpty(content);
        }

        /// <summary>
        /// Forgets local authentication without affecting the server.
        /// </summary>
        public void Logout()
        {
            _client = new Client();
        }

        public async Task<List<GoogleMusicSong>> GetAllSongs()
        {
            var songs = new List<GoogleMusicSong>();
            var token = await GetAllSongs(songs, "");
            while (!string.IsNullOrEmpty(token))
            {
                token = await GetAllSongs(songs, token);
            }
            return songs;
        }

        public async Task<string> GetSongStreamUrl(string songId)
        {
            var result = await _client.DownloadDataAsync(new Uri(String.Format(BaseUrl + "play?u=0&songid={0}&pt=e", songId)));
            return Json.Deserialize<GoogleMusicSongUrl>(result).Url;
        }

        public async Task<string> GetSongStreamUrl(Uri address)
        {
            var result = await _client.DownloadDataAsync(address);
            return Json.Deserialize<GoogleMusicSongUrl>(result).Url;
        }

        public async Task<GoogleMusicPlaylists> GetAllPlaylists()
        {
            var fields = new Dictionary<string, string> { { "json", "{}" } };
            var form = new FormBuilder(fields, true);

            var content = await _client.UploadDataAsync(new Uri(ServiceUrl + "loadplaylist"), form);
            return Json.Deserialize<GoogleMusicPlaylists>(content);
        }

        /// <summary>
        /// Creates a new playlist. Returns the new playlist id.
        /// </summary>
        /// <param name="playlistName">the title of the playlist to create.</param>
        public async void AddPlaylist(string playlistName)
        {
            var fields = new Dictionary<string, string> { { "json", string.Format("{{\"title\":\"{0}\"}}", playlistName) } };
            var form = new FormBuilder(fields, true);

            var content = await _client.UploadDataAsync(new Uri(ServiceUrl + "addplaylist"), form);
            if (!Json.Deserialize<AddPlaylistResp>(content).Success)
                throw new Exception("Playlist could not be added");
        }

        public async void DeletePlaylist(string playlistId)
        {
            var fields = new Dictionary<string, string> { { "json", string.Format("{{\"id\":\"{0}\"}}", playlistId) } };
            var form = new FormBuilder(fields, true);

            var content = await _client.UploadDataAsync(new Uri(ServiceUrl + "modifyplaylist"), form);
            if (Json.Deserialize<DeletePlaylistResp>(content).Id != playlistId)
                throw new Exception("Playlist could not be deleted");
        }

        /// <summary>
        /// Changes the name of a playlist.
        /// </summary>
        /// <param name="playlistId">id of the playlist to rename.</param>
        /// <param name="name">desired title.</param>
        public async void RenamePlaylist(string playlistId, string name)
        {
            var fields = new Dictionary<string, string> { { "json", string.Format("{{\"playlistId\": \"{0}\", \"playlistName\": \"{1}\"}}", playlistId, name) } };
            var form = new FormBuilder(fields, true);

            await _client.UploadDataAsync(new Uri(ServiceUrl + "addplaylist"), form);
        }

        #region private methods

        private async Task<string> GetAllSongs(List<GoogleMusicSong> songs, string continuationToken)
        {
            var fields = new Dictionary<string, string> { { "json", string.Format("{{\"continuationToken\":\"{0}\"}}", continuationToken) } };
            var form = new FormBuilder(fields, true);

            var content = await _client.UploadDataAsync(new Uri(ServiceUrl + "loadalltracks"), form);
            var playlist = Json.Deserialize<GoogleMusicPlaylist>(content);
            songs.AddRange(playlist.Songs);
            return playlist.ContinuationToken;
        }

        #endregion
    }
}
