namespace ControllerAndWorkspacesExample.Workspaces.Another
{
    using System;

    using Autofac;

    using Services;

    public sealed class WorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/controllerAndWorkspacesExample;component/workspaces/another/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;

        public WorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public string Name { get { return "Another"; } }

        public Workspace CreateWorkspace()
        {
            return _workspaceFactory.Create<Registrar, ViewModel>(_resources);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder container)
            {
                container.RegisterType<ViewModel>();
                container.RegisterType<Controller>().As<IController<ViewModel>>();

                container.RegisterType<UniqueService>().As<IUniqueService>().InstancePerLifetimeScope();
            }
        }
    }
}