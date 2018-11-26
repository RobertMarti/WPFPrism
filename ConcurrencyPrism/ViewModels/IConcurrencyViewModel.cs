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
    bool IsProgressBarVisible { get; set; }

    int Calculate(int n);
    void CancelTask();
    Task<string> GetHtmlAsync(string url);
    Task<int> StartCalculateAsync(int n);
  }
}