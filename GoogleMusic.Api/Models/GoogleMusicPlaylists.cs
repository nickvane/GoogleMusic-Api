using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class GoogleMusicPlaylists
    {
        [DataMember(Name="playlists")]
        public List<GoogleMusicPlaylist> UserPlaylists { get; set; }

        [DataMember(Name="magicPlaylists")]
        public List<GoogleMusicPlaylist> InstantMixes { get; set; }
    }
}