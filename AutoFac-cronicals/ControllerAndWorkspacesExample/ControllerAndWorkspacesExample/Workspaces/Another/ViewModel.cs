namespace ControllerAndWorkspacesExample.Workspaces.Another
{
    using controllerAndWorkspacesExample;

    public sealed class ViewModel : ViewModelBase
    {
        public ViewModel(IController<ViewModel> controller)
        {
            controller.Register(this);
        }
    }
}