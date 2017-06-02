namespace ControllerAndWorkspacesExample.Workspaces.Recursive
{
    using System;

    using Autofac;

    using Startup;
    using Workspaces;

    public sealed class RecursiveWorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/controllerAndWorkspacesExample;component/startup/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;
        
        public RecursiveWorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public int Position { get { return 3; } }

        public string Name { get { return "Recursive Example Workspaces"; } }

        public Workspace CreateWorkspace()
        {
            return _workspaceFactory.Create<Registrar, MainController>(_resources);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder builder)
            {
                // Register the application 'chrome' stuff...
                builder.RegisterType<MainViewModel>();
                builder.RegisterType<MainController>();
            }
        }
    }
}