using Autofac;
using AutoFac_Scope.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Core;

namespace AutoFac_Scope
{   

    public interface IFactory
    {
        ILifetimeScope Scope { get; set; }
        T GetViewModel<T>(Parameter[] parameter);        
        T GetViewModel<T>();        
    }

    public class ViewModelFactory //:IFactory
    {
        // These static property and method is used by Autofac
        // for delegate-factory registration.
        public static ILifetimeScope Scope { get; set; }
        public static T GetViewModel<T>()
        {
            if (Scope == null)
                throw new ArgumentNullException("Scope is null");
            return Scope.Resolve<T>();
        }

        public ViewModelFactory(ILifetimeScope scope)
        {
            Scope = scope;
        }
        
        public T GetViewModel<T>(Parameter[] parameter)
        {
            return Scope.Resolve<T>(parameter);
        }

        public T GetViewModelIns<T>()
        {
            return Scope.Resolve<T>();            
        }

        internal static void DisposeScope()
        {
            
            Scope.Dispose();
        }
    }


}
