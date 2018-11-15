using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace VVM.Views.AsyncAwait
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AsyncAwaitWindow : Window
    {
        private const int Anzahl = 10;

        public AsyncAwaitWindow()
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
            DateTime StartZeit = DateTime.Now;

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

            DateTime EndZeit = DateTime.Now;
            TimeSpan GemessendeZeit = EndZeit - StartZeit;

            MessageBox.Show($"GemessendeZeit: {GemessendeZeit}", sSquares);
        }

        private async void BtnCalculateMultiAsync_Click(object sender, RoutedEventArgs e)
        {
            DateTime StartZeit = DateTime.Now;

            List<TaskResult> taskResults = new List<TaskResult>();
            for (int i = 0; i < Anzahl; i++)
            {
                Task<int> taskSquare = StartCalculateAsync(i);
                taskResults.Add(new TaskResult(i, taskSquare));
            }

            Progress.IsIndeterminate = true;

            foreach (var taskResult in taskResults)
            {
                int nSquare = await taskResult.Task;
                taskResults[taskResult.Key].Square = nSquare;
            }

            //IEnumerable<Task> tasks = taskResults.Select(x => x.Task);
            //await Task.WhenAll(tasks);

            Progress.IsIndeterminate = false;

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
