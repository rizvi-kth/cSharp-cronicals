using System.Diagnostics;

namespace AutoFac_Scope.World.Europe.EastEurope
{
    public class EastEuropeViewModel
    {
        public System.Timers.Timer timer = new System.Timers.Timer(200);
        public EastEuropeViewModel()
        {
            timer.Elapsed += (timerSender_, timerEvent_) => OnTimerElapsed(); ;
            timer.AutoReset = true;
            timer.Interval = 3000;
            timer.Start();
        }
        private void OnTimerElapsed()
        {
            Debug.WriteLine("EastEuropeViewModel:" + this.GetHashCode().ToString());
        }
    }
}