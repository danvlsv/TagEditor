using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using WinUIEx;
using System.Diagnostics;
using System.Drawing;

//using System.Windows.Media.Imaging;


using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Core;
using TagEditor.Files;
//using BitmapImage = System.Windows.Media.Imaging.BitmapImage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagEditor
{

	
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WinUIEx.WindowEx
	{

		public Dictionary<string,AudioFile> openedFiles = new Dictionary<string,AudioFile>();
		private string currentFile = "";
		//public List<AudioFile> openedFiles = new List<AudioFile>();

		public MainWindow()
		{
			this.SetTitleBar(AppTitleBar);
			//this.MinHeight = 550;
			//this.MinWidth =850;
			this.Height = 550;
			this.Width = 900;
			this.IsResizable = false;
			//this.Title = "Music Player";
			this.SystemBackdrop = new DesktopAcrylicBackdrop();

			this.InitializeComponent();

			ElementSoundPlayer.State = ElementSoundPlayerState.On;
			this.Editor.Visibility = Visibility.Collapsed;
			//this.PersistenceId = "MainWindowSize";
		}

		private void WelcomeButton_Click(object sender, RoutedEventArgs e)
		{

			OpenFile();
		}

		private void OpenDifferentButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFile();
		}



		private void OpenImageButton_Click(object sender, RoutedEventArgs e)
		{
			OpenImage();
		}

		private void SaveFileButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFile();
		}

		private async void OpenFile()
		{
			var filePicker = new FileOpenPicker();
			filePicker.FileTypeFilter.Add(".mp3");
	
			// Get the current window's HWND by passing in the Window object
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

			// Associate the HWND with the file picker
			WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

			var file = await filePicker.PickSingleFileAsync();
			if (file != null && openedFiles.ContainsKey(file.Path)==false)
			{
				await FilesManager.OpenAudioFile(openedFiles, file);

				var songListElement = new SongListElement(this)
				{
					AlbumArtSource = openedFiles[file.Path].albumArt,
					SongName = openedFiles[file.Path].songName,
					ArtistName = openedFiles[file.Path].artists,
					FilePath = openedFiles[file.Path].filePath
				};
				this.OpenedSongs.Children.Add(songListElement);
				this.Welcome.Visibility = Visibility.Collapsed;
				this.Editor.Visibility = Visibility.Visible;
				//this.Art.Source = new BitmapImage(new Uri("ms-appx:///Assets/tempCat.jpg"));
				SetCurrentFile(file.Path);
				Debug.WriteLine($"File path: {songListElement.FilePath}");
			}
			else
			{
				ElementSoundPlayer.Play(ElementSoundKind.GoBack);
			}

		}


		private async void OpenImage()
		{
			var filePicker = new FileOpenPicker();
			filePicker.FileTypeFilter.Add(".jpeg");
			filePicker.FileTypeFilter.Add(".png");
			filePicker.FileTypeFilter.Add(".jpg");

			// Get the current window's HWND by passing in the Window object
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

			// Associate the HWND with the file picker
			WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

			var file = await filePicker.PickSingleFileAsync();
			if (file != null)
			{
				BitmapImage newImage = new BitmapImage(new Uri(file.Path));
				openedFiles[currentFile].albumArt = newImage;
				this.Art.Source = newImage;
				
			}
			else
			{
				ElementSoundPlayer.Play(ElementSoundKind.GoBack);
			}
		}

		private async void SaveFile()
		{
			SaveToDictionary(currentFile);
			await FilesManager.SaveAudioFile(openedFiles,currentFile);
		}

		public void SetCurrentFile(string filePathKey)
		{
			if (filePathKey != currentFile)
			{
				if (!String.IsNullOrEmpty(currentFile))
				{
					SaveToDictionary(currentFile);
				}
				
				currentFile = filePathKey;
				this.SongTitle.Text = openedFiles[currentFile].songName;
				this.Artist.Text = openedFiles[currentFile].artists;
				this.Album.Text = openedFiles[currentFile].albumName;
				this.YearOfRelease.Date = openedFiles[currentFile].yearOfRelease;
				this.DiscNumber.Value = openedFiles[currentFile].discNumber;
				this.Art.Source = openedFiles[currentFile].albumArt;
			}
			else
			{
				ElementSoundPlayer.Play(ElementSoundKind.GoBack);
			}

		}

		private void SaveToDictionary(string filePathKey)
		{
			openedFiles[currentFile].songName = this.SongTitle.Text;
			openedFiles[currentFile].artists = this.Artist.Text;
			openedFiles[currentFile].albumName = this.Album.Text;
			openedFiles[currentFile].yearOfRelease = this.YearOfRelease.Date;
			openedFiles[currentFile].discNumber = (uint)this.DiscNumber.Value;
			openedFiles[currentFile].albumArt = (BitmapImage)this.Art.Source;
		}
	}


}
