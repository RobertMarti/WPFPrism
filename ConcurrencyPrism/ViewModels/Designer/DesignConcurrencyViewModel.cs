using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ConcurrencyPrism.Infrastructure;
using ConcurrencyPrism.Infrastructure.AwaitableDelegateCommand;
using Prism.Commands;

namespace ConcurrencyPrism.ViewModels.Designer
{
  class DesignConcurrencyViewModel : IConcurrencyViewModel
  {
    public int Anzahl { get; set; }

    public AwaitableDelegateCommand CalculateAsyncCommand { get; }
    public DelegateCommand CalculateCommand { get; }
    public AwaitableDelegateCommand CalculateMultiAsyncCommand { get; }
    public DelegateCommand CalculateMultiCommand { get; }
    public DelegateCommand CancelTaskCommand { get; }
    public DelegateCommand GetHtmlAsync2Command { get; }
    public DelegateCommand GetHtmlAsyncCommand { get; }
    public DelegateCommand GetHtmlCommand { get; }
    public AwaitableDelegateCommand CalculateParallelCommand { get; }
    public DelegateCommand CancelParallelCommand { get; }

    public string ResultOutput
    {
      get => "1,4,9,16,25";
      set => throw new NotImplementedException();
    }

    public string ResultElapsedTime
    {
      get => "GemessendeZeit: 3.3";
      set => throw new NotImplementedException();
    }

    public bool IsProgressBarVisible
    {
      get => false;
      set => throw new NotImplementedException();
    }
    public bool IsResultVisible
    {
      get => true;
      set => throw new NotImplementedException();
    }

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
