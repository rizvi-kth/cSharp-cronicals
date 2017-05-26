using System;
using System.Drawing;
using System.Windows.Media;

namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class Chield1ViewModel
    {
        public Chield1ViewModel()
        {
            CenterView = new GrandChield1ViewModel();
            CenterView.GrandChield1ColorChangeEvent += doColorChange;
        }

        // CREATE AN EVENT IN CHILD
        public event Action<SolidColorBrush> Chield1ColorChangeEvent = delegate { };
        // RAISE A EVENT FROM CHILD WHICH WILL BE HANDLED BY PARENT VIEWMODEL.
        // (Much like a real baby raises event(cry) that are taken care of(feed the baby) or disregard by the parent)
        private void doColorChange(SolidColorBrush col)
        {
            Chield1ColorChangeEvent(col);
        }

        public GrandChield1ViewModel CenterView { get; set; }

    }
}