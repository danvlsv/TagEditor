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
		public MainWindow()
		{
			this.SetTitleBar(AppTitleBar);
			//this.MinHeight = 550;
			//this.MinWidth =850;
			this.Height = 550;
			this.Width = 730;
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

		private async void OpenFile()
		{
			//myButton.Content = "Clicked";
			var filePicker = new FileOpenPicker();
			filePicker.FileTypeFilter.Add(".mp3");

			//openPicker.FileTypeFilter.Add(".jpeg");
			//openPicker.FileTypeFilter.Add(".png");
			// Get the current window's HWND by passing in the Window object
			var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);

			// Associate the HWND with the file picker
			WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

			// Use file picker like normal!
			//filePicker.FileTypeFilter.Add("*");
			var file = await filePicker.PickSingleFileAsync();
			if (file != null)
			{
				var tfile = TagLib.File.Create(file.Path);
				this.Welcome.Visibility = Visibility.Collapsed;
				this.Editor.Visibility = Visibility.Visible;


				this.SongTitle.Text = tfile.Tag.Title;


				string artists = "";
				string[] artistsFromFile = tfile.Tag.Performers;
				for (int i = 0; i < artistsFromFile.Length; i++)
				{
					artists += artistsFromFile[i];
					if (i != artistsFromFile.Length - 1)
					{
						artists += "; ";
					}
				}
				this.Artist.Text = artists;


				this.Album.Text = tfile.Tag.Album;

				Debug.Print(tfile.Tag.Year.ToString());
				if (tfile.Tag.Year != 0)
				{
					TimeSpan ts = new(0, 0, 0);
					this.YearOfRelease.Date = new DateTimeOffset((int)tfile.Tag.Year, 1, 1, 1, 0, 0, ts);
				}
				


				this.DiscNumber.Value = tfile.Tag.Disc;

				bool pictureFound = true;
				using (InMemoryRandomAccessStream ms = new())
				{
					using (DataWriter writer = new(ms.GetOutputStreamAt(0)))
					{
						try
						{
							writer.WriteBytes(tfile.Tag.Pictures[0].Data.Data);
							writer.StoreAsync().GetResults();
						}
						catch
						{
							pictureFound = false;
							this.Art.Source = null;
						}
					}
					if (pictureFound)
					{
						var image = new BitmapImage();
						image.SetSource(ms);
						this.Art.Source = image;
					}
					
				}

				this.MusicPlayer.Source = MediaSource.CreateFromStorageFile(file);
				this.FilePath.Text = file.Path.ToString();
				

			}
			else
			{
				ElementSoundPlayer.Play(ElementSoundKind.GoBack);
			}

		}


	}


}
