using System;
using System.Collections.Generic;
//using System.Windows.Media.Imaging;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Streams;

//using System.Windows.Media.Imaging;


using Microsoft.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace TagEditor.Files
{
	public class FilesManager
	{




		static public async Task OpenAudioFile(Dictionary<string, AudioFile> filesList, StorageFile file)
		{
			string songName = "";
			string artists = "";
			string albumName = "";
			DateTimeOffset yearOfRelease = DateTimeOffset.MinValue;
			uint discNumber = 1;
			BitmapImage albumArt;
			Uri albumArtSource;
			//MediaSource audioSource;
			string filePath;

			var tfile = TagLib.File.Create(file.Path);

			//this.SongTitle.Text = tfile.Tag.Title;
			songName = tfile.Tag.Title;
			albumName = tfile.Tag.Album;

			string[] artistsFromFile = tfile.Tag.Performers;
			for (int i = 0; i < artistsFromFile.Length; i++)
			{
				artists += artistsFromFile[i];
				if (i != artistsFromFile.Length - 1)
				{
					artists += "; ";
				}
			}
			//this.Artist.Text = artists;


			//this.Album.Text = tfile.Tag.Album;

			//Debug.Print(tfile.Tag.Year.ToString());
			if (tfile.Tag.Year != 0)
			{
				TimeSpan ts = new(0, 0, 0);
				yearOfRelease = new DateTimeOffset((int)tfile.Tag.Year, 1, 1, 1, 0, 0, ts);
				//this.YearOfRelease.Date = new DateTimeOffset((int)tfile.Tag.Year, 1, 1, 1, 0, 0, ts);
			}


			discNumber = tfile.Tag.Disc;
			//this.DiscNumber.Value = tfile.Tag.Disc;

			bool pictureFound = true;

			albumArt = new BitmapImage();
			try
			{
				TagLib.IPicture pic = tfile.Tag.Pictures[0];
				using (MemoryStream ms = new MemoryStream(pic.Data.Data))
				{
					ms.Seek(0, SeekOrigin.Begin);

					// Convert MemoryStream to IRandomAccessStream
					var randomAccessStream = new InMemoryRandomAccessStream();
					await ms.CopyToAsync(randomAccessStream.AsStreamForWrite());
					await randomAccessStream.FlushAsync();
					randomAccessStream.Seek(0); // Reset the stream position

					// Set the BitmapImage source
					albumArt = new BitmapImage();
					await albumArt.SetSourceAsync(randomAccessStream);

				}
			}
			catch
			{
				pictureFound = false;
				//this.Art.Source = null;
				albumArtSource = new Uri("ms-appx:///Assets/tempCat.jpg");
				albumArt = new BitmapImage(albumArtSource);
			}


		//	if (pictureFound)
		//	{

		//	}
		//	else
		//	{
				
		//	}

		

		//albumArtSource = new Uri("ms-appx:///Assets/tempCat.jpg");
		//albumArt = new BitmapImage(albumArtSource);



		//audioSource = MediaSource.CreateFromStorageFile(file);
		filePath = file.Path.ToString();

//this.MusicPlayer.Source = MediaSource.CreateFromStorageFile(file);
//this.FilePath.Text = file.Path.ToString();
filesList[filePath] = new AudioFile(songName, albumName, artists, yearOfRelease, discNumber, albumArt, filePath);
		//filesList.Add(new AudioFile(songName, albumName, artists, yearOfRelease, discNumber, albumArtSource, filePath));
	}

	}
}

