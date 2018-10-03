using System;
using System.Collections.Generic;
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

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class Data : INotifyPropertyChanged
        {
            private string plaintext;
            private string ciphertext;
            private string key;

            public string Plaintext
            {
                get { return plaintext; }
                set
                {
                    plaintext = value;
                    OnPropertyChanged("Plaintext");
                }
            }
            public string Ciphertext
            {
                get { return ciphertext; }
                set
                {
                    ciphertext = value;
                    OnPropertyChanged("Ciphertext");
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

        }

        private Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = data;
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            data.Ciphertext = data.Plaintext.Encrypt(data.Key);
        }

        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            data.Plaintext = Desx.Decrypt(data.Ciphertext);
        }

    }
}
