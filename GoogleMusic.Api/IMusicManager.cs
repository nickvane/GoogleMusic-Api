using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleMusic.Api.Models;

namespace GoogleMusic.Api
{
    public interface IMusicManager
    {
        Task<bool> Login(string email, string password);
        void Logout();
        Task<List<GoogleMusicSong>> GetAllSongs();
        Task<string> GetSongStreamUrl(string songId);

        Task<GoogleMusicPlaylists> GetAllPlaylists();
        void AddPlaylist(string playlist);
        void DeletePlaylist(string playlistId);
        void RenamePlaylist(string playlistId, string name);
    }
}