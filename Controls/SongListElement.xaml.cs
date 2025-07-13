using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using TagEditor.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagEditor.Files
{
    public sealed partial class SongListElement : UserControl
    {

		private AudioFile thisFile;

        public SongListElement(Editor editor, AudioFile audioFile)
        {
            this.InitializeComponent();
			this.DataContext = audioFile;
			thisFile = audioFile;
			this._editor = editor;
		}

		private readonly Editor _editor;

		private void SongListElementButton_Click(object sender, RoutedEventArgs e)
		{
			_editor.SetCurrentFile(thisFile.FilePath);
			
		}

		private void SongListElementCloseButton_Click(object sender, RoutedEventArgs e)
		{
			_editor.CloseFile(thisFile.FilePath);
		}

	}
}
