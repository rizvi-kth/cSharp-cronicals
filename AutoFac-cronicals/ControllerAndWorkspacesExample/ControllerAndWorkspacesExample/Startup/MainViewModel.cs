namespace ControllerAndWorkspacesExample.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Subjects;

    using Workspaces;

    public sealed class MainViewModel : BaseViewModel, IDisposable
    {
        private readonly ObservableCollection<string> _availableWorkspaces;
        private readonly Subject<string> _requestedWorkspaceStream;

        private string _requestedWorkspace;
        private ReadOnlyObservableCollection<string> _workspaces;
        private Workspace _currentWorkspace;
        
        public MainViewModel()
        {
            _availableWorkspaces = new ObservableCollection<string>();
            _requestedWorkspaceStream = new Subject<string>();
        }

        public ReadOnlyObservableCollection<string> AvailableWorkspaces { get { return _workspaces ?? (_workspaces = new ReadOnlyObservableCollection<string>(_availableWorkspaces)); } }

        public string RequestedWorkspace
        {
            get
            {
                return _requestedWorkspace;
            }

            set
            {
                using (SuspendNotifications())
                {
                    if (SetPropertyAndNotify(ref _requestedWorkspace, value))
                    {
                        _requestedWorkspaceStream.OnNext(_requestedWorkspace);
                    }
                }
            }
        }

        public Workspace CurrentWorkspace
        {
            get
            {
                return _currentWorkspace;
            }

            set
            {
                SetPropertyAndNotify(ref _currentWorkspace, value);
            }
        }
        
        public IObservable<string> RequestedWorkspaceStream { get { return _requestedWorkspaceStream; } }

        public void AddAvailableWorkspaces(IEnumerable<string> workspaces)
        {
            using (SuspendNotifications())
            {
                foreach (var workspace in workspaces)
                {
                    _availableWorkspaces.Add(workspace);
                }

                RequestedWorkspace = _availableWorkspaces.First();
            }
        }

        public void Dispose()
        {
            _requestedWorkspaceStream.Dispose();
        }
    }
}