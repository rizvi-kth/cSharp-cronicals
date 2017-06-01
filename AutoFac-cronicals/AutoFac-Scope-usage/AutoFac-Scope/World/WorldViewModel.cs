using AutoFac_Scope.World.Asia;
using AutoFac_Scope.World.Europe;
using System.ComponentModel;

namespace AutoFac_Scope.World
{
    public class WorldViewModel : INotifyPropertyChanged
    {
        //, EuropeViewModel _EuropeViewModel
        public WorldViewModel(AsiaViewModel _AsiaViewModel)
        {
            //AsiaRegion = new AsiaViewModel();
            AsiaRegion = _AsiaViewModel;

            //EuropeRegion = new EuropeViewModel();
            //EuropeRegion = _EuropeViewModel;
        }

        public AsiaViewModel AsiaRegion { get; set; }

        private EuropeViewModel _EuropeRegion;
        public EuropeViewModel EuropeRegion {
            get { return _EuropeRegion; }
            set { _EuropeRegion = value; OnPropertyChanged("EuropeRegion"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}