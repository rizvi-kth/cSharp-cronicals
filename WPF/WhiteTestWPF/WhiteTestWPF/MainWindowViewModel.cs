using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteTestWPF
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            CurrentParentViewModel = new ParentViewModel();
        }
        public ParentViewModel CurrentParentViewModel { get; set; }
    }
}
