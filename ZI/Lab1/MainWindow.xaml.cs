using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private class Data
        {
            public string Input { get; set; }
            public string Key { get; set; }
            public ObservableCollection<string> output = new ObservableCollection<string>();

            public void Mutate(KeyValuePair<string, int> operation)
            {
                var result = new StringBuilder();
                result.Append(this.Input);
                result.Append($" --> {operation.Key} (Ключ: {this.Key}) --> ");
                foreach (char symbol in this.Input)
                {
                    int key = int.Parse(this.Key) * operation.Value;
                    result.Append((char)((int)symbol + key));
                }
                this.output.Add(result.ToString());
            }
        }

        private Data data = new Data();
        private Dictionary<string, int> operations = new Dictionary<string, int>
        {
            ["Зашифровать"] = -1,
            ["Расшифровать"] = 1
        };

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.data;
            this.Output.ItemsSource = this.data.output;
        }

        private void ButtonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            this.data.Mutate(new KeyValuePair<string, int>();
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            this.data.Mutate(this.operations["Decrypt"]);
        }
    }
}
