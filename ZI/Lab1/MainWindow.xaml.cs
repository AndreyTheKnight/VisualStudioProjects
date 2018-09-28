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
        private ObservableCollection<string> output = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.Output.ItemsSource = this.output;
        }

        private void ButtonEncrypt_Click(object sender, RoutedEventArgs e)
        {
            var result = new StringBuilder();
            result.Append(this.Input.Text + this.Key.Text);
            this.output.Add(result.ToString());
        }
    }
}
