using AutoFac_Scope.World.Europe;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;

namespace AutoFac_Scope.World.Asia.SouthAsia
{
    public class SouthAsiaViewModel : INotifyPropertyChanged
    {
        public ICommand LoadEuropeCommand { get; private set; }
        public IFactory VMFactory { get; private set; }
        public System.Timers.Timer timer = new System.Timers.Timer(200);

        private string _Feedback;
        public string Feedback {
            get {
                return _Feedback;
            }
            set{
                _Feedback = value;
                OnPropertyChanged("Feedback");
            }
        }

        // 2. This the Factory-method
        Func<WorldViewModel> Factory { get; set; }
        // 1. Client (SouthAsiaViewModel) should depend on a Factory.
        // AutoFac Delegate-Factory is used to resolve dependency.
        public SouthAsiaViewModel(Func<WorldViewModel> factory)//IFactory MyCreatorFactory
        {
            LoadEuropeCommand = new RelayCommand((obj) => DoSomeThing());
            Factory = factory;

            timer.Elapsed += (timerSender_, timerEvent_) => OnTimerElapsed(); ;
            timer.AutoReset = false;
            timer.Interval = 3000;
            timer.Start();

        }
        private void OnTimerElapsed()
        {
            Debug.Write(DateTime.Now.Ticks);
            Debug.WriteLine(" > SouthAsiaViewModel:" + this.GetHashCode().ToString());
            
            var WorldVM = Factory.Invoke();            
            Debug.WriteLine(" > WorldVM in SouthAsiaVM:" + WorldVM.GetHashCode().ToString());
            

        }
        private void DoSomeThing()
        {
            //GrandChield1ColorChangeEvent(new SolidColorBrush(Colors.Brown));
            //var europeVM = VMFactory.GetViewModel<EuropeViewModel>();
            //var worldVM = VMFactory.GetViewModel<WorldViewModel>();
            //worldVM.EuropeRegion = (EuropeViewModel)europeVM;
            //Debug.WriteLine($" >>> worldVM :{worldVM.GetHashCode()}");

            var WorldVM = Factory.Invoke();
            WorldVM.LoadEurope();
            Feedback = "Not Loaded!";

        }

        // Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}