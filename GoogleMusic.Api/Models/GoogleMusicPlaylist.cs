using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class GoogleMusicPlaylist
    {
        [DataMember(Name= "title")]
        public string Title { get; set; }

        [DataMember(Name= "playlistId")]
        public string PlaylistId { get; set; }

        [DataMember(Name= "requestTime")]
        public double RequestTime { get; set; }

        [DataMember(Name= "continuationToken")]
        public string ContinuationToken { get; set; }

        [DataMember(Name= "differentialUpdate")]
        public bool DifferentialUpdate { get; set; }

        [DataMember(Name= "playlist")]
        public List<GoogleMusicSong> Songs { get; set; }

        [DataMember(Name= "continuation")]
        public bool Continuation { get; set; }

        public string TrackString
        {
            get { return Songs.Count + " tracks"; }
        }
    }
}