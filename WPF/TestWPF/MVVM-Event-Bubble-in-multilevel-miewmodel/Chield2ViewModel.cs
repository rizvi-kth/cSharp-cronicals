using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using MVVM_Event_Bubble_in_multilevel_miewmodel.Annotations;
using Color = System.Drawing.Color;

namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class Chield2ViewModel:INotifyPropertyChanged
    {
        private SolidColorBrush _bgColor;

        public Chield2ViewModel()
        {
            _bgColor = new SolidColorBrush(Colors.DarkRed); 
        }

        public SolidColorBrush BGColor
        {
            get { return _bgColor; }
            set { _bgColor = value; OnPropertyChanged(); }
        }

        // This method is called by the parent and 
        // the request for the call came to parent from its grand-child using event chaining.
        public void setMyColor(SolidColorBrush col)
        {
            BGColor = col;
        }


        // INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        // INotifyPropertyChanged implementation
    }
}