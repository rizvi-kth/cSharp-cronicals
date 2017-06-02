namespace ControllerAndWorkspacesExample.Workspaces.Example
{
    public sealed class ExampleController : BaseController<ExampleViewModel>
    {
        public ExampleController(ExampleViewModel viewModel) : base(viewModel)
        {
        }
    }
}