using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using VVM.ViewModels.AsyncAwait;

namespace VVM.Views.AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AsyncAwaitWindow : Window
    {
        public AsyncAwaitWindow()
        {
            InitializeComponent();

            //Passiert mit mvvm:ViewModelLocator.AutoWireViewModel="True" (Prism)
            //DataContext = new AsyncAwaitWindowViewModel();
        }

    }
}
