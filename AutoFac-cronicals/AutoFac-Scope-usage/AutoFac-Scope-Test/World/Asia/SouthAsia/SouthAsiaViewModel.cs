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
        public RootFactory VMFactory { get; private set; }
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
        

        public SouthAsiaViewModel(RootFactory vmFactory)
        {
            LoadEuropeCommand = new RelayCommand((obj) => DoSomeThing());
            VMFactory = vmFactory;

            timer.Elapsed += (timerSender_, timerEvent_) => OnTimerElapsed(); ;
            timer.AutoReset = false;
            timer.Interval = 3000;
            timer.Start();

        }
        private void OnTimerElapsed()
        {
            Debug.Write(DateTime.Now.Ticks);
            Debug.WriteLine(" > SouthAsiaViewModel:" + this.GetHashCode().ToString());
            Debug.WriteLine(" > VMFactory:" + VMFactory.GetHashCode().ToString()); 
        }
        private void DoSomeThing()
        {
            //GrandChield1ColorChangeEvent(new SolidColorBrush(Colors.Brown));
            var europeVM = VMFactory.CreateEuropeViewModel<EuropeViewModel>();
            var worldVM = VMFactory.CreateWorldViewModel<WorldViewModel>();
            worldVM.EuropeRegion = (EuropeViewModel)europeVM;
            Debug.WriteLine($" >>> worldVM :{worldVM.GetHashCode()}");

            Feedback = "Loaded!";

        }

        // Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}