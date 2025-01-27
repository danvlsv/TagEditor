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
using System.Collections;
using System.Windows.Input;
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

		public Dictionary<string,AudioFile> openedFiles = [];
		public Dictionary<string, SongListElement> openedFilesControls = new Dictionary<string, SongListElement>();

		private string currentFile = "";
		//public List<AudioFile> openedFiles = new List<AudioFile>();

		public MainWindow()
		{
			this.SetTitleBar(AppTitleBar);
			this.MinHeight = 550;
			this.MinWidth = 900;
			this.Height = 550;
			this.Width = 900;
			//this.IsResizable = false;
			//this.IsMaximizable = false;
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

				var songListElement = new SongListElement(this, openedFiles[file.Path]);
				openedFilesControls[file.Path] = songListElement;

				this.OpenedSongs.Children.Add(songListElement);
				this.Welcome.Visibility = Visibility.Collapsed;
				this.Editor.Visibility = Visibility.Visible;

				SetCurrentFile(file.Path);
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
				openedFiles[currentFile].AlbumArt = newImage;
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
				this.SongTitle.Text = openedFiles[currentFile].SongName;
				this.Artist.Text = openedFiles[currentFile].Artists;
				this.Album.Text = openedFiles[currentFile].AlbumName;
				this.YearOfRelease.Date = openedFiles[currentFile].YearOfRelease;
				this.TrackNumber.Value = openedFiles[currentFile].TrackNumber;
				this.Art.Source = openedFiles[currentFile].AlbumArt;
			}
			else
			{
				ElementSoundPlayer.Play(ElementSoundKind.GoBack);
			}

		}

		public void CloseFile(string filePathKey)
		{
			if (filePathKey == currentFile)
			{
				var listOfPaths = openedFiles.Keys.ToList();
				int index = listOfPaths.IndexOf(filePathKey);
				if (openedFiles.Count == 1)
				{
					this.Welcome.Visibility = Visibility.Visible;
					this.Editor.Visibility = Visibility.Collapsed;
					currentFile = null;
				}
				else
				{
					if (index == 0)
					{
						SetCurrentFile(listOfPaths[index + 1]);
					}
					else
					{
						SetCurrentFile(listOfPaths[index - 1]);
					}
				}
				

				
				
			}
			openedFiles.Remove(filePathKey);
			this.OpenedSongs.Children.Remove(openedFilesControls[filePathKey]);
			openedFilesControls.Remove(filePathKey);
		}

		private void SaveToDictionary(string  filePathKey)
		{
			openedFiles[currentFile].SongName = this.SongTitle.Text;
			openedFiles[currentFile].Artists = this.Artist.Text;
			openedFiles[currentFile].AlbumName = this.Album.Text;
			openedFiles[currentFile].YearOfRelease = this.YearOfRelease.Date;
			openedFiles[currentFile].TrackNumber = (uint)this.TrackNumber.Value;
			openedFiles[currentFile].AlbumArt = (BitmapImage)this.Art.Source;

		}
	}


}
