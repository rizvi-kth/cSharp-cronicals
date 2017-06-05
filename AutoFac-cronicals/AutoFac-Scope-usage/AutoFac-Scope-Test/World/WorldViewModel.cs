using AutoFac_Scope.World.Asia;
using AutoFac_Scope.World.Europe;
using System;
using System.ComponentModel;

namespace AutoFac_Scope.World
{
    public class WorldViewModel : INotifyPropertyChanged
    {
        // 2. This the Factory-method
        public Func<EuropeViewModel> Factory { get; set; }
        // 1. Client (SouthAsiaViewModel) should depend on a Factory.
        // AutoFac Delegate-Factory is used to resolve dependency.
        public WorldViewModel(AsiaViewModel _AsiaViewModel, Func<EuropeViewModel> factory)
        {            
            AsiaRegion = _AsiaViewModel;
            Factory = factory;            
        }

        public AsiaViewModel AsiaRegion { get; set; }

        private EuropeViewModel _EuropeRegion;
        public EuropeViewModel EuropeRegion {
            get { return _EuropeRegion; }
            set { _EuropeRegion = value; OnPropertyChanged("EuropeRegion"); }
        }

        public void LoadEurope()
        {
            EuropeRegion = Factory.Invoke();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}