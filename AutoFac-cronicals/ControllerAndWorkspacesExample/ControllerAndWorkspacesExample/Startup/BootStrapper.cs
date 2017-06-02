namespace ControllerAndWorkspacesExample.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Autofac;
    using Autofac.Core;
    using Workspaces;

    public static class BootStrapper
    {
        private static ILifetimeScope rootScope;
        private static MainController rootController;

        public static BaseViewModel RootVisual
        {
            get
            {
                if (rootScope == null)
                {
                    Run();
                }

                rootController = rootScope.Resolve<MainController>();
                return rootController.ViewModel;
            }
        }
        
        private static void Run()
        {
            if (rootScope != null)
            {
                return;
            }

            var builder = new ContainerBuilder();

            // Register the application 'chrome' stuff...
            builder.RegisterType<MainViewModel>();
            builder.RegisterType<MainController>();
            
            // Register all the workspace descriptors in the assembly...
            foreach (var type in GetWorkspaceDescriptorTypes())
            {
                builder.RegisterType(type).As(typeof(IWorkspaceDescriptor));
            }

            builder.RegisterType<WorkspaceFactory>().InstancePerLifetimeScope();
            
            rootScope = builder.Build();
            rootScope.Resolve<WorkspaceFactory>(new Parameter[]{ new NamedParameter("scope", rootScope) });
        }

        private static IEnumerable<Type> GetWorkspaceDescriptorTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => typeof(IWorkspaceDescriptor).IsAssignableFrom(x) && x.IsClass);
        }
    }
}