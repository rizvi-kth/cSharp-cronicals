using Autofac;
using Autofac.Core;
using AutoFac_Scope.Main;
using AutoFac_Scope.World;
using AutoFac_Scope.World.Europe;
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
            //builder.RegisterType<ServiceProvider>().As<IServiceProvider>().SingleInstance();            
            builder.RegisterType<ViewModelFactory>().InstancePerLifetimeScope();  // Singleton in the scope
            builder.Register<Func<WorldViewModel>>((context) => { return ViewModelFactory.GetViewModel<WorldViewModel>; });
            builder.Register<Func<EuropeViewModel>>((context) => { return ViewModelFactory.GetViewModel<EuropeViewModel>; });

            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<World.WorldViewModel>().InstancePerLifetimeScope(); // Avoid Singletone and use Lifetime scope
            builder.RegisterType<World.Asia.AsiaViewModel>();
            builder.RegisterType<World.Asia.SouthAsia.SouthAsiaViewModel>();
            builder.RegisterType<World.Europe.EuropeViewModel>();
            builder.RegisterType<World.Europe.EastEurope.EastEuropeViewModel>();
            //builder.RegisterType<BaseDependency>().As<IDependency>().SingleInstance();
            var rootScope = builder.Build();

            MainWindow mw = new MainWindow();

            Stopwatch watch = new Stopwatch();
            watch.Start();
                                    
            // Factory on Root Scope but takes Child Scope
            var childScope = rootScope.BeginLifetimeScope();
            Parameter[] param = new Parameter[] { new NamedParameter("scope", childScope) };
            ViewModelFactory fac = rootScope.Resolve<ViewModelFactory>(param);

            // MainWindowViewModel on Root Scope
            MainWindowViewModel _mainWindowViewModel = rootScope.Resolve<MainWindowViewModel>();

            // WorldViewModel on Child Scope
            var WorldVM = fac.GetViewModelIns<WorldViewModel>();
            Debug.WriteLine($"WorldVM : {WorldVM.GetHashCode()}");
            _mainWindowViewModel.CurrentViewModel = WorldVM;
            
            mw.DataContext = _mainWindowViewModel;
            mw.Show();

            watch.Stop();
            TimeSpan timeSpan = watch.Elapsed;
            Debug.WriteLine("Time: {0}h {1}m {2}s {3}ms", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

    }
}
