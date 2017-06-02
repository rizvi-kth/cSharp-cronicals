namespace ControllerAndWorkspacesExample
{
    using System.Windows;

    using Startup;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            var mainWindow = new MainWindow
            {
                DataContext = BootStrapper.RootVisual
            };

            mainWindow.Show();
        }
    }
}
