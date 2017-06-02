namespace ControllerAndWorkspacesExample.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Workspaces;

    public sealed class MainController : BaseController<MainViewModel>
    {
        private readonly IEnumerable<IWorkspaceDescriptor> _workspaceDescriptors;
        private readonly IDisposable _requestedWorkspaceDisposable;

        public MainController(MainViewModel viewModel, IEnumerable<IWorkspaceDescriptor> workspaceDescriptors)
            : base(viewModel)
        {
            _workspaceDescriptors = workspaceDescriptors;
        
            var availableWorkspaces = _workspaceDescriptors.OrderBy(x => x.Position)
                .Select(x => x.Name)
                .ToList();

            availableWorkspaces.Insert(0, string.Empty);

            ViewModel.AddAvailableWorkspaces(availableWorkspaces);

            _requestedWorkspaceDisposable = viewModel.RequestedWorkspaceStream
                .Subscribe(CreateWorkspace);
        }
        
        public override void Dispose()
        {
            base.Dispose();

            _requestedWorkspaceDisposable.Dispose();
        }

        private void CreateWorkspace(string requestedWorkspace)
        {
            var workspace = ViewModel.CurrentWorkspace;

            var newWorkspace = string.IsNullOrEmpty(requestedWorkspace) 
                ? null
                : _workspaceDescriptors.Single(x => x.Name == requestedWorkspace).CreateWorkspace();

            ViewModel.CurrentWorkspace = newWorkspace;

            if (workspace != null)
            {
                workspace.Dispose();
            }
        }
    }
}