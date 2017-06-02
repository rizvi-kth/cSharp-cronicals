namespace ControllerAndWorkspacesExample.Workspaces.SystemInfo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    using ControllerAndWorkspacesExample.Services;

    using Dilbert;

    public sealed class Controller : ControllerBase<ViewModel>
    {
        private readonly ISystemInfoService _systemInfoService;
        private readonly IDailyDilbertService _dailyDilbertService;

        private ViewModel _viewModel;
        private IDisposable _selectedGroupDisposable;

        public Controller(ISystemInfoService systemInfoService, IDailyDilbertService dailyDilbertService)
        {
            _systemInfoService = systemInfoService;
            _dailyDilbertService = dailyDilbertService;
        }

        public override async void Register(ViewModel viewModel)
        {
            _viewModel = viewModel;

            _selectedGroupDisposable = _viewModel.SelectedGroupStream
                .Select(x => x == "Hardware" ? _systemInfoService.GetHardwareInfo() : _systemInfoService.GetSoftwareInfo())
                .Subscribe(SetSystemInfo);

            viewModel.UpdateDilbert(await _dailyDilbertService.DailyAsFileAsync());
        }
        
        public override void Dispose()
        {
            base.Dispose();

            if (_selectedGroupDisposable != null)
            {
                _selectedGroupDisposable.Dispose();
                _selectedGroupDisposable = null;
            }
        }

        private void SetSystemInfo(IDictionary<string, string> parameters)
        {
            var nameValuePairs = parameters.Select(x => new ViewModel.NameValuePair(x.Key, x.Value));

            _viewModel.ClearValues();
            _viewModel.AddValues(nameValuePairs);
        }
    }
}