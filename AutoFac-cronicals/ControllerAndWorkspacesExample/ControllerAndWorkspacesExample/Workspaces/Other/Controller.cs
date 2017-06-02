namespace ControllerAndWorkspacesExample.Workspaces.Other
{
    using controllerAndWorkspacesExample;

    using ControllerAndWorkspacesExample.Services;

    public sealed class Controller : ControllerBase<ViewModel>
    {
        private readonly IUniqueService _uniqueService;

        public Controller(IUniqueService uniqueService)
        {
            _uniqueService = uniqueService;
        }

        public override void Register(ViewModel viewModel)
        {
        }
    }
}