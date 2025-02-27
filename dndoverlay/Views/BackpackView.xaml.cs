using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace dndoverlay.Views
{
    public partial class BackpackView : Page
    {
        private static string pastaMochila = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mochila");
        private List<Item> itens = new List<Item>();
        private List<Item> itensFiltrados = new List<Item>();

        public BackpackView()
        {
            InitializeComponent();
            Loaded += BackpackView_Loaded;
        }

        private void BackpackView_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarItens();
        }

        private void CarregarItens()
        {
            if (ListaItens == null) return;

            ListaItens.ItemsSource = null;
            itens.Clear();
            itensFiltrados.Clear();

            if (!Directory.Exists(pastaMochila))
                Directory.CreateDirectory(pastaMochila);

            string[] arquivos = Directory.GetFiles(pastaMochila, "*.json");

            foreach (string arquivo in arquivos)
            {
                try
                {
                    string conteudo = File.ReadAllText(arquivo);
                    Item item = JsonConvert.DeserializeObject<Item>(conteudo);

                    if (item != null)
                        itens.Add(item);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Erro ao carregar o item: {arquivo}\n{ex.Message}", "Erro de Leitura", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // Verifica se existe um item chamado "Ouro", se não, cria um automaticamente
            Item ouroItem = itens.FirstOrDefault(i => i.Nome == "Ouro");
            if (ouroItem == null)
            {
                ouroItem = new Item
                {
                    Nome = "Ouro",
                    Quantidade = 0,
                    Peso = 0,
                    Preco = 1, // Definimos um preço padrão de 1 moeda de ouro por moeda
                    Raridade = "Comum",
                    Descricao = "Moedas de ouro"
                };

                itens.Add(ouroItem);
                SalvarItens();
            }

            AtualizarOuroUI(ouroItem.Quantidade);

            itensFiltrados = itens.Where(item => item.Nome != "Ouro").ToList();
            ListaItens.ItemsSource = itensFiltrados;

            AtualizarUI();
        }

        public void SalvarItens()
        {
            if (!Directory.Exists(pastaMochila))
                Directory.CreateDirectory(pastaMochila);

            foreach (var item in itens)
            {
                string caminhoArquivo = Path.Combine(pastaMochila, $"{item.Nome}.json");
                string json = JsonConvert.SerializeObject(item, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, json);
            }

            AtualizarUI();
        }

        private void SalvarItem_Editado(object sender, TextChangedEventArgs e)
        {
            // Lógica para salvar as edições feitas no item
            var textBox = sender as TextBox;

            if (textBox != null)
            {
                string novoValor = textBox.Text;

            }
        }


        private void AtualizarUI()
        {
            Item ouroItem = itens.FirstOrDefault(i => i.Nome == "Ouro");
            if (ouroItem != null)
                AtualizarOuroUI(ouroItem.Quantidade);

            CalcularPesoTotal();
        }

        private void AtualizarOuroUI(int quantidade)
        {
            if (OuroTextBox != null)
                OuroTextBox.Text = quantidade.ToString();
        }

        private void CalcularPesoTotal()
        {
            double pesoTotal = itens.Where(item => item.Nome != "Ouro")
                                    .Sum(item => item.Peso * item.Quantidade);

            if (PesoTotalTextBox != null)
                PesoTotalTextBox.Text = pesoTotal.ToString("N2");
        }

        private void AdicionarItem_Click(object sender, RoutedEventArgs e)
        {
            Item novoItem = new Item
            {
                Nome = "Novo Item",
                Raridade = "Comum",
                Quantidade = 1,
                Descricao = "",
                Peso = 0,
                Preco = 0
            };

            itens.Add(novoItem);
            itensFiltrados = itens.Where(item => item.Nome != "Ouro").ToList();

            if (ListaItens != null)
            {
                ListaItens.ItemsSource = null;
                ListaItens.ItemsSource = itensFiltrados;
            }

            AtualizarUI();
        }

        private void ExcluirItem_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is Item itemSelecionado)
            {
                string caminhoArquivo = Path.Combine(pastaMochila, $"{itemSelecionado.Nome}.json");
                if (File.Exists(caminhoArquivo))
                    File.Delete(caminhoArquivo);

                itens.Remove(itemSelecionado);

                if (ListaItens != null)
                {
                    ListaItens.ItemsSource = null;
                    ListaItens.ItemsSource = itens.Where(item => item.Nome != "Ouro").ToList();
                }

                AtualizarUI();
            }
        }

        private void AtualizarItens_Click(object sender, RoutedEventArgs e)
        {
            SalvarItens();
            if (SalvoText != null)
            {
                SalvoText.Visibility = Visibility.Visible;
                IniciarFadeOut(SalvoText);
            }
        }

        private void IniciarFadeOut(UIElement elemento)
        {
            if (elemento == null) return;

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1.5),
                BeginTime = TimeSpan.FromSeconds(1.5)
            };

            fadeOut.Completed += (s, e) => elemento.Visibility = Visibility.Collapsed;
            elemento.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void FiltrarItens(string termoPesquisa)
        {
            if (ListaItens == null) return;

            if (string.IsNullOrWhiteSpace(termoPesquisa))
            {
                itensFiltrados = itens.Where(item => item.Nome != "Ouro").ToList();
            }
            else
            {
                itensFiltrados = itens.Where(item => item.Nome != "Ouro" &&
                                                     item.Nome.Contains(termoPesquisa, StringComparison.OrdinalIgnoreCase))
                                      .ToList();
            }

            ListaItens.ItemsSource = itensFiltrados;
        }

        private void PesquisaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PesquisaTextBox != null)
                FiltrarItens(PesquisaTextBox.Text);
        }

        private void OuroTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (OuroTextBox == null) return;

            if (int.TryParse(OuroTextBox.Text, out int novoValor))
            {
                Item ouroItem = itens.FirstOrDefault(i => i.Nome == "Ouro");
                if (ouroItem != null)
                {
                    ouroItem.Quantidade = Math.Max(0, novoValor); // Evita valores negativos

                    // Atualiza a UI e salva
                    AtualizarUI();
                    SalvarItens();
                }
            }
        }

        public class RaridadeBackgroundConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string raridade = value as string;

                switch (raridade?.ToLower())
                {
                    case "comum":
                        return new SolidColorBrush(Colors.Gray);
                    case "incomum":
                        return new SolidColorBrush(Colors.Green);
                    case "raro":
                        return new SolidColorBrush(Colors.Blue);
                    case "epico":
                        return new SolidColorBrush(Colors.Purple);
                    case "lendario":
                        return new SolidColorBrush(Colors.Orange);
                    default:
                        return new SolidColorBrush(Colors.Transparent);
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return null;
            }
        }

        private void EditarItem_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Item item)
            {
                // Ativar o modo de edição para o item
                item.EstaEditando = !item.EstaEditando; // Alterna entre editar e não editar
                ListaItens.ItemsSource = null; // Forçar atualização da lista
                ListaItens.ItemsSource = itensFiltrados;
            }
        }

        private void ToggleDetalhes_Click(object sender, RoutedEventArgs e)
        {
            // Obtém o botão que foi clicado
            Button botao = sender as Button;
            if (botao == null) return;

            // Obtém o item associado ao botão (StackPanel que contém os detalhes)
            StackPanel parentStack = botao.Parent as StackPanel;
            if (parentStack == null) return;

            // Encontra o painel de detalhes dentro do item
            StackPanel detalhes = parentStack.FindName("DetalhesPanel") as StackPanel;
            if (detalhes == null) return;

            // Alterna a visibilidade
            detalhes.Visibility = (detalhes.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;

            // Alterna o ícone do botão (▶ para ▼)
            botao.Content = (detalhes.Visibility == Visibility.Visible) ? "▼" : "▶";
        }



        public class Item
        {
            public string Nome { get; set; }
            public string Raridade { get; set; }
            public int Quantidade { get; set; }
            public string Descricao { get; set; }
            public double Peso { get; set; }
            public double Preco { get; set; }
            public bool EstaFixado { get; set; }
            public bool EstaEditando { get; set; }
            public string TipoIcone { get; set; }
        }
    }
}
