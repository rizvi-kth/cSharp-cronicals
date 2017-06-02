namespace ControllerAndWorkspacesExample.Workspaces.SystemInfo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Subjects;
    using System.Text.RegularExpressions;
    using System.Windows.Media.Imaging;

    public sealed class ViewModel : ViewModelBase
    {
        private readonly List<string> _internalGroups;
        private readonly ReplaySubject<string> _selectedGroupStream;
        private readonly ObservableCollection<NameValuePair> _internalValues;

        private ReadOnlyCollection<string> _groups;
        private ReadOnlyObservableCollection<NameValuePair> _values;
        private string _selectedGroup;
        private BitmapImage _dilbert;

        public sealed class NameValuePair
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public NameValuePair(string name, string value)
            {
                Name = Regex.Replace(name, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
                Value = value;
            }
        }
        
        public ViewModel(IController<ViewModel> controller)
        {
            _internalValues = new ObservableCollection<NameValuePair>();
            _internalGroups = new List<string> { "Hardware", "Software" };
            _selectedGroupStream = new ReplaySubject<string>(1);

            SelectedGroup = Groups.First();

            controller.Register(this);
        }

        public ReadOnlyObservableCollection<NameValuePair> Values { get { return _values ?? (_values = new ReadOnlyObservableCollection<NameValuePair>(_internalValues)); } }

        public ReadOnlyCollection<string> Groups { get { return _groups ?? (_groups = new ReadOnlyCollection<string>(_internalGroups)); } }
        
        public string SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                if (SetPropertyAndNotify(ref _selectedGroup, value))
                {
                    _selectedGroupStream.OnNext(_selectedGroup);
                }
            }
        }

        public BitmapImage Dilbert
        {
            get
            {
                return _dilbert;
            }

            private set
            {
                SetPropertyAndNotify(ref _dilbert, value);
            }
        }

        public IObservable<string> SelectedGroupStream { get { return _selectedGroupStream;} }

        public void ClearValues()
        {
            _internalValues.Clear();
        }

        public void AddValues(IEnumerable<NameValuePair> nameValuePairs)
        {
            foreach (var nameValuePair in nameValuePairs)
            {
                _internalValues.Add(nameValuePair);
            }
        }

        public void UpdateDilbert(string filePath)
        {
            Dilbert = new BitmapImage(new Uri(filePath));
        }
    }
}