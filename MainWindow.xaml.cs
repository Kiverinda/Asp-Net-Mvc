using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Mvc.CustomThreadPool;

namespace Mvc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ObservableCollection<string> _messagesCollection = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            Thread threadViewRandomValue = new Thread(ViewMessages);
            threadViewRandomValue.Start();
        }

        private void ViewMessages()
        {
            var threadPool = new CustomThreadPool.CustomThreadPool(5);
            var random = new Random();
            var tasks = new KeyValuePair<Priority, ITask>[100];
            for (var i = 0; i < tasks.Length; i++)
            {
                var priority = (Priority)random.Next(0, 3);
                tasks[i] = new KeyValuePair<Priority, ITask>(priority, new CustomTask(i));

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    _messagesCollection.Add($"Created task {i} with priority {priority}");
                    ListMessages.ItemsSource = _messagesCollection;
                }));
                Thread.Sleep(1000);
            }

            Parallel.ForEach(tasks, new ParallelOptions { MaxDegreeOfParallelism = 100 },
                task => { threadPool.Execute(task.Value, task.Key); });

            threadPool.Stop();
        }
    }
}

