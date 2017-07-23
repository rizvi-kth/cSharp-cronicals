using System;
using System.Windows.Input;
using System.Windows.Media;
using MVVM_Event_Bubble_in_multilevel_miewmodel.View.ViewModel;


namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class GrandChield1ViewModel
    {
        public ICommand ChangeColorCommand { get; private set; }

        public GrandChield1ViewModel()
        {
            ChangeColorCommand = new RelayCommand((obj)=> DoSomeThing() );
        }

        // CREATE AN EVENT IN GRAND-CHILD
        public event Action<SolidColorBrush> GrandChield1ColorChangeEvent = delegate { };
        // RAISE A EVENT FROM GRAND-CHILD WHICH WILL BE HANDLED BY CHILD VIEWMODEL.
        // (Much like a real baby raises event(cry) that are taken care of(feed the baby) or disregard by the parent)
        private void DoSomeThing()
        {
            GrandChield1ColorChangeEvent(new SolidColorBrush(Colors.Brown));
        }

    }
}