using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagEditor.Files
{
    public sealed partial class SongListElement : UserControl
    {
        public SongListElement(MainWindow mainWindow)
        {
            this.InitializeComponent();
			this.DataContext = this;
			this._mainWindow = mainWindow;
		}

		private MainWindow _mainWindow;

		private void SongListElementButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.SetCurrentFile(FilePath);
		}


		public static readonly DependencyProperty AlbumArtSourceProperty =
			DependencyProperty.Register("AlbumArtSource", typeof(BitmapImage), typeof(SongListElement), new PropertyMetadata(null));

		public BitmapImage AlbumArtSource
		{
			get { return (BitmapImage)GetValue(AlbumArtSourceProperty); }
			set { SetValue(AlbumArtSourceProperty, value); }
		}

		public static readonly DependencyProperty SongNameProperty =
			DependencyProperty.Register("SongName", typeof(string), typeof(SongListElement), new PropertyMetadata(string.Empty));

		public string SongName
		{
			get { return (string)GetValue(SongNameProperty); }
			set { SetValue(SongNameProperty, value); }
		}

		public static readonly DependencyProperty ArtistNameProperty =
			DependencyProperty.Register("ArtistName", typeof(string), typeof(SongListElement), new PropertyMetadata(string.Empty));

		public string ArtistName
		{
			get { return (string)GetValue(ArtistNameProperty); }
			set { SetValue(ArtistNameProperty, value); }
		}

		public static readonly DependencyProperty FilePathProperty =
			DependencyProperty.Register("FilePath", typeof(string), typeof(SongListElement), new PropertyMetadata(string.Empty));

		public string FilePath
		{
			get { return (string)GetValue(FilePathProperty); }
			set { SetValue(FilePathProperty, value); }
		}
	}
}
