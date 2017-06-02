using Autofac;
using AutoFac_Scope.Main;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoFac_Scope
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs startupEventArgs)
        {
                        
            var builder = new ContainerBuilder();
            builder.RegisterType<ServiceProvider>().As<IServiceProvider>().SingleInstance();
            builder.RegisterType<RootFactory>();
            builder.RegisterType<MainWindowViewModel>();
            builder.RegisterType<World.WorldViewModel>().SingleInstance(); // Avoid Singletone and use Lifetime scope
            builder.RegisterType<World.Asia.AsiaViewModel>();
            builder.RegisterType<World.Asia.SouthAsia.SouthAsiaViewModel>();
            builder.RegisterType<World.Europe.EuropeViewModel>();
            builder.RegisterType<World.Europe.EastEurope.EastEuropeViewModel>();
            //builder.RegisterType<BaseDependency>().As<IDependency>().SingleInstance();
            var appContainer = builder.Build();

            MainWindow mw = new MainWindow();

            Stopwatch watch = new Stopwatch();
            watch.Start();
            //mw.DataContext = appContainer.Resolve(typeof(MainWindowViewModel));
            //using (var scope = appContainer.BeginLifetimeScope())
            //{
            //    mw.DataContext = scope.Resolve(typeof(MainWindowViewModel));
            //}

            IServiceProvider serviceProvider = appContainer.Resolve<IServiceProvider>();
            serviceProvider.AppContainer = appContainer;
            mw.DataContext = serviceProvider.GetService<MainWindowViewModel>();

            mw.Show();

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;
            Debug.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);

        }

    }
}
