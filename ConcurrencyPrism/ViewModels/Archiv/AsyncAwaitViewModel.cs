using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

//using Prism.Commands;

namespace ConcurrencyPrism.ViewModels
{

    public class AsyncAwaitViewModel : BindableBase
    {

        private const int Anzahl = 10;

        #region Properties

        private bool _isProgressBarVisible;
        public bool IsProgressBarVisible
        {
            get { return _isProgressBarVisible; }
            set { SetProperty(ref _isProgressBarVisible, value); }
        }

        //private double _progressValue;
        //public double ProgressValue
        //{
        //    get { return _progressValue; }
        //    set { SetProperty(ref _progressValue, value); }
        //}

        #endregion

        #region Commands

        public ICommand GetHtmlCommand { get; }
        public ICommand GetHtmlAsyncCommand { get; }
        public ICommand GetHtmlAsync2Command { get; }
        public ICommand CalculateCommand { get; }
        public ICommand CalculateAsyncCommand { get; }
        public ICommand CalculateMultiCommand { get; }
        public ICommand CalculateMultiAsyncCommand { get; }

        #endregion

        public AsyncAwaitViewModel()
        {
            GetHtmlCommand = new DelegateCommand(GetHtml);
            GetHtmlAsyncCommand = new DelegateCommand(GetHtmlAsync);
            GetHtmlAsync2Command = new DelegateCommand(GetHtmlAsync2);
            CalculateCommand = new DelegateCommand(Calculate);
            CalculateAsyncCommand = new DelegateCommand(CalculateAsync);
            CalculateMultiCommand = new DelegateCommand(CalculateMulti);
            CalculateMultiAsyncCommand = new DelegateCommand(CalculateMultiAsync);

            IsProgressBarVisible = false;
        }

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

        public async Task<string> GetHtmlAsync(string url)
        {
            var webClient = new WebClient();
            return await webClient.DownloadStringTaskAsync(new Uri(url));
        }

        #endregion

        #region CPU Bound

        private void Calculate()
        {
            int nSquare = Calculate(3);
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
            DateTime StartZeit = DateTime.Now;

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

            DateTime EndZeit = DateTime.Now;
            TimeSpan GemessendeZeit = EndZeit - StartZeit;

            MessageBox.Show($"GemessendeZeit: {GemessendeZeit}", sSquares);
        }

        private async void CalculateMultiAsync()
        {
            DateTime StartZeit = DateTime.Now;

            List<TaskResult> taskResults = new List<TaskResult>();
            for (int i = 0; i < Anzahl; i++)
            {
                Task<int> taskSquare = StartCalculateAsync(i);
                taskResults.Add(new TaskResult(i, taskSquare));
            }

            IsProgressBarVisible = true;

            foreach (var taskResult in taskResults)
            {
                int nSquare = await taskResult.Task;
                taskResults[taskResult.Key].Square = nSquare;
            }

            //IEnumerable<Task> tasks = taskResults.Select(x => x.Task);
            //await Task.WhenAll(tasks);

            IsProgressBarVisible = false;

            string sSquares = "";
            for (int i = 0; i < Anzahl; i++)
            {
                if (sSquares != "") sSquares += ",";

                sSquares += taskResults[i].Square;
            }

            DateTime EndZeit = DateTime.Now;
            TimeSpan GemessendeZeit = EndZeit - StartZeit;

            MessageBox.Show($"GemessendeZeit: {GemessendeZeit}", sSquares);
        }
        public Task<int> StartCalculateAsync(int n)
        {
            return Task.Run(() => Calculate(n));
        }

        #endregion

        #region Common

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

        public int Calculate(int n)
        {
            //Lange Rechenoparation: ca 1 Sekunden
            for (int i = 0; i < 100000; i++)
            {
                for (int j = 0; j < 3000; j++)
                {
                    int z = i * j;
                }
            }

            return n * n;
        }

        #endregion

    }
}
