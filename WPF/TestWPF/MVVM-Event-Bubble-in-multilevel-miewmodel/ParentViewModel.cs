using System.Drawing;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;

namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class ParentViewModel
    {
        public ParentViewModel()
        {
            LeftView = new Chield1ViewModel();
            LeftView.Chield1ColorChangeEvent += doColorChange;
            
            RightView = new Chield2ViewModel();
        }

        // Parent viewmodel can directly call a child-viewmodel method.
        private void doColorChange(SolidColorBrush col)
        {
            RightView.setMyColor(col);
        }

        public Chield1ViewModel LeftView { get; set; }
        public Chield2ViewModel RightView { get; set; }
    }
}