namespace ControllerAndWorkspacesExample
{
    using System;

    public abstract class ControllerBase<T> : IDisposable, IController<T> where T : ViewModelBase
    {
        public abstract void Register(T viewModel);

        public virtual void Dispose()
        {
        }
    }
}