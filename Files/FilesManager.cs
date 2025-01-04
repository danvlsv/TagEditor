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
using Windows.Storage.Pickers;
using System.Formats.Tar;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Resources;
using TagLib;
//using System.Windows.Media.Imaging;
using Windows.Graphics.Imaging;
//using System.Windows.Media.Imaging;
//using System.Windows.Media.Imaging;

namespace TagEditor.Files
{
	public class FilesManager
	{




		static public async Task OpenAudioFile(Dictionary<string, AudioFile> filesList, StorageFile file)
		{
			string songName = "";
			string artists = "";
			string albumName = "";
			DateTimeOffset yearOfRelease = new DateTimeOffset(1970,1,1,0,0,0,TimeSpan.Zero);
			uint discNumber = 1;
			BitmapImage albumArt;
			Uri albumArtSource;
			string filePath;
			TagLib.File tfile = null;


			tfile = TagLib.File.Create(file.Path);

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

			if (tfile.Tag.Year != 0)
			{
				TimeSpan ts = new(0, 0, 0);
				yearOfRelease = new DateTimeOffset((int)tfile.Tag.Year, 1, 1, 1, 0, 0, ts);
			}


			discNumber = tfile.Tag.Disc;


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
				//pictureFound = false;
				//this.Art.Source = null;
				albumArtSource = new Uri("ms-appx:///Assets/tempCat.jpg");

				albumArt = new BitmapImage(albumArtSource);
				

			}



			filePath = file.Path.ToString();


			filesList[filePath] = new AudioFile(songName, albumName, artists, yearOfRelease, discNumber, albumArt, filePath, tfile);
		}


		static public async Task SaveAudioFile(Dictionary<string, AudioFile> filesList, string filePath)
		{
			if (System.IO.File.Exists(filePath))
			{
				TagLib.File tfile = filesList[filePath].TFile;
				tfile.Tag.Title = filesList[filePath].SongName;
				tfile.Tag.Album = filesList[filePath].AlbumName;
				tfile.Tag.Performers = filesList[filePath].Artists.Split(';');

				tfile.Tag.Year = (uint)filesList[filePath].YearOfRelease.Year;
				tfile.Tag.Disc = filesList[filePath].DiscNumber;

				
				try
				{
					Uri albumArtSource = filesList[filePath].AlbumArt.UriSource; // Replace with your actual URI

					//StorageFile imageFile = await StorageFile.GetFileFromApplicationUriAsync(albumArtSource);
					//using (var stream = await imageFile.OpenReadAsync())
					//{
					//	using (var memoryStream = new MemoryStream())
					//	{
					//		// Читаем данные из IInputStream и записываем их в MemoryStream
					//		using (var dataReader = new DataReader(stream))
					//		{
					//			uint bytesLoaded = await dataReader.LoadAsync((uint)stream.Size);
					//			byte[] imageData = new byte[bytesLoaded];
					//			dataReader.ReadBytes(imageData);

					//			// Создаем объект TagLib.Picture
					//			var picture = new TagLib.Picture
					//			{
					//				Data = new TagLib.ByteVector(imageData),
					//				Type = TagLib.PictureType.FrontCover, // Установите тип по мере необходимости
					//				Description = "Cover",
					//				MimeType = "image/jpeg" // Установите правильный MIME-тип в зависимости от формата изображения
					//			};

					//			// Присваиваем изображение тегу
					//			tfile.Tag.Pictures = new[] { picture }; // Используйте массив для установки изображений
					//		}

					//	}
					//}

				}
				catch (Exception)
				{

				}

				tfile.Save();

			}
		}







	}
}

