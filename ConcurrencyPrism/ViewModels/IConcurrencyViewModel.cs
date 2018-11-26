using System.Threading.Tasks;
using System.Windows.Input;

namespace ConcurrencyPrism.ViewModels
{
  public interface IConcurrencyViewModel
  {
    ICommand CalculateAsyncCommand { get; }
    ICommand CalculateCommand { get; }
    ICommand CalculateMultiAsyncCommand { get; }
    ICommand CalculateMultiCommand { get; }
    ICommand CancelTaskCommand { get; }
    ICommand GetHtmlAsync2Command { get; }
    ICommand GetHtmlAsyncCommand { get; }
    ICommand GetHtmlCommand { get; }
    ICommand CalculateParallelCommand { get; }
    ICommand CancelParallelCommand { get; }
    
      
    bool IsProgressBarVisible { get; set; }
  }
}