using AutoFac_Scope.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFac_Scope
{
    public class RootFactory
    {
        IServiceProvider ServiceLocator;
        public RootFactory(IServiceProvider serviceloc)
        {
            ServiceLocator = serviceloc;
        }

        public EuropeViewModelBase CreateEuropeViewModel<T>()
            where T : EuropeViewModelBase
        {
            return ServiceLocator.GetService<T>();
        }

        public WorldViewModel CreateWorldViewModel<T>()
            where T : WorldViewModel
        {
            return ServiceLocator.GetService<T>();
        }
    }
}
