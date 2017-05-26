using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
            MainWindow mw = new MainWindow();
            mw.DataContext = new MainWindowViewModel();
            mw.Show();

            //ParentViewModel pvm = new ParentViewModel();
            //Chield1ViewModel pvm = new Chield1ViewModel();
            //Chield2ViewModel pvm = new Chield2ViewModel();
            //GrandChield1ViewModel pvm = new GrandChield1ViewModel();

        }

    }
}
