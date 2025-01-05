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

		private AudioFile thisFile;

        public SongListElement(MainWindow mainWindow, AudioFile audioFile)
        {
            this.InitializeComponent();
			this.DataContext = audioFile;
			thisFile = audioFile;
			this._mainWindow = mainWindow;
		}

		private readonly MainWindow _mainWindow;

		private void SongListElementButton_Click(object sender, RoutedEventArgs e)
		{
			_mainWindow.SetCurrentFile(thisFile.FilePath);
		}


		
	}
}
