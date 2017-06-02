namespace ControllerAndWorkspacesExample.Workspaces.AnotherExample
{
    using System;

    using Autofac;

    public sealed class AnotherExampleWorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/controllerAndWorkspacesExample;component/workspaces/anotherexample/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;

        public AnotherExampleWorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public int Position { get { return 1; } }

        public string Name { get { return "Another Example Workspace"; } }

        public Workspace CreateWorkspace()
        {
            return _workspaceFactory.Create<Registrar, AnotherExampleController>(_resources);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder container)
            {
                container.RegisterType<AnotherExampleViewModel>();
                container.RegisterType<AnotherExampleController>();
            }
        }
    }
}