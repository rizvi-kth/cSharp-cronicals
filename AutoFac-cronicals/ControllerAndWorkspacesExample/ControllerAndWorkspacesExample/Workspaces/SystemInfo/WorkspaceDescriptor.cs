namespace ControllerAndWorkspacesExample.Workspaces.SystemInfo
{
    using System;

    using Autofac;

    using Services;

    using Workspaces;

    using Dilbert;

    public sealed class WorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/controllerAndWorkspacesExample;component/workspaces/systeminfo/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;
        
        public WorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public string Name { get { return "System Info"; } }

        public Workspace CreateWorkspace()
        {
            var workspace = _workspaceFactory.Create<Registrar, ViewModel>(_resources);
            
            return workspace;
        }
        
        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder container)
            {
                container.RegisterType<ViewModel>();
                container.RegisterType<Controller>().As<IController<ViewModel>>();

                container.RegisterType<SystemInfoService>().As<ISystemInfoService>();
                container.RegisterType<DailyDilbertService>().As<IDailyDilbertService>();
            }
        }
    }
}