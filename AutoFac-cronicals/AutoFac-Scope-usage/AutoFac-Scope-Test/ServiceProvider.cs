using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFac_Scope
{
    public class ServiceProvider : IServiceProvider
    {
        public IContainer AppContainer { get; set; }

        // This ServiceProvider will NOT be a service locator pattern as long as you 
        // 1. Dont share the container in a global static function
        // 2. Dont use the container as a dependency; rather If components 
        //    have a dependency on the container (or on a lifetime scope), 
        //    look at how they’re using the container to retrieve services, 
        //    and add those services to the component’s (dependency injected) 
        //    constructor arguments instead.
        // 3. Adding a new component dont force to add a new GetService function.
        // 4. GetService function should return abstruct types.  
        public T GetService<T>()
        {
            T service;
            using (var scope = AppContainer.BeginLifetimeScope())
            {
                service = scope.Resolve<T>();
            }
            // Debug Condition : 
            //if (service.GetType().Equals(typeof(AutoFac_Scope.Main.MainWindowViewModel)))
            //{
            //    var vm = service as AutoFac_Scope.Main.MainWindowViewModel;
            //    Debug.WriteLine($">> WorldViewModel: {vm.CurrentViewModel.GetHashCode()}") ;
            //}


            return service;
            //return AppContainer.Resolve<T>();

        }



    }
}
