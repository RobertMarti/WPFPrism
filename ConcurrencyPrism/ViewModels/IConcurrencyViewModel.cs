using System.Threading.Tasks;
using System.Windows.Input;
using ConcurrencyPrism.Infrastructure;
using ConcurrencyPrism.Infrastructure.AwaitableDelegateCommand;
using Prism.Commands;

namespace ConcurrencyPrism.ViewModels
{
  public interface IConcurrencyViewModel
  {
    int Anzahl { get; set; }  

    AwaitableDelegateCommand CalculateAsyncCommand { get; }
    DelegateCommand CalculateCommand { get; }
    AwaitableDelegateCommand CalculateMultiAsyncCommand { get; }
    DelegateCommand CalculateMultiCommand { get; }
    DelegateCommand CancelTaskCommand { get; }
    DelegateCommand GetHtmlAsync2Command { get; }
    DelegateCommand GetHtmlAsyncCommand { get; }
    DelegateCommand GetHtmlCommand { get; }
    AwaitableDelegateCommand CalculateParallelForEachCommand { get; }
    DelegateCommand CancelParallelForEachCommand { get; }
   
    string ResultOutput { get; set; }
    string ResultElapsedTime { get; set; }

    bool IsResultVisible { get; set; }
    bool IsProgressBarVisible { get; set; }
  }
}