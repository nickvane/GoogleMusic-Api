﻿using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace GoogleMusic.Api.Models
{
    [DataContract]
    public class GoogleMusicSong
    {
        string albumart;

        [DataMember(Name= "genre")]
        public string Genre { get; set; }

        [DataMember(Name= "beatsPerMinute")]
        public int Bpm { get; set; }

        [DataMember(Name= "albumArtistNorm")]
        public string AlbumArtistNorm { get; set; }

        [DataMember(Name= "artistNorm")]
        public string ArtistNorm { get; set; }

        [DataMember(Name= "album")]
        public string Album { get; set; }

        [DataMember(Name= "lastPlayed")]
        public double LastPlayed { get; set; }

        [DataMember(Name= "type")]
        public int Type { get; set; }

        [DataMember(Name= "disc")]
        public int Disc { get; set; }

        [DataMember(Name= "id")]
        public string Id { get; set; }

        [DataMember(Name= "composer")]
        public string Composer { get; set; }

        [DataMember(Name= "title")]
        public string Title { get; set; }

        [DataMember(Name= "albumArtist")]
        public string AlbumArtist { get; set; }

        [DataMember(Name= "totalTracks")]
        public int TotalTracks { get; set; }

        [DataMember(Name= "name")]
        public string Name { get; set; }

        [DataMember(Name= "totalDiscs")]
        public int TotalDiscs { get; set; }

        [DataMember(Name= "year")]
        public int Year { get; set; }

        [DataMember(Name= "titleNorm")]
        public string TitleNorm { get; set; }

        [DataMember(Name= "artist")]
        public string Artist { get; set; }

        [DataMember(Name= "albumNorm")]
        public string AlbumNorm { get; set; }

        [DataMember(Name= "track")]
        public int Track { get; set; }

        [DataMember(Name= "durationMillis")]
        public long Duration { get; set; }

        [DataMember(Name= "albumArt")]
        public string AlbumArt { get; set; }

        [DataMember(Name= "deleted")]
        public bool Deleted { get; set; }

        [DataMember(Name= "url")]
        public string Url { get; set; }

        [DataMember(Name= "creationDate")]
        public float CreationDate { get; set; }

        [DataMember(Name= "playCount")]
        public int Playcount { get; set; }

        [DataMember(Name= "rating")]
        public int Rating { get; set; }

        [DataMember(Name= "comment")]
        public string Comment { get; set; }

        [DataMember(Name= "albumArtUrl")]
        public string ArtUrl
        {
            get
            {
                if (string.IsNullOrEmpty(albumart)) return string.Empty;
                // change the size by adjusting the last part of the url
                // https://lh3.googleusercontent.com/kC0Rjq90XqeCsh3jpHww5rJ6WJEKsp2fRgJk0zMkOu88CTUMEY-yHOmHfX4pFpui24RXLe4qTosZYaQ=s128-c-e100
                var artUrl = Regex.Replace(albumart, @"[=][s]\d+[-][c][-][e]\d+", "=s500-c-e100", RegexOptions.IgnoreCase);
                return (!artUrl.StartsWith("http:")) ? "http:" + artUrl : artUrl;
            }
            set { albumart = value; }
        }

        public string ArtistAlbum
        {
            get
            {
                return Artist + ", " + Album;
            }
        }
    }
}
