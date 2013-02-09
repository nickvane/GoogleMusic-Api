using System;
using System.Runtime.Serialization;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class DeletePlaylistResp
    {
        [DataMember(Name="deleteId")]
        public String Id { get; set; }
    }
}