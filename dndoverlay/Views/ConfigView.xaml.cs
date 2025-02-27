using System.Windows;
using System.Windows.Controls;

namespace dndoverlay.Views
{
    public partial class ConfigView : Page
    {
        public ConfigView()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Fecha a aplicação
            Application.Current.Shutdown();
        }
    }
}
