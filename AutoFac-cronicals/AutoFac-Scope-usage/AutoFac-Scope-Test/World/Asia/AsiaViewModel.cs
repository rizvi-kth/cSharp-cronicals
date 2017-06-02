using AutoFac_Scope.World.Asia.SouthAsia;

namespace AutoFac_Scope.World.Asia
{
    public class AsiaViewModel:AsiaViewModelBase
    {
        public AsiaViewModel(SouthAsiaViewModel _SouthAsiaViewModel)
        {
            SouthAsiaRegion = _SouthAsiaViewModel;
        }

        public SouthAsiaViewModel SouthAsiaRegion { get; set; }

    }

    
}