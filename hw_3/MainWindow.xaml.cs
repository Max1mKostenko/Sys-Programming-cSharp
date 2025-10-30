using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HorseRace
{
    public partial class MainWindow : Window
    {
        private List<Button> horses;
        private List<Task> raceTasks;
        private List<RaceResult> results;
        private bool raceInProgress = false;
        private object lockObject = new object();
        private int finishPosition = 0;
        private const double FINISH_LINE_POSITION = 720.0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeRace();
        }

        private void InitializeRace()
        {
            horses = new List<Button> { Horse1, Horse2, Horse3, Horse4, Horse5 };
            results = new List<RaceResult>();
            raceTasks = new List<Task>();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (raceInProgress)
                return;

            StartRace();
        }

        private void StartRace()
        {
            raceInProgress = true;
            finishPosition = 0;
            results.Clear();
            ResultsGrid.ItemsSource = null;

            StartButton.IsEnabled = false;
            ResetButton.IsEnabled = false;

            raceTasks.Clear();
            for (int i = 0; i < horses.Count; i++)
            {
                int horseIndex = i;
                Task task = Task.Run(() => RunHorse(horseIndex));
                raceTasks.Add(task);
            }

            Task.Run(async () =>
            {
                await Task.WhenAll(raceTasks);

                Dispatcher.Invoke(() =>
                {
                    ShowResults();
                    raceInProgress = false;
                    StartButton.IsEnabled = true;
                    ResetButton.IsEnabled = true;
                });
            });
        }

        private void RunHorse(int horseIndex)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            Button horse = horses[horseIndex];
            double startPosition = 10.0;
            double currentPosition = startPosition;
            DateTime startTime = DateTime.Now;

            while (currentPosition < FINISH_LINE_POSITION)
            {
                int speed = random.Next(1, 6);
                currentPosition += speed;

                Dispatcher.Invoke(() =>
                {
                    Canvas.SetLeft(horse, currentPosition);
                });

                Thread.Sleep(random.Next(50, 150));
            }

            DateTime finishTime = DateTime.Now;
            TimeSpan raceTime = finishTime - startTime;

            lock (lockObject)
            {
                finishPosition++;
                results.Add(new RaceResult
                {
                    Position = finishPosition,
                    HorseName = $"Лошадь {horseIndex + 1}",
                    Time = $"{raceTime.TotalSeconds:F2}",
                    Status = finishPosition == 1 ? "🏆 Победитель!" : "Финишировал"
                });
            }
        }

        private void ShowResults()
        {
            var sortedResults = results.OrderBy(r => r.Position).ToList();
            ResultsGrid.ItemsSource = sortedResults;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (raceInProgress)
                return;

            ResetRace();
        }

        private void ResetRace()
        {
            foreach (var horse in horses)
            {
                Canvas.SetLeft(horse, 10.0);
            }

            results.Clear();
            ResultsGrid.ItemsSource = null;
            finishPosition = 0;
        }
    }

    public class RaceResult
    {
        public int Position { get; set; }
        public string HorseName { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
    }
}