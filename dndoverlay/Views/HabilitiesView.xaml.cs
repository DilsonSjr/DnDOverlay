using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Newtonsoft.Json;

namespace dndoverlay.Views
{
    public partial class HabilitiesView : Page
    {
        private int slotIndex = 1; // Variável para controle do número do slot
        private List<Habilidade> habilidades = new List<Habilidade>();
        private List<Slot> slots = new List<Slot>();
        private string habilidadesDirectory = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "habilidades");

        public HabilitiesView()
        {
            InitializeComponent();

            // Criar a pasta "habilidades" se não existir
            if (!Directory.Exists(habilidadesDirectory))
            {
                Directory.CreateDirectory(habilidadesDirectory);
            }

            // Carregar as habilidades existentes na pasta
            CarregarHabilidades();
            CarregarSlots();
        }

        // Método para carregar as habilidades da pasta
        private void CarregarHabilidades()
        {
            if (Directory.Exists(habilidadesDirectory))
            {
                var arquivos = Directory.GetFiles(habilidadesDirectory, "*.json");
                foreach (var arquivo in arquivos)
                {
                    // Ignorar o arquivo de slots
                    if (arquivo.EndsWith("slots.json"))
                        continue;

                    string json = File.ReadAllText(arquivo);
                    Habilidade habilidade = JsonConvert.DeserializeObject<Habilidade>(json);
                    habilidades.Add(habilidade);
                }
            }

            // Atualizar a lista na interface
            ListaHabilidades.ItemsSource = habilidades;
        }

        // Método para carregar os slots salvos
        private void CarregarSlots()
        {
            string slotsFilePath = System.IO.Path.Combine(habilidadesDirectory, "slots.json");
            if (File.Exists(slotsFilePath))
            {
                string json = File.ReadAllText(slotsFilePath);
                slots = JsonConvert.DeserializeObject<List<Slot>>(json);
                foreach (var slot in slots)
                {
                    StackPanel slotPanel = CriarSlotPanel(slot);
                    SlotsContainer.Children.Add(slotPanel);
                }
            }
        }

        // Evento ao clicar no botão "Adicionar Slot"
        private void AdicionarSlot_Click(object sender, RoutedEventArgs e)
        {
            Slot novoSlot = new Slot
            {
                Nome = $"Slot {slotIndex}",
                Quantidade = 0,
                CorFixa = GerarCorAleatoria() // Gerar uma cor fixa para o slot
            };
            slots.Add(novoSlot);
            StackPanel slotPanel = CriarSlotPanel(novoSlot);
            SlotsContainer.Children.Add(slotPanel);
            slotIndex++;
        }

        // Método para gerar uma cor aleatória
        private Brush GerarCorAleatoria()
        {
            Random random = new Random();
            byte r = (byte)random.Next(0, 256);
            byte g = (byte)random.Next(0, 256);
            byte b = (byte)random.Next(0, 256);
            return new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        // Método para adicionar uma habilidade
        private void AdicionarHabilidade_Click(object sender, RoutedEventArgs e)
        {
            Habilidade novaHabilidade = new Habilidade
            {
                Nome = "Nova Habilidade",
                TipoAcao = "Ação Padrão",
                Nivel = 1,
                Descricao = "Descrição da habilidade"
            };

            habilidades.Add(novaHabilidade);
            ListaHabilidades.ItemsSource = null; // Limpa a lista
            ListaHabilidades.ItemsSource = habilidades; // Atualiza a lista
        }

        // Evento ao clicar no botão "Excluir Habilidade"
        private void ExcluirHabilidade_Click(object sender, RoutedEventArgs e)
        {
            Button botaoExcluir = sender as Button;
            if (botaoExcluir == null) return;

            // Encontrando a habilidade associada ao botão
            Habilidade habilidadeParaExcluir = botaoExcluir.DataContext as Habilidade;
            if (habilidadeParaExcluir != null)
            {
                habilidades.Remove(habilidadeParaExcluir);
                ListaHabilidades.ItemsSource = null; // Limpa a lista
                ListaHabilidades.ItemsSource = habilidades; // Atualiza a lista

                // Remover o arquivo correspondente
                string filePath = System.IO.Path.Combine(habilidadesDirectory, $"{habilidadeParaExcluir.Nome}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        // Evento ao alternar a visibilidade da descrição da habilidade
        private void ToggleDetalhes_Click(object sender, RoutedEventArgs e)
        {
            Button botao = sender as Button;
            if (botao == null) return;

            StackPanel parentStack = botao.Parent as StackPanel;
            if (parentStack == null) return;

            StackPanel detalhes = parentStack.FindName("DetalhesPanel") as StackPanel;
            if (detalhes == null) return;

            detalhes.Visibility = (detalhes.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            botao.Content = (detalhes.Visibility == Visibility.Visible) ? "▼" : "▶";
        }

        // Evento ao editar os campos de habilidades
        private void SalvarHabilidade_Editada(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && ListaHabilidades.SelectedItem is Habilidade habilidadeEditada)
            {
                SalvarHabilidade(habilidadeEditada);
                ListaHabilidades.Items.Refresh();
            }
        }

        // Método para salvar uma habilidade
        private void SalvarHabilidade(Habilidade habilidade)
        {
            string json = JsonConvert.SerializeObject(habilidade, Formatting.Indented);
            string filePath = System.IO.Path.Combine(habilidadesDirectory, $"{habilidade.Nome}.json");

            // Se o arquivo já existe, remove o existente antes de salvar
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Salvar o arquivo JSON na pasta "habilidades"
            File.WriteAllText(filePath, json);
        }

        // Criar o botão de salvar
        private void Salvar_Click(object sender, RoutedEventArgs e)
        {
            // Salvar todas as habilidades
            foreach (var habilidade in habilidades)
            {
                SalvarHabilidade(habilidade);
            }

            // Salvar os slots
            string slotsFilePath = System.IO.Path.Combine(habilidadesDirectory, "slots.json");
            string slotsJson = JsonConvert.SerializeObject(slots, Formatting.Indented);
            File.WriteAllText(slotsFilePath, slotsJson);

            // Exibe o texto "Salvo!"
            SalvoText.Visibility = Visibility.Visible;
            IniciarFadeOut(SalvoText);
        }

        // Criação do slot de habilidade
        private StackPanel CriarSlotPanel(Slot slot)
        {
            StackPanel slotPanel = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(5) };

            // Criando o header com o nome do slot e botão de remoção
            StackPanel headerPanel = CriarHeaderPanel(slot);

            // Campo de quantidade e slots (CheckBoxes)
            TextBox quantidadeTextBox = CriarQuantidadeTextBox(slot);
            WrapPanel slotsPanel = CriarSlotsPanel(slot);

            quantidadeTextBox.TextChanged += (s, ev) => AtualizarCheckBoxes(slotsPanel, quantidadeTextBox, slot);

            // Adicionando os elementos no painel do slot
            StackPanel contentPanel = new StackPanel { Orientation = Orientation.Horizontal };
            contentPanel.Children.Add(quantidadeTextBox);
            contentPanel.Children.Add(slotsPanel);

            slotPanel.Children.Add(headerPanel);
            slotPanel.Children.Add(contentPanel);

            return slotPanel;
        }

        // Criando o cabeçalho do slot
        private StackPanel CriarHeaderPanel(Slot slot)
        {
            StackPanel headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            TextBox nomeTextBox = new TextBox
            {
                Text = slot.Nome,
                Width = 500,
                Margin = new Thickness(5, 0, 5, 0),
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Height = 15, // Diminuir a altura
            };

            nomeTextBox.TextChanged += (s, ev) => slot.Nome = nomeTextBox.Text;

            Button removerButton = new Button
            {
                Content = "🗑",
                Width = 30, // Diminuir largura do botão
                Height = 30, // Diminuir altura do botão
                Background = (Brush)new BrushConverter().ConvertFrom("#3E0000"),
                BorderBrush = Brushes.Transparent,
                Foreground = Brushes.White,
            };
            removerButton.Click += (s, ev) =>
            {
                slots.Remove(slot);
                SlotsContainer.Children.Remove(headerPanel.Parent as StackPanel);
            };

            headerPanel.Children.Add(nomeTextBox);
            headerPanel.Children.Add(removerButton);

            return headerPanel;
        }

        // Criando o TextBox de quantidade
        private TextBox CriarQuantidadeTextBox(Slot slot)
        {
            TextBox textBox = new TextBox
            {
                Width = 12, // Diminuir a largura
                Margin = new Thickness(5),
                Background = (Brush)new BrushConverter().ConvertFrom("#232323"),
                Foreground = Brushes.White,
                TextAlignment = TextAlignment.Center,
                BorderThickness = new Thickness(0),
                Height = 20, // Diminuir a altura
                FontSize = 8, // Reduzir o tamanho da fonte
                FontWeight = FontWeights.Bold,
                VerticalContentAlignment = VerticalAlignment.Center,
                Text = slot.Quantidade.ToString()
            };

            textBox.TextChanged += (s, ev) =>
            {
                if (int.TryParse(textBox.Text, out int quantidade))
                {
                    slot.Quantidade = quantidade;
                }
            };

            return textBox;
        }

        // Criando o painel de CheckBoxes
        private WrapPanel CriarSlotsPanel(Slot slot)
        {
            WrapPanel wrapPanel = new WrapPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };
            AtualizarCheckBoxes(wrapPanel, slot.Quantidade, slot);
            return wrapPanel;
        }

        // Atualizando os CheckBoxes com base na quantidade informada
        private void AtualizarCheckBoxes(WrapPanel slotsPanel, TextBox quantidadeTextBox, Slot slot)
        {
            if (!int.TryParse(quantidadeTextBox.Text, out int quantidade) || quantidade < 0)
                return;

            // Limita a quantidade de CheckBoxes a 12
            if (quantidade > 24)
            {
                quantidade = 24;
            }

            slot.Quantidade = quantidade;
            AtualizarCheckBoxes(slotsPanel, quantidade, slot);
        }

        private void AtualizarCheckBoxes(WrapPanel slotsPanel, int quantidade, Slot slot)
        {
            slotsPanel.Children.Clear();

            // Ajustar a lista de estados para corresponder à quantidade de CheckBoxes
            while (slot.EstadosCheckBoxes.Count < quantidade)
            {
                slot.EstadosCheckBoxes.Add(false); // Adiciona novos estados como false (não marcado)
            }

            // Remover estados extras, se houver
            while (slot.EstadosCheckBoxes.Count > quantidade)
            {
                slot.EstadosCheckBoxes.RemoveAt(slot.EstadosCheckBoxes.Count - 1);
            }

            for (int i = 0; i < quantidade; i++)
            {
                // Verificar se o índice está dentro dos limites da lista
                if (i < slot.EstadosCheckBoxes.Count)
                {
                    CheckBox checkBox = new CheckBox
                    {
                        IsChecked = slot.EstadosCheckBoxes[i],
                        Margin = new Thickness(1),
                        Background = slot.EstadosCheckBoxes[i] ? Brushes.Gray : slot.CorFixa, // Cor fixa ou cinza
                        Width = 15, // Tamanho do CheckBox
                        Height = 15
                    };

                    // Capturar o índice atual em uma variável local para evitar problemas de closure
                    int index = i;

                    checkBox.Checked += (s, ev) =>
                    {
                        if (index < slot.EstadosCheckBoxes.Count)
                        {
                            slot.EstadosCheckBoxes[index] = true;
                            checkBox.Background = Brushes.Gray; // Muda para cinza quando marcado
                        }
                    };

                    checkBox.Unchecked += (s, ev) =>
                    {
                        if (index < slot.EstadosCheckBoxes.Count)
                        {
                            slot.EstadosCheckBoxes[index] = false;
                            checkBox.Background = slot.CorFixa; // Volta para a cor fixa quando desmarcado
                        }
                    };

                    slotsPanel.Children.Add(checkBox);
                }
            }
        }

        // Classe para representar as habilidades
        public class Habilidade
        {
            public string Nome { get; set; }
            public string TipoAcao { get; set; }
            public int Nivel { get; set; }
            public string Descricao { get; set; }
        }

        // Classe para representar os slots
        public class Slot
        {
            public Slot()
            {
                EstadosCheckBoxes = new List<bool>(); // Inicializa a lista de estados
            }

            public string Nome { get; set; }
            public int Quantidade { get; set; }
            public List<bool> EstadosCheckBoxes { get; set; } // Novo campo
            public Brush CorFixa { get; set; } // Cor fixa para o slot
        }

        private void IniciarFadeOut(UIElement elemento)
        {
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
    }
}