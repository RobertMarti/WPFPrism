using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowCancelTask : Window
    {
        private const int Anzahl = 10;

        private CancellationToken[] _cancellationToken = new CancellationToken[Anzahl];
        private CancellationTokenSource[] _cancellationTokenSource = new CancellationTokenSource[Anzahl];


        public MainWindowCancelTask()
        {
            InitializeComponent();
        }

        #region I/O Bound

        private void BtnGetHtml_Click(object sender, RoutedEventArgs e)
        {
            string sHtml = GetHtml(@"http://www.eigerconsulting.ch");
            MessageBox.Show(sHtml.Substring(0, 100));
        }

        public string GetHtml(string url)
        {
            var webClient = new WebClient();
            return webClient.DownloadString(url);
        }

        private async void BtnGetHtmlAsync_Click(object sender, RoutedEventArgs e)
        {
            string html = await GetHtmlAsync(@"http://www.eigerconsulting.ch");
            MessageBox.Show(html.Substring(0, 100));
        }

        private async void BtnGetHtmlAsync2_Click(object sender, RoutedEventArgs e)
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

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            int nSquare = Calculate(3);

            //Nur zur Illustration
            Progress.IsIndeterminate = true;
            Progress.IsIndeterminate = false;

            MessageBox.Show(nSquare.ToString());
        }

        private async void BtnCalculateAsync_Click(object sender, RoutedEventArgs e)
        {
            Task<int> taskSquare = StartCalculateAsync(3);

            Progress.IsIndeterminate = true;

            int nSquare = await taskSquare;

            Progress.IsIndeterminate = false;

            MessageBox.Show(nSquare.ToString());
        }

        private void BtnCalculateMulti_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("BtnCalculateMulti_Click Startet");

            DateTime startZeit = DateTime.Now;

            List<int> results = new List<int>();
            for (int i = 0; i < Anzahl; i++)
            {
                results.Add(i);
                results[i] = Calculate(i);
            }

            //Nur zur Illustration
            Progress.IsIndeterminate = true;
            Progress.IsIndeterminate = false;

            string sSquares = "";
            for (int i = 0; i < Anzahl; i++)
            {
                if (sSquares != "") sSquares += ",";

                sSquares += results[i];
            }

            DateTime endZeit = DateTime.Now;
            TimeSpan gemessendeZeit = endZeit - startZeit;

            MessageBox.Show($"GemessendeZeit: {gemessendeZeit}", sSquares);
        }

        private async void BtnCalculateMultiAsync_Click(object sender, RoutedEventArgs e)
        {
            DateTime startZeit = DateTime.Now;

            List<TaskResult> taskResults = new List<TaskResult>();
            for (int i = 0; i < Anzahl; i++)
            {
                Task<int> taskSquare = StartCalculateAsync(i);
                taskResults.Add(new TaskResult(i, taskSquare));
            }

            Progress.IsIndeterminate = true;

            try
            {
                await Calculate(taskResults);

                Progress.IsIndeterminate = false;
                Output(taskResults, startZeit);
            }
            catch (OperationCanceledException)
            {
                Progress.IsIndeterminate = false;
                MessageBox.Show("Tasks Cancelled");
            }
        }

        private static void Output(List<TaskResult> taskResults, DateTime startZeit)
        {
            string sSquares = "";
            for (int i = 0; i < Anzahl; i++)
            {
                if (sSquares != "") sSquares += ",";

                sSquares += taskResults[i].Square;
            }

            DateTime endZeit = DateTime.Now;
            TimeSpan gemessendeZeit = endZeit - startZeit;

            MessageBox.Show($"GemessendeZeit: {gemessendeZeit}", sSquares);
        }

        private async Task Calculate(List<TaskResult> taskResults)
        {
            foreach (var taskResult in taskResults)
            {
                int nSquare = await taskResult.Task;
                taskResults[taskResult.Key].Square = nSquare;
            }
        }

        private void BtnCancelTask_Click(object sender, RoutedEventArgs e)
        {
            for (var n = 0; n < Anzahl; n++)
            {
                _cancellationTokenSource[n].Cancel();
            }
        }

        public Task<int> StartCalculateAsync(int n)
        {
            _cancellationTokenSource[n] = new CancellationTokenSource();
            _cancellationToken[n] = _cancellationTokenSource[n].Token;
            _cancellationToken[n].Register(TaskCancelled, n);

            Func<int> calculate = () => Calculate(n);

            //return Task.Factory.StartNew(calculate, _cancellationToken[n]);

            //Shortcut ab .Net 4.5
            return Task.Run(calculate, _cancellationToken[n]);
        }

         private void TaskCancelled(object n)
        {
            Debug.WriteLine($"Task {n} Cancelled");
            //MessageBox.Show($"Task {n} Cancelled");
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
                    //_cancellationToken[n].ThrowIfCancellationRequested(); -- gibt Exceptions ???

                    if (_cancellationToken[n].IsCancellationRequested) return -1;
                    
                    int z = i * j;
                }
            }

            return n * n;
        }

        #endregion

    }
}
