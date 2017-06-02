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

    public class ViewModelFactory :IFactory
    {
        public ILifetimeScope Scope { get; set; }
        public ViewModelFactory(ILifetimeScope scope)
        {
            Scope = scope;
        }
        
        public T GetViewModel<T>(Parameter[] parameter)
        {
            return Scope.Resolve<T>(parameter);
        }

        public T GetViewModel<T>()
        {
            return Scope.Resolve<T>();
        }
    }


}
