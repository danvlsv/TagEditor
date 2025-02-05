using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TagEditor.Files;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagEditor.Controls
{
	public sealed partial class Editor : UserControl
	{

		public Dictionary<string, AudioFile> openedFiles = [];
		public Dictionary<string, SongListElement> openedFilesControls = new Dictionary<string, SongListElement>();

		private string currentFile = "";

		//Window mainWindow;

		public Editor()
		{
			//mainWindow = App.m_window;
			this.InitializeComponent();
			this.EditorComponent.Visibility = Visibility.Collapsed;
		}


		private async void OpenFile()
		{
			var filePicker = new FileOpenPicker();
			filePicker.FileTypeFilter.Add(".mp3");

			// Get the current window's HWND by passing in the Window object
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.m_window);

			// Associate the HWND with the file picker
			WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

			var file = await filePicker.PickSingleFileAsync();
			if (file != null && openedFiles.ContainsKey(file.Path) == false)
			{
				try
				{
					await FilesManager.OpenAudioFile(openedFiles, file);

					var songListElement = new SongListElement(this, openedFiles[file.Path]);
					openedFilesControls[file.Path] = songListElement;

					this.OpenedSongs.Children.Add(songListElement);
					this.Welcome.Visibility = Visibility.Collapsed;
					this.EditorComponent.Visibility = Visibility.Visible;

					SetCurrentFile(file.Path);
				}
				catch
				{
					ContentDialog dialog = new();
					dialog.XamlRoot = this.Content.XamlRoot;
					dialog.Title = "File has corrupt metadata";
					dialog.CloseButtonText = "Ok";
					dialog.DefaultButton = ContentDialogButton.Close;
					dialog.Content = $"File:\n\n{file.Path}\n\n is corrupt.";

					await dialog.ShowAsync();
				}

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
			try
			{
				await FilesManager.SaveAudioFile(openedFiles, currentFile);
			}
			catch (Exception ex)
			{
				if (ex is FileNotFoundException)
				{
					ContentDialog dialog = new();
					dialog.XamlRoot = this.Content.XamlRoot;
					dialog.Title = "File not found";
					dialog.CloseButtonText = "Ok";
					dialog.DefaultButton = ContentDialogButton.Close;
					dialog.Content = $"File:\n\n{currentFile}\n\nwasn't found.";


					CloseFile(currentFile);
					await dialog.ShowAsync();


				}
			}

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
					this.Visibility = Visibility.Collapsed;
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

		private void SaveToDictionary(string filePathKey)
		{
			openedFiles[currentFile].SongName = this.SongTitle.Text;
			openedFiles[currentFile].Artists = this.Artist.Text;
			openedFiles[currentFile].AlbumName = this.Album.Text;
			openedFiles[currentFile].YearOfRelease = this.YearOfRelease.Date;
			openedFiles[currentFile].TrackNumber = (uint)this.TrackNumber.Value;
			openedFiles[currentFile].AlbumArt = (BitmapImage)this.Art.Source;

		}


		private void OpenImageButton_Click(object sender, RoutedEventArgs e)
		{
			OpenImage();
		}

		private void SaveFileButton_Click(object sender, RoutedEventArgs e)
		{
			SaveFile();
		}

		private void WelcomeButton_Click(object sender, RoutedEventArgs e)
		{

			OpenFile();
		}

		private void OpenDifferentButton_Click(object sender, RoutedEventArgs e)
		{
			OpenFile();
		}




	}
}
