using DraCraft.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DraCraft.View
{
    /// <summary>
    /// Interaction logic for CreatorControl.xaml
    /// </summary>
    public partial class CreatorControl : UserControl
    {
        private CreatorControlModel ViewModel { get; set; }
        public CreatorControl()
        {
            InitializeComponent();
        }

        private void CraftableCheckbox_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CraftableToggle(((CheckBox)sender).IsChecked ?? false);
        }

        private void AddCraftItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddCraftItem();
        }

        private void CreateItem_Click(object sender, RoutedEventArgs e)
        {

            ViewModel.CreateItem();
        }

        private void RemoveCraftItem_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContentPresenter? contentPresenter = ((Grid)button.Parent).TemplatedParent as ContentPresenter;
            ViewModel.RemoveComponent(contentPresenter?.DataContext);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (CreatorControlModel)DataContext;
        }
    }
}
