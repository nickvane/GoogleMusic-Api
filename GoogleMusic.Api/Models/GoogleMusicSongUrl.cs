using System;
using System.Runtime.Serialization;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class GoogleMusicSongUrl
    {
        [DataMember(Name="url")]
        public String Url { get; set; }
    };
}