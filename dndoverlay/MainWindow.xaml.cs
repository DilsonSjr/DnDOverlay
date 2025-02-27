using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using dndoverlay.Views;
using static dndoverlay.Views.BackpackView;

namespace dndoverlay
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Configurações iniciais da janela
            Loaded += SetWindowPosition;
            SizeChanged += MainWindow_SizeChanged;

            // Carrega a BackpackView no Frame
            var backpackView = new BackpackView();
            MainFrame.Navigate(backpackView);

            // Carrega a HudPersonagem no ContentControl
            HudPersonagemControl.Visibility = Visibility.Visible;
        } 

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Aqui você pode adicionar lógica para ajustar o layout, se necessário
        }

        private void SetWindowPosition(object sender, RoutedEventArgs e)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            double windowHeight = this.ActualHeight;

            this.Left = 0;
            this.Top = screenHeight - windowHeight;
        }


        private string currentPage = null; // Guarda a página aberta

        private void TogglePage(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                string pageToOpen = clickedButton.Tag.ToString();

                if (currentPage == pageToOpen)
                {
                    // Se já está aberta, fecha a aba
                    if (MainFrame.Content is BackpackView backpackView)
                    {
                        // Salva os itens antes de fechar
                        backpackView.SalvarItens();
                    }

                    MainFrame.Visibility = Visibility.Collapsed;
                    MainFrame.Content = null;
                    currentPage = null;
                }
                else
                {
                    // Abre a nova aba
                    MainFrame.Visibility = Visibility.Visible;
                    MainFrame.Navigate(new Uri(pageToOpen, UriKind.Relative));
                    currentPage = pageToOpen;
                }
            }
        }

        private void HudPersonagemControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}