using AutoFac_Scope.World;

namespace AutoFac_Scope.Main
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(WorldViewModel _WorldViewModel)
        {
            CurrentViewModel = _WorldViewModel;
            //CurrentViewModel = new WorldViewModel();
        }
        public WorldViewModel CurrentViewModel { get; set; }

    }
}
