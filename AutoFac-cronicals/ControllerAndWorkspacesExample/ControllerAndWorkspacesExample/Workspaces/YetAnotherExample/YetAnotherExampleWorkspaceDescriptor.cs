namespace ControllerAndWorkspacesExample.Workspaces.YetAnotherExample
{
    using System;

    using Autofac;

    public sealed class YetAnotherExampleWorkspaceDescriptor : IWorkspaceDescriptor
    {
        private readonly Uri _resources = new Uri("/controllerAndWorkspacesExample;component/workspaces/yetanotherexample/resources.xaml", UriKind.RelativeOrAbsolute);

        private readonly WorkspaceFactory _workspaceFactory;

        public YetAnotherExampleWorkspaceDescriptor(WorkspaceFactory workspaceFactory)
        {
            _workspaceFactory = workspaceFactory;
        }

        public int Position { get { return 2; } }

        public string Name { get { return "Yet Another Example Workspace"; } }

        public Workspace CreateWorkspace()
        {
            return _workspaceFactory.Create<Registrar, YetAnotherExampleController>(_resources);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private sealed class Registrar
        {
            // Scoped registrations, these will be thrown away when the workspace is disposed...
            public Registrar(ContainerBuilder container)
            {
                container.RegisterType<YetAnotherExampleViewModel>();
                container.RegisterType<YetAnotherExampleController>();
            }
        }
    }
}