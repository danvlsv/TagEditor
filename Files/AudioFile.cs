using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

namespace TagEditor.Files
{
	public class AudioFile : INotifyPropertyChanged
	{
		private string songName;
		private string albumName;
		private string artists;
		private DateTimeOffset yearOfRelease;
		private uint trackNumber;
		private BitmapImage albumArt;
		private string filePath;
		private TagLib.File tfile;

		public AudioFile(string songName, string albumName, string artists, DateTimeOffset yearOfRelease, uint trackNumber, BitmapImage albumArt, string filePath, TagLib.File tfile)
		{
			SongName = songName;
			AlbumName = albumName;
			Artists = artists;
			YearOfRelease = yearOfRelease;
			TrackNumber = trackNumber;
			AlbumArt = albumArt;
			FilePath = filePath;
			TFile = tfile;
		}

		public string SongName
		{
			get => songName;
			set
			{
				if (!Equals(songName, value))
				{
					songName = value;
					OnPropertyChanged();
				}
			}
		}

		public string AlbumName
		{
			get => albumName;
			set
			{
				if (!Equals(albumName, value))
				{
					albumName = value;
					OnPropertyChanged();
				}
			}
		}

		public string Artists
		{
			get => artists;
			set
			{
				if (!Equals(artists, value))
				{
					artists = value;
					OnPropertyChanged();
				}
			}
		}

		public DateTimeOffset YearOfRelease
		{
			get => yearOfRelease;
			set
			{
				if (!Equals(yearOfRelease, value))
				{
					yearOfRelease = value;
					OnPropertyChanged();
				}
			}
		}

		public uint TrackNumber
		{
			get => trackNumber;
			set
			{
				if (!Equals(trackNumber, value))
				{
					trackNumber = value;
					OnPropertyChanged();
				}
			}
		}

		public BitmapImage AlbumArt
		{
			get => albumArt;
			set
			{
				if (!Equals(albumArt, value))
				{
					albumArt = value;
					OnPropertyChanged();
				}
			}
		}

		public string FilePath
		{
			get => filePath;
			set
			{
				if (!Equals(filePath, value))
				{
					filePath = value;
					OnPropertyChanged();
				}
			}
		}

		public TagLib.File TFile
		{
			get => tfile;
			set
			{
				if (!Equals(tfile, value))
				{
					tfile = value;
					OnPropertyChanged();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
