namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class Chield1ViewModel
    {
        public Chield1ViewModel()
        {
            CenterView = new GrandChield1ViewModel();
        }
        public GrandChield1ViewModel CenterView { get; set; }

    }
}