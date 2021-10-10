using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Mvc
{
    public partial class MainWindow : Window
    {
        private float interval = 1;
        private SafeCollection<int> safeCollection = new SafeCollection<int>();

        public MainWindow()
        {
            InitializeComponent();

            Thread threadViewResult = new Thread(AddNumbersFibonacciInTextBox);
            Thread threadAddItemsInSafeCollection = new Thread(AddItems);
            Thread threadRemoveItemsInSafeCollection = new Thread(RemoveAtItems);
            Thread threadGetItemsFromSafeCollection = new Thread(ViewCollection);
            
            threadAddItemsInSafeCollection.Start();
            threadRemoveItemsInSafeCollection.Start();
            threadGetItemsFromSafeCollection.Start();
            threadViewResult.Start();
        }

        private void AddItems()
        {
            Random random = new Random();

            for (int i = 0; i < 20; i++)
            {
                safeCollection.SafeAdd(random.Next(0, 100));
                Thread threadGetItemsFromSafeCollection = new Thread(ViewCollection);
                threadGetItemsFromSafeCollection.Start();
                Thread.Sleep(1000);
            }
        }

        private void RemoveAtItems()
        {
            for (int i = 0; i < 20; i++)
            {
                if (safeCollection?.GetList().Count > 0)
                {
                    safeCollection.SafeRemove(0);
                    Thread threadGetItemsFromSafeCollection = new Thread(ViewCollection);
                    threadGetItemsFromSafeCollection.Start();
                }
                Thread.Sleep(2000);
            }
        }

        private void AddNumbersFibonacciInTextBox(object obj)
        {
            int firstNumber = 0;
            int secondNumber = 1;
            int currentNumber;

            for (int i = 0; i < 30; i++)
            {
                currentNumber = firstNumber + secondNumber;
                firstNumber = secondNumber;
                secondNumber = currentNumber;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    ResultViewTextBox.Text = $" {currentNumber}";
                }));
                Thread.Sleep((int)(interval * 1000));
            }
        }

        private void TimeSettingsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            interval = (float)(e.NewValue);
            TimeSettingsView.Text = $"{interval}";
        }

        private void ViewCollection()
        {
            if (safeCollection?.GetList().Count > 0)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    ValueList.ItemsSource = new ObservableCollection<int>(safeCollection.GetList());
                }));
            }
        }
    }
}
