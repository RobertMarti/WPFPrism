using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ConcurrencyPrism.Infrastructure;
using ConcurrencyPrism.Infrastructure.AwaitableDelegateCommand;
using Prism.Commands;
using Prism.Mvvm;

//using Prism.Commands;

namespace ConcurrencyPrism.ViewModels
{

  public class ConcurrencyViewModel : BindableBase, IConcurrencyViewModel
  {

    #region declarations

    private CancellationToken[] _cancellationTokens;
    private CancellationTokenSource[] _cancellationTokenSources;

    private CancellationToken _cancellationToken;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    private Stopwatch _stopWatch = new Stopwatch();

    #endregion

    #region Bindable Properties

    private int _anzahl;
    public int Anzahl
    {
      get { return _anzahl; }
      set { SetProperty(ref _anzahl, value); }
    }

    private string _resultOutput;
    public string ResultOutput
    {
      get { return _resultOutput; }
      set { SetProperty(ref _resultOutput, value); }
    }

    private string _elapsedTime;
    public string ResultElapsedTime
    {
      get { return _elapsedTime; }
      set { SetProperty(ref _elapsedTime, value); }
    }

    private bool _isProgressBarVisible;
    public bool IsProgressBarVisible
    {
      get { return _isProgressBarVisible; }
      set { SetProperty(ref _isProgressBarVisible, value); }
    }

    private bool _isResultVisible;
    public bool IsResultVisible
    {
      get { return _isResultVisible; }
      set { SetProperty(ref _isResultVisible, value); }
    }

    #endregion

    #region Commands

    public DelegateCommand GetHtmlCommand { get; }
    public DelegateCommand GetHtmlAsyncCommand { get; }
    public DelegateCommand GetHtmlAsync2Command { get; }
    public DelegateCommand CalculateCommand { get; }
    public AwaitableDelegateCommand CalculateAsyncCommand { get; }
    public DelegateCommand CalculateMultiCommand { get; }
    public AwaitableDelegateCommand CalculateMultiAsyncCommand { get; }
    public DelegateCommand CancelTaskCommand { get; }
    public AwaitableDelegateCommand CalculateParallelCommand { get; }
    public DelegateCommand CancelParallelCommand { get; }
    int IConcurrencyViewModel.Anzahl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    #endregion

    #region Constructors
    public ConcurrencyViewModel()
    {
      GetHtmlCommand = new DelegateCommand(GetHtml);
      GetHtmlAsyncCommand = new DelegateCommand(GetHtmlAsync);
      GetHtmlAsync2Command = new DelegateCommand(GetHtmlAsync2);
      CalculateCommand = new DelegateCommand(Calculate);
      CalculateAsyncCommand = new AwaitableDelegateCommand(CalculateAsync);
      CalculateMultiCommand = new DelegateCommand(CalculateMulti);
      CalculateMultiAsyncCommand = new AwaitableDelegateCommand(CalculateMultiAsync);
      CancelTaskCommand = new DelegateCommand(CancelTasks);
      CalculateParallelCommand = new AwaitableDelegateCommand(CalculateParallelAsync);
      CancelParallelCommand = new DelegateCommand(CancelParallel);

      IsProgressBarVisible = false;
      IsResultVisible = false;

      Anzahl = 10;
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

      MessageBox.Show(nSquare.ToString());
    }

    private async Task CalculateAsync()
    {
      StartCalculation();

      Task<int> taskSquare = StartCalculateAsync(3);

      int nSquare = await taskSquare;

      EndCalculation(new List<int> { nSquare });
    }

    private void CalculateMulti()
    {
      Debug.WriteLine("CalculateMulti Startet");

      StartCalculation();

      List<int> squares = new List<int>();
      for (int i = 0; i < Anzahl; i++)
      {
        squares.Add(i);
        squares[i] = Calculate(i);
      }

      EndCalculation(squares);
    }

    private async Task CalculateMultiAsync()
    {
      StartCalculation();
      
      List<TaskResult> taskResults = new List<TaskResult>();
      for (int i = 0; i < Anzahl; i++)
      {
        Task<int> taskSquare = StartCalculateAsync(i);
        taskResults.Add(new TaskResult(i, taskSquare));
      }

      try
      {
        await Calculate(taskResults);

        EndCalculation(taskResults);
      }
      catch (OperationCanceledException)
      {
        CancelCalculation();
      }
    }

    private async Task Calculate(List<TaskResult> taskResults)
    {
      foreach (var taskResult in taskResults)
      {
        int nSquare = await taskResult.Task;
        taskResults[taskResult.Key].Square = nSquare;
      }
    }

    private void CancelTasks()
    {
      for (var n = 0; n < Anzahl; n++)
      {
        _cancellationTokenSources[n].Cancel();
      }
    }

    public Task<int> StartCalculateAsync(int n)
    {
      _cancellationTokenSources = new CancellationTokenSource[Anzahl];
      _cancellationTokenSources[n] = new CancellationTokenSource();
      _cancellationTokens = new CancellationToken[Anzahl];
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

          if (_cancellationTokens != null)
          {
            if (_cancellationTokens[n].IsCancellationRequested) return -1;
          }

          int z = i * j;
        }
      }

      return n * n;
    }

    #endregion

    #region CPU Bound Parallel

    private async Task CalculateParallelAsync()
    {
      StartCalculation();

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

      if (!_cancellationToken.IsCancellationRequested)
      {
        EndCalculation(results);
      }
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
      ////Debug.WriteLine($"Parallel.Foreach Cancelled");
      CancelCalculation();
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

    private void StartCalculation()
    {
      _stopWatch.Restart();

      IsProgressBarVisible = true;
      IsResultVisible = false;
    }

    private void EndCalculation(List<Result> results)
    {
      List<int> squares = new List<int>();
      foreach (var result in results)
      {
        squares.Add(result.Square);
      }

      EndCalculation(squares);
    }

    private void EndCalculation(List<TaskResult> results)
    {
      List<int> squares = new List<int>();
      foreach (var result in results)
      {
        squares.Add(result.Square);
      }

      EndCalculation(squares);
    }

    private void EndCalculation(List<int> squares)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var square in squares)
      {
        if (sb.Length > 0) sb.Append(",");

        sb.Append(square);
      }

      IsProgressBarVisible = false;
      IsResultVisible = true;
      ResultOutput = sb.ToString();
      ResultElapsedTime = $"GemessendeZeit: {_stopWatch.Elapsed.TotalSeconds}";
    }

    private void CancelCalculation()
    {
      IsProgressBarVisible = false;
      IsResultVisible = true;
      ResultOutput = "Calculation Cancelled";
      ResultElapsedTime = $"GemessendeZeit: {_stopWatch.Elapsed.TotalSeconds}";
    }

    #endregion

  }
}
