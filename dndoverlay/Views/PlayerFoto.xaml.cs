using Microsoft.Win32;
using Newtonsoft.Json;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using dndoverlay.Models;

namespace dndoverlay.Views
{
    public partial class PlayerFoto : UserControl
    {
        public static readonly DependencyProperty PlayerImageProperty =
            DependencyProperty.Register(nameof(PlayerImage), typeof(BitmapImage), typeof(PlayerFoto), new PropertyMetadata(null));

        public BitmapImage PlayerImage
        {
            get => (BitmapImage)GetValue(PlayerImageProperty);
            set => SetValue(PlayerImageProperty, value);
        }

        public PlayerFoto()
        {
            InitializeComponent(); // Certifique-se de que este método está sendo chamado
            DataContext = this;

            // Carregar a imagem ao iniciar o programa
            CarregarImagem();
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Atribui a imagem ao controle e salva o caminho
                PlayerImage = new BitmapImage(new Uri(openFileDialog.FileName));
                SalvarImagem(openFileDialog.FileName);
            }
        }

        private void SalvarImagem(string caminhoImagem)
        {
            try
            {
                string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personagem.json");

                // Verifica se o arquivo existe, se não, cria um novo
                PersonagemData personagemData = CarregarDados(caminhoArquivo);

                // Atualiza apenas o caminho da imagem
                personagemData.PlayerImagePath = caminhoImagem;

                // Salva o arquivo JSON com o novo caminho
                SalvarDados(personagemData, caminhoArquivo);

                Console.WriteLine("Caminho da imagem salvo.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar a imagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private PersonagemData CarregarDados(string caminhoArquivo)
        {
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    // Lê o arquivo JSON
                    string json = File.ReadAllText(caminhoArquivo);
                    return JsonConvert.DeserializeObject<PersonagemData>(json);
                }
                else
                {
                    return new PersonagemData(); // Retorna um novo objeto caso o arquivo não exista
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                return new PersonagemData(); // Retorna um novo objeto em caso de erro
            }
        }

        private void SalvarDados(PersonagemData personagemData, string caminhoArquivo)
        {
            try
            {
                // Salva os dados no arquivo JSON
                string json = JsonConvert.SerializeObject(personagemData, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, json);
                Console.WriteLine("Dados salvos.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CarregarImagem()
        {
            try
            {
                string caminhoArquivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personagem.json");

                if (File.Exists(caminhoArquivo))
                {
                    // Lê o arquivo JSON
                    var personagemData = CarregarDados(caminhoArquivo);

                    // Carrega a imagem se o caminho estiver presente
                    if (!string.IsNullOrEmpty(personagemData.PlayerImagePath))
                    {
                        PlayerImage = new BitmapImage(new Uri(personagemData.PlayerImagePath));
                    }

                    Console.WriteLine("Imagem carregada do arquivo.");
                }
                else
                {
                    Console.WriteLine("Arquivo não encontrado. Nenhuma imagem carregada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a imagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}