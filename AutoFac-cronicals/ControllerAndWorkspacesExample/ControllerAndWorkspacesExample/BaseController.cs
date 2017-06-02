namespace ControllerAndWorkspacesExample
{
    using System;

    public abstract class BaseController : IDisposable
    {
        protected BaseController(BaseViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public BaseViewModel ViewModel { get; private set; }

        public virtual void Dispose()
        {
            
        }
    }

    public abstract class BaseController<TViewModel> : BaseController where TViewModel : BaseViewModel
    {
        protected BaseController(TViewModel viewModel)
            : base(viewModel)
        {
        }

        public new TViewModel ViewModel { get { return (TViewModel)base.ViewModel; } }
    }
}