using Microsoft.UI.Xaml;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Graphics.Display;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Artsis.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // Maximizar la ventana principal
            MaximizeWindow();
        }

        private void MaximizeWindow()
        {
            var window = Application.Windows.FirstOrDefault()?.Handler?.PlatformView as Microsoft.UI.Xaml.Window;
            if (window != null)
            {
                var hwnd = WindowNative.GetWindowHandle(window);
                MaximizeWindow(hwnd);
            }
        }

        private void MaximizeWindow(IntPtr hwnd)
        {
            // PInvoke user32.dll to maximize the window
            ShowWindow(hwnd, ShowWindowCommands.Maximize);
        }

        // PInvoke ShowWindow from user32.dll
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        private enum ShowWindowCommands
        {
            Hide = 0,
            Normal = 1,
            Minimize = 6,
            Maximize = 3,
        }
    }

}
