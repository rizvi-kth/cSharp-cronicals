using System.Windows.Forms.VisualStyles;

namespace MVVM_Event_Bubble_in_multilevel_miewmodel
{
    public class ParentViewModel
    {
        public ParentViewModel()
        {
            LeftView = new Chield1ViewModel();
            RightView = new Chield2ViewModel();
        }

        public Chield1ViewModel LeftView { get; set; }
        public Chield2ViewModel RightView { get; set; }
    }
}