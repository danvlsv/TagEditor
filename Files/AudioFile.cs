using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace TagEditor.Files
{
	public class AudioFile
	{
		public string songName = "";
		public string albumName = "";
		public string artists = "";
		public DateTimeOffset yearOfRelease;
		public uint discNumber;
		//public Uri albumArtSource;
		public BitmapImage albumArt;
		//MediaSource audioSource;
		public string filePath;
		public TagLib.File tfile;

		bool saved = false;

		public AudioFile(string songName, string albumName, string artists, DateTimeOffset yearOfRelease, uint discNumber, BitmapImage albumArt, string filePath, TagLib.File tfile)
		{
			this.songName = songName;
			this.albumName = albumName;
			this.artists = artists;
			this.yearOfRelease = yearOfRelease;
			this.discNumber = discNumber;
			this.albumArt= albumArt;
			//this.audioSource = audioSource;
			this.filePath = filePath;
			this.tfile = tfile;
		}
	}
}
