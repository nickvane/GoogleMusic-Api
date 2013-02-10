GoogleMusic-Api
===============

C# API for using Google Music features.

Used https://github.com/taylorfinnell/GoogleMusicAPI.NET as starting point, but switched to using async await instead of callbacks to simplify code.
The project references portable .net framework, so should be usable in mono.

Current features:
* Authentication
* Get all songs
* Get all playlists
* Get stream url for song
* Add playlist
* Delete playlist
* Rename playlist
