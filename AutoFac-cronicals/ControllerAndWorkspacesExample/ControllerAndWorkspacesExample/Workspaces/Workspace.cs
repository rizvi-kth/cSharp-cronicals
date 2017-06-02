namespace ControllerAndWorkspacesExample.Workspaces
{
    using System;

    public sealed class Workspace
    {
        private readonly BaseController _controller;
        private readonly Action _dispose;

        public Workspace(BaseController controller, Uri resources, Action dispose)
        {
            Resources = resources;

            _controller = controller;
            _dispose = dispose;
        }

        public BaseViewModel Content { get { return _controller.ViewModel; } }

        public Uri Resources { get; private set; }

        public void Dispose()
        {
            if (_dispose != null)
            {
                _controller.Dispose();
                _dispose();
            }
        }
    }
}