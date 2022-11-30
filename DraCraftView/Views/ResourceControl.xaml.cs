using DraCraft.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DraCraft.View
{
    /// <summary>
    /// Interaction logic for ResourceControl.xaml
    /// </summary>
    public partial class ResourceControl : UserControl
    {
        ResourceControlModel ViewModel { get; set; }
        public ResourceControl()
        {
            InitializeComponent();
        }

        private void RemoveResource_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ContentPresenter? contentPresenter = ((Grid)button.Parent).TemplatedParent as ContentPresenter;
            ViewModel.RemoveResource(contentPresenter?.DataContext);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = (ResourceControlModel)DataContext;
        }
    }
}
