using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

//using Prism.Commands;

namespace ConcurrencyPrism.ViewModels
{

  public class ConcurrencyViewModel : BindableBase, IConcurrencyViewModel
  {

    private const int Anzahl = 20;

    private CancellationToken[] _cancellationTokens = new CancellationToken[Anzahl];
    private CancellationTokenSource[] _cancellationTokenSources = new CancellationTokenSource[Anzahl];

    private CancellationToken _cancellationToken = new CancellationToken();
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    #region Properties

    private bool _isProgressBarVisible;

    public bool IsProgressBarVisible
    {
      get { return _isProgressBarVisible; }
      set { SetProperty(ref _isProgressBarVisible, value); }
    }

    #endregion

    #region Commands

    public ICommand GetHtmlCommand { get; }
    public ICommand GetHtmlAsyncCommand { get; }
    public ICommand GetHtmlAsync2Command { get; }
    public ICommand CalculateCommand { get; }
    public ICommand CalculateAsyncCommand { get; }
    public ICommand CalculateMultiCommand { get; }
    public ICommand CalculateMultiAsyncCommand { get; }
    public ICommand CancelTaskCommand { get; }
    public ICommand CalculateParallelCommand { get; }
    public ICommand CancelParallelCommand { get; }

    #endregion

    #region Constructors
    public ConcurrencyViewModel()
    {
      GetHtmlCommand = new DelegateCommand(GetHtml);
      GetHtmlAsyncCommand = new DelegateCommand(GetHtmlAsync);
      GetHtmlAsync2Command = new DelegateCommand(GetHtmlAsync2);
      CalculateCommand = new DelegateCommand(Calculate);
      CalculateAsyncCommand = new DelegateCommand(CalculateAsync);
      CalculateMultiCommand = new DelegateCommand(CalculateMulti);
      CalculateMultiAsyncCommand = new DelegateCommand(CalculateMultiAsync);
      CancelTaskCommand = new DelegateCommand(CancelTask);
      CalculateParallelCommand = new DelegateCommand(CalculateParallel);
      CancelParallelCommand = new DelegateCommand(CancelParallel);

      IsProgressBarVisible = false;
    }

    #endregion

    #region I/O Bound

    private void GetHtml()
    {
      string sHtml = GetHtml(@"http://www.eigerconsulting.ch");
      MessageBox.Show(sHtml.Substring(0, 100));
    }

    private string GetHtml(string url)
    {
      var webClient = new WebClient();
      return webClient.DownloadString(url);
    }

    private async void GetHtmlAsync()
    {
      string html = await GetHtmlAsync(@"http://www.eigerconsulting.ch");
      MessageBox.Show(html.Substring(0, 100));
    }

    private async void GetHtmlAsync2()
    {
      Task<string> task = GetHtmlAsync(@"http://www.eigerconsulting.ch");

      MessageBox.Show("Waiting...");

      string html = await task;
      MessageBox.Show(html.Substring(0, 100));
    }

    private async Task<string> GetHtmlAsync(string url)
    {
      var webClient = new WebClient();
      return await webClient.DownloadStringTaskAsync(new Uri(url));
    }

    #endregion
    
    #region CPU Bound Wait

    private void Calculate()
    {
      int nSquare = Calculate(3);

      //Nur zur Illustration
      IsProgressBarVisible = true;
      IsProgressBarVisible = false;

      MessageBox.Show(nSquare.ToString());
    }

    private async void CalculateAsync()
    {
      Task<int> taskSquare = StartCalculateAsync(3);

      IsProgressBarVisible = true;

      int nSquare = await taskSquare;

      IsProgressBarVisible = false;

      MessageBox.Show(nSquare.ToString());
    }

    private void CalculateMulti()
    {
      Debug.WriteLine("CalculateMulti Startet");

      var stopWatch = Stopwatch.StartNew();

      List<int> results = new List<int>();
      for (int i = 0; i < Anzahl; i++)
      {
        results.Add(i);
        results[i] = Calculate(i);
      }

      //Nur zur Illustration
      IsProgressBarVisible = true;
      IsProgressBarVisible = false;

      string sSquares = "";
      for (int i = 0; i < Anzahl; i++)
      {
        if (sSquares != "") sSquares += ",";

        sSquares += results[i];
      }

      MessageBox.Show($"GemessendeZeit: {stopWatch.Elapsed.TotalSeconds}", sSquares);
    }

    private async void CalculateMultiAsync()
    {
      IsProgressBarVisible = true;

      var stopWatch = Stopwatch.StartNew();

      List<TaskResult> taskResults = new List<TaskResult>();
      for (int i = 0; i < Anzahl; i++)
      {
        Task<int> taskSquare = StartCalculateAsync(i);
        taskResults.Add(new TaskResult(i, taskSquare));
      }

      try
      {
        await Calculate(taskResults);

        IsProgressBarVisible = false;
        Output(taskResults, stopWatch.Elapsed.TotalSeconds);
      }
      catch (OperationCanceledException)
      {
        IsProgressBarVisible = false;
        MessageBox.Show("Tasks Cancelled");
      }
    }

    private static void Output(List<TaskResult> taskResults, double elapsedTime)
    {
      string sSquares = "";
      for (int i = 0; i < Anzahl; i++)
      {
        if (sSquares != "") sSquares += ",";

        sSquares += taskResults[i].Square;
      }

      MessageBox.Show($"GemessendeZeit: {elapsedTime}", sSquares);
    }

    private async Task Calculate(List<TaskResult> taskResults)
    {
      foreach (var taskResult in taskResults)
      {
        int nSquare = await taskResult.Task;
        taskResults[taskResult.Key].Square = nSquare;
      }
    }

    private void CancelTask()
    {
      for (var n = 0; n < Anzahl; n++)
      {
        _cancellationTokenSources[n].Cancel();
      }
    }

    public Task<int> StartCalculateAsync(int n)
    {
      _cancellationTokenSources[n] = new CancellationTokenSource();
      _cancellationTokens[n] = _cancellationTokenSources[n].Token;
      _cancellationTokens[n].Register(TaskCancelled, n);

      Func<int> calculate = () => Calculate(n);

      //return Task.Factory.StartNew(calculate, _cancellationToken[n]);

      //Shortcut ab .Net 4.5
      return Task.Run(calculate, _cancellationTokens[n]);
    }

    private void TaskCancelled(object n)
    {
      Debug.WriteLine($"Task {n} Cancelled");
      //MessageBox.Show($"Task {n} Cancelled");
    }
    private class TaskResult
    {
      public TaskResult(int key, Task<int> task)
      {
        Key = key;
        Square = 0;
        Task = task;
      }
      public int Key { get; set; }
      public int Square { get; set; }
      public Task<int> Task { get; set; }
    }

    private int Calculate(int n)
    {
      //Lange Rechenoparation: ca 1 Sekunden
      for (int i = 0; i < 100000; i++)
      {
        for (int j = 0; j < 3000; j++)
        {
          //_cancellationToken[n].ThrowIfCancellationRequested(); -- gibt Exceptions ???

          if (_cancellationTokens[n].IsCancellationRequested) return -1;

          int z = i * j;
        }
      }

      return n * n;
    }

    #endregion

    #region CPU Bound Parallel
    private async void CalculateParallel()
    {
      var stopWatch = Stopwatch.StartNew();
      IsProgressBarVisible = true;

      var results = new List<Result>();
      for (var i = 0; i < Anzahl; i++)
      {
        results.Add(new Result(i));
      }

      Action action = () => Parallel.ForEach(results, new ParallelOptions(), CalculateParallel);
      ProvideCancellation();

      //Aktion in Threadpool ausführen,m damit der UI-Thread nicht blockiert wird
      Task task = Task.Run(action , _cancellationToken);
      await task;

      IsProgressBarVisible = false;
      Output(results, stopWatch.Elapsed.TotalSeconds);
    }

    private void ProvideCancellation()
    {
      _cancellationTokenSource = new CancellationTokenSource();
      _cancellationToken = _cancellationTokenSource.Token;
      _cancellationToken.Register(TaskCancelled);
    }

    private void CalculateParallel(Result result)
    {
      result.Square = CalculateParallel(result.Key);
    }

    private int CalculateParallel(int n)
    {
      //Lange Rechenoparation: ca 1 Sekunden
      for (int i = 0; i < 100000; i++)
      {
        for (int j = 0; j < 3000; j++)
        {
          if (_cancellationToken.IsCancellationRequested) return -1;
          int z = i * j;
        }
      }

      return n * n;
    }

    private void CancelParallel()
    {
      _cancellationTokenSource.Cancel();
    }

    private void TaskCancelled()
    {
      Debug.WriteLine($"Parallel.Foreach Cancelled");
    }

    private static void Output(List<Result> results, double elapsedTime)
    {
      string sSquares = "";
      for (int i = 0; i < Anzahl; i++)
      {
        if (sSquares != "") sSquares += ",";

        sSquares += results[i].Square;
      }

      MessageBox.Show($"GemessendeZeit: {elapsedTime}", sSquares);
    }

    private class Result
    {
      public Result(int key)
      {
        Key = key;
        Square = 0;
      }
      public int Key { get; set; }
      public int Square { get; set; }
    }

    #endregion

    #region Common

    #endregion

  }
}
