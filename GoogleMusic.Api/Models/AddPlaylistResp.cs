using System;
using System.Runtime.Serialization;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class AddPlaylistResp
    {
        [DataMember(Name="id")]
        public String Id { get; set; }

        [DataMember(Name="title")]
        public String Title { get; set; }

        [DataMember(Name="success")]
        public bool Success { get; set; }
    }
}