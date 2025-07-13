using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media.Imaging;
using TagLib;

namespace TagEditor.Files
{
	public class FilesManager
	{
		public static async Task OpenAudioFile(Dictionary<string, AudioFile> filesList, StorageFile file)
		{
			string songName = "";
			string artists = "";
			string albumName = "";
			DateTimeOffset yearOfRelease = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
			uint trackNumber = 1;
			BitmapImage albumArt = null;
			string filePath = file.Path;
			TagLib.File tfile = null;

			try
			{
				tfile = TagLib.File.Create(filePath);

				songName = tfile.Tag.Title ?? "";
				albumName = tfile.Tag.Album ?? "";
				artists = (tfile.Tag.Performers != null && tfile.Tag.Performers.Length > 0)
					? string.Join("; ", tfile.Tag.Performers)
					: "";

				if (tfile.Tag.Year != 0)
				{
					yearOfRelease = new DateTimeOffset((int)tfile.Tag.Year, 1, 1, 0, 0, 0, TimeSpan.Zero);
				}

				trackNumber = tfile.Tag.Track;

				// Обработка обложки
				if (tfile.Tag.Pictures is { Length: > 0 })
				{
					try
					{
						var pic = tfile.Tag.Pictures[0];
						using var ms = new MemoryStream(pic.Data.Data);
						ms.Seek(0, SeekOrigin.Begin);
						var randomAccessStream = new InMemoryRandomAccessStream();
						await ms.CopyToAsync(randomAccessStream.AsStreamForWrite());
						await randomAccessStream.FlushAsync();
						randomAccessStream.Seek(0);

						albumArt = new BitmapImage();
						await albumArt.SetSourceAsync(randomAccessStream);
					}
					catch
					{
						albumArt = new BitmapImage(new Uri("ms-appx:///Assets/tempCat.jpg"));
					}
				}
				else
				{
					albumArt = new BitmapImage(new Uri("ms-appx:///Assets/tempCat.jpg"));
				}

				filesList[filePath] = new AudioFile(songName, albumName, artists, yearOfRelease, trackNumber, albumArt, filePath, tfile);
			}
			catch (Exception ex)
			{
				throw new FileNotFoundException($"Ошибка при открытии файла: {ex.Message}", ex);
			}
		}

		public static async Task SaveAudioFile(Dictionary<string, AudioFile> filesList, string filePath)
		{
			if (!System.IO.File.Exists(filePath))
				throw new FileNotFoundException();

			var tfile = filesList[filePath].TFile;
			tfile.Tag.Title = filesList[filePath].SongName;
			tfile.Tag.Album = filesList[filePath].AlbumName;
			tfile.Tag.Performers = filesList[filePath].Artists.Split(';', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			tfile.Tag.Year = (uint)filesList[filePath].YearOfRelease.Year;
			tfile.Tag.Track = filesList[filePath].TrackNumber;

			var albumArt = filesList[filePath].AlbumArt;
			if (albumArt?.UriSource is { } uriSource && !string.IsNullOrWhiteSpace(uriSource.OriginalString))
			{
				StorageFile imageFile = null;
				try
				{
					imageFile = uriSource.IsFile
						? await StorageFile.GetFileFromPathAsync(uriSource.LocalPath)
						: await StorageFile.GetFileFromApplicationUriAsync(uriSource);
				}
				catch
				{
					imageFile = null;
				}

				if (imageFile != null)
				{
					using var stream = await imageFile.OpenStreamForReadAsync();
					byte[] imageBytes = new byte[stream.Length];
					await stream.ReadAsync(imageBytes, 0, (int)stream.Length);

					var picture = new TagLib.Picture
					{
						Data = imageBytes,
						Type = PictureType.FrontCover
					};
					tfile.Tag.Pictures = new TagLib.IPicture[] { picture };
				}
			}

			tfile.Save();
		}
	}
}

