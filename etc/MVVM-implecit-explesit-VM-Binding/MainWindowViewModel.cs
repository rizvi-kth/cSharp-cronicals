using System.CodeDom;
using System.Windows.Input;
using MVVM.Customer;
using MVVM.Order;
using MVVM.View.ViewModel;

namespace MVVM
{
    public class MainWindowViewModel :BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public MainWindowViewModel()
        {
            CurrentViewModel = new CustomerListViewModel();
            LoadCustomerView = new RelayCommand( (o) => { CurrentViewModel = new CustomerListViewModel(); }, (o) => true);
            LoadOrderView = new RelayCommand((o) => { CurrentViewModel = new OrderListViewModel(); }, (o) => true);
        }

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            private set {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadCustomerView { get; private set; } 
        public ICommand LoadOrderView { get; private set; }



    }
}