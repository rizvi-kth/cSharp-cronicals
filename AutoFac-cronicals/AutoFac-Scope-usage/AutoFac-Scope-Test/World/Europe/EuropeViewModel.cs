using AutoFac_Scope.World.Europe.EastEurope;

namespace AutoFac_Scope.World.Europe
{
    public class EuropeViewModel : EuropeViewModelBase
    {
        public EuropeViewModel(EastEuropeViewModel _EastEuropeViewModel)
        {
            EastEuropeRegion = _EastEuropeViewModel;
        }

        public EastEuropeViewModel EastEuropeRegion { get; set; }

    }
}