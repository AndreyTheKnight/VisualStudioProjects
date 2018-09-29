using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Dictionary<string, int> actions = new Dictionary<string, int>
        {
            ["Зашифровать"] = -1,
            ["Расшифровать"] = 1
        };

        private class Data : INotifyPropertyChanged
        {
            public class LogEntry
            {
                public string input;
                public string output;
                public string action;
                public string key;

                public LogEntry(string input, string output, string action, string key)
                {
                    this.input = input;
                    this.output = output;
                    this.action = action;
                    this.key = key;
                }
                override public string ToString()
                {
                    return (action == "Зашифровать") ?
                        ($"{input} --> {action} c ключом {key} --> {output}") :
                        ($"{output} --> {action} c ключом {key} --> {input}");
                }
            }

            private string input;
            private string output;
            private string key;
            public ObservableCollection<LogEntry> log = new ObservableCollection<LogEntry>();

            public string Input
            {
                get { return input; }
                set
                {
                    input = value;
                    OnPropertyChanged("Input");
                }
            }
            public string Output
            {
                get { return output; }
                set
                {
                    output = value;
                    OnPropertyChanged("Output");
                }
            }
            public string Key
            {
                get { return key; }
                set
                {
                    key = value;
                    OnPropertyChanged("Key");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string property)
            {
                try
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
                catch (NullReferenceException) { };
            }

            public void Mutate(string action)
            {
                var source = (action == "Зашифровать") ? Input : Output;
                var key = int.Parse(Key) * actions[action];
                var result = new StringBuilder();
                foreach (var symbol in source)
                    result.Append((char)(symbol + key));
                if (action == "Зашифровать")
                    Output = result.ToString();
                else
                    Input = result.ToString();
                log.Add(new LogEntry(Input, Output, action, Key));
            }
            public void LogEntrySelected(object item)
            {
                if (item != null)
                {
                    Input = ((LogEntry)item).input;
                    Output = ((LogEntry)item).output;
                    Key = ((LogEntry)item).key;
                }
            }
        }

        private Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;
            Log.ItemsSource = data.log;
            data.Key = "4";
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            data.Mutate("Зашифровать");
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            data.Mutate("Расшифровать");
        }

        private void Log_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            data.LogEntrySelected(Log.SelectedItem);
        }
    }
}
