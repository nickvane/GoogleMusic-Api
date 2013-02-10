GoogleMusic-Api
===============

C# API for using Google Music features.

I used https://github.com/taylorfinnell/GoogleMusicAPI.NET which is a port of https://github.com/simon-weber/Unofficial-Google-Music-API as starting point, but switched to using async await instead of callbacks to simplify code.
The project references portable .net framework, so should be usable in mono.

Current features:
* Authentication
* Get all songs
* Get all playlists
* Get stream url for song
* Add playlist
* Delete playlist
* Rename playlist
