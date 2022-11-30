using DraCraft.Model;
using DraCraft.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace DraCraft.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private MainWindowModel ViewModel { get; set; }

        public MainWindow()
        {
            DataContext = this;
            Config.Init();
            ResourceList.Load();
            InitializeComponent();
        }







        private void Window_Closed(object sender, EventArgs e)
        {
            ResourceList.Current.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (MainWindowModel)DataContext;
        }
    }
}



