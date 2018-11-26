using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConcurrencyPrism.ViewModels.Designer
{
  class DesignConcurrencyViewModel : IConcurrencyViewModel
  {

    public ICommand CalculateAsyncCommand { get; }
    public ICommand CalculateCommand { get; }
    public ICommand CalculateMultiAsyncCommand { get; }
    public ICommand CalculateMultiCommand { get; }
    public ICommand CancelTaskCommand { get; }
    public ICommand GetHtmlAsync2Command { get; }
    public ICommand GetHtmlAsyncCommand { get; }
    public ICommand GetHtmlCommand { get; }
    public ICommand CalculateParallelCommand { get; }
    public ICommand CancelParallelCommand { get; }

    public bool IsProgressBarVisible { get; set; }
    public bool IsResultVisible { get; set; }
    public int Calculate(int n)
    {
      throw new NotImplementedException();
    }

    public void CancelTask()
    {
      throw new NotImplementedException();
    }

    public Task<string> GetHtmlAsync(string url)
    {
      throw new NotImplementedException();
    }

    public Task<int> StartCalculateAsync(int n)
    {
      throw new NotImplementedException();
    }
  }
}
