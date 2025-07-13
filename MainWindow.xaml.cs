using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TagEditor
{
	/// <summary>
	/// An empty window that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainWindow : WinUIEx.WindowEx
	{
		private const double DefaultMinHeight = 550;
		private const double DefaultMinWidth = 900;
		private const double DefaultHeight = 550;
		private const double DefaultWidth = 900;

		public MainWindow()
		{
			this.InitializeComponent();
			this.SetTitleBar(AppTitleBar);
			this.MinHeight = DefaultMinHeight;
			this.MinWidth = DefaultMinWidth;
			this.Height = DefaultHeight;
			this.Width = DefaultWidth;

			this.SystemBackdrop = new DesktopAcrylicBackdrop();

			ElementSoundPlayer.State = ElementSoundPlayerState.Off;
		}
	}
}
