using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media; // Используйте это пространство имен
using System.Windows.Media; // Удалите, если не нужно
using TagEditor.Controls;
using Windows.UI.ViewManagement;

namespace TagEditor.Files
{
	public sealed partial class SongListElement : UserControl
	{
		private AudioFile thisFile;
		private UISettings uiSettings = new UISettings();
		private Windows.UI.Color accentColor;
		private readonly Editor _editor;

		public SongListElement(Editor editor, AudioFile audioFile)
		{
			this.InitializeComponent();
			this.DataContext = audioFile;
			thisFile = audioFile;
			this._editor = editor;
			uiSettings.ColorValuesChanged += ColorValuesChanged;
		}

		private void ColorValuesChanged(UISettings sender, object args)
		{
			accentColor = sender.GetColorValue(UIColorType.Accent);
		}

		private void SongListElementButton_Click(object sender, RoutedEventArgs e)
		{
			_editor.SetCurrentFile(thisFile.FilePath);
		}

		private void SongListElementCloseButton_Click(object sender, RoutedEventArgs e)
		{
			_editor.CloseFile(thisFile.FilePath);
		}

		public void SetBorderColor(bool active)
		{
			this.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(accentColor);
		}
	}
}
