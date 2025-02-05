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


using Microsoft.UI.Xaml.Media.Imaging;
using System.Drawing.Imaging;
using Windows.Storage.Streams;
using Windows.Media.Core;
using TagEditor.Files;
using System.Collections;
using System.Windows.Input;


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

			this.SystemBackdrop = new DesktopAcrylicBackdrop();

			this.InitializeComponent();

			ElementSoundPlayer.State = ElementSoundPlayerState.On;
			//this.Editor.Visibility = Visibility.Collapsed;
			this.PersistenceId = "MainWindowSize";
		}

		//private void WelcomeButton_Click(object sender, RoutedEventArgs e)
		//{

		//	OpenFile();
		//}

		//private void OpenDifferentButton_Click(object sender, RoutedEventArgs e)
		//{
		//	OpenFile();
		//}




		

	}


}
