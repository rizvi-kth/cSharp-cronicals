namespace ControllerAndWorkspacesExample
{
    public interface IController<in T> where T : ViewModelBase
    {
        void Register(T viewModel);
    }
}
