using DraCraft.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DraCraft.View
{
    /// <summary>
    /// Interaction logic for CraftingStepControl.xaml
    /// </summary>
    public partial class CraftingStepControl : UserControl
    {
        private CraftingStepControlModel ViewModel { get; set; }

        public CraftingStepControl()
        {
            InitializeComponent();
        }

        private void AmountTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (uint.TryParse(((TextBox)sender).Text, out uint result))
            {
                ViewModel.SetItemAmount(result);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (CraftingStepControlModel)DataContext;
            ViewModel.DispatcherEvent += Application.Current.Dispatcher.Invoke;
        }
    }
}
