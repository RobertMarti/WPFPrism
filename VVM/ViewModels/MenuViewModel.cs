using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SimpleHmi.Infrastructure;
using VVM.Views.AsyncAwait;
using MainWindow = VVM.Views.SimpleHmi.MainWindow;

namespace VVM.ViewModels
{
    class MenuViewModel : BindableBase
    {
        public ICommand AsyncAwaitCommand { get; private set; }
        public ICommand AsyncAwaitWithCancelTaskCommand { get; private set; }

        public ICommand SimpleHmiCommand { get; private set; }

        public MenuViewModel()
        {
            AsyncAwaitCommand = new DelegateCommand(AsyncAwait);
            AsyncAwaitWithCancelTaskCommand = new DelegateCommand(AsyncAwaitWithCancelTask);
            SimpleHmiCommand = new DelegateCommand(SimpleHmi);
        }

        private void AsyncAwait()
        {
            AsyncAwaitWindow window = new AsyncAwaitWindow();
            window.Show();
        }
        private void AsyncAwaitWithCancelTask()
        {
            AsyncAwaitWithCancelTaskWindow window = new AsyncAwaitWithCancelTaskWindow();
            window.Show();
        }
        private void SimpleHmi()
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

    }
}
