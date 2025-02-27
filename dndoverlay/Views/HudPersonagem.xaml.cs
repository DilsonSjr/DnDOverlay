using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace dndoverlay.Views
{
    public partial class HudPersonagem : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public HudPersonagem()
        {
            InitializeComponent();

            // Inscreva-se nos eventos
            NomePersonagemTextBox.TextChanged += SalvarAutomaticamente;
            DeslocamentoTextBox.TextChanged += SalvarAutomaticamente; // Adicione esta linha
            ClasseArmaduraTextBox.TextChanged += SalvarAutomaticamente; // Adicione esta linha
            CurrentLifeBox.TextChanged += SalvarAutomaticamente;
            MaxLifeBox.TextChanged += SalvarAutomaticamente;
            CurrentXpBox.TextChanged += SalvarAutomaticamente;
            MaxXpBox.TextChanged += SalvarAutomaticamente;
            LevelTextBox.TextChanged += SalvarAutomaticamente;
            HpBar.ValueChanged += SalvarAutomaticamente;
            XpBar.ValueChanged += SalvarAutomaticamente;

            // Inscreva-se no evento PropertyChanged para detectar mudanças na imagem
            PropertyChanged += HudPersonagem_PropertyChanged;

            // Carrega os dados ao iniciar
            string caminhoArquivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personagem.json");
            CarregarDados(caminhoArquivo);
        }

        // Propriedade bindable para a imagem do jogador
        public BitmapImage PlayerImage
        {
            get { return (BitmapImage)playerimage.Source; }
            set
            {
                playerimage.Source = value;
                OnPropertyChanged(nameof(PlayerImage));
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        private void HudPersonagem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PlayerImage))
            {
                // Salva os dados automaticamente quando a imagem é alterada
                string caminhoArquivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personagem.json");
                SalvarDados(caminhoArquivo);
            }
        }

        // Atualiza a barra de HP
        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PlayerImage = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void UpdateLife(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CurrentLifeBox.Text, out int current) &&
                int.TryParse(MaxLifeBox.Text, out int max))
            {
                HpBar.Maximum = max;
                HpBar.Value = current;
            }
        }

        private void HpBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Lógica para quando o valor da barra de HP muda
        }

        private void CurrentLifeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para quando o texto da caixa de vida atual muda
        }

        private void UpdateXp(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CurrentXpBox.Text, out int current) &&
                int.TryParse(MaxXpBox.Text, out int max))
            {
                XpBar.Maximum = max;
                XpBar.Value = current;
            }
        }

        private void LevelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para quando o texto da caixa de nível muda
        }

        private void MaxXpBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lógica para quando o texto da caixa de XP máxima muda
        }

        private PersonagemData ColetarDados()
        {
            var hudData = new PersonagemData();

            // Coleta o nome do personagem
            hudData.NomePersonagem = NomePersonagemTextBox.Text;

            // Coleta o movimento (Deslocamento)
            if (int.TryParse(DeslocamentoTextBox.Text, out int deslocamento))
            {
                hudData.Deslocamento = deslocamento;
            }

            // Coleta a classe de armadura
            if (int.TryParse(ClasseArmaduraTextBox.Text, out int classeArmadura))
            {
                hudData.ClasseArmadura = classeArmadura;
            }

            // Coleta os valores dos TextBoxes e ProgressBars
            if (int.TryParse(CurrentLifeBox.Text, out int currentLife))
            {
                hudData.CurrentLife = currentLife;
            }

            if (int.TryParse(MaxLifeBox.Text, out int maxLife))
            {
                hudData.MaxLife = maxLife;
            }

            if (int.TryParse(CurrentXpBox.Text, out int currentXp))
            {
                hudData.CurrentXp = currentXp;
            }

            if (int.TryParse(MaxXpBox.Text, out int maxXp))
            {
                hudData.MaxXp = maxXp;
            }

            if (int.TryParse(LevelTextBox.Text, out int level))
            {
                hudData.Level = level;
            }

            if (PlayerImage != null)
            {
                hudData.PlayerImagePath = PlayerImage.UriSource.LocalPath;
            }

            return hudData;
        }

        private void SalvarDados(string caminhoArquivo)
        {
            try
            {
                // Carrega os dados existentes do arquivo
                PersonagemData personagemData = null;
                if (File.Exists(caminhoArquivo))
                {
                    string jsonExistente = File.ReadAllText(caminhoArquivo);
                    personagemData = JsonConvert.DeserializeObject<PersonagemData>(jsonExistente);
                }
                else
                {
                    personagemData = new PersonagemData();
                }

                // Atualiza os dados do HudPersonagem
                var hudData = ColetarDados();
                personagemData.NomePersonagem = hudData.NomePersonagem;
                personagemData.Deslocamento = hudData.Deslocamento; // Atualiza o movimento
                personagemData.ClasseArmadura = hudData.ClasseArmadura; // Atualiza a classe de armadura
                personagemData.CurrentLife = hudData.CurrentLife;
                personagemData.MaxLife = hudData.MaxLife;
                personagemData.CurrentXp = hudData.CurrentXp;
                personagemData.MaxXp = hudData.MaxXp;
                personagemData.Level = hudData.Level;
                personagemData.PlayerImagePath = hudData.PlayerImagePath;

                // Salva os dados atualizados no arquivo
                string json = JsonConvert.SerializeObject(personagemData, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, json);

                Console.WriteLine("Dados salvos automaticamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CarregarDados(string caminhoArquivo)
        {
            try
            {
                if (File.Exists(caminhoArquivo))
                {
                    string json = File.ReadAllText(caminhoArquivo);
                    var personagemData = JsonConvert.DeserializeObject<PersonagemData>(json);

                    // Preenche os controles com os dados carregados
                    NomePersonagemTextBox.Text = personagemData.NomePersonagem;
                    DeslocamentoTextBox.Text = personagemData.Deslocamento.ToString(); // Preenche o movimento
                    ClasseArmaduraTextBox.Text = personagemData.ClasseArmadura.ToString(); // Preenche a classe de armadura
                    CurrentLifeBox.Text = personagemData.CurrentLife.ToString();
                    MaxLifeBox.Text = personagemData.MaxLife.ToString();
                    CurrentXpBox.Text = personagemData.CurrentXp.ToString();
                    MaxXpBox.Text = personagemData.MaxXp.ToString();
                    LevelTextBox.Text = personagemData.Level.ToString();

                    if (!string.IsNullOrEmpty(personagemData.PlayerImagePath))
                    {
                        PlayerImage = new BitmapImage(new Uri(personagemData.PlayerImagePath));
                    }

                    Console.WriteLine("Dados carregados com sucesso.");
                }
                else
                {
                    Console.WriteLine("Arquivo de dados não encontrado.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SalvarAutomaticamente(object sender, EventArgs e)
        {
            // Salva os dados automaticamente
            string caminhoArquivo = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personagem.json");
            SalvarDados(caminhoArquivo);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NomePersonagemTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private int quantidadeCliquesPositivos = 0;
        private int quantidadeCliquesNegativos = 0;

        private void AddVida_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CurrentLifeBox.Text, out int currentLife))
            {
                if (int.TryParse(MaxLifeBox.Text, out int maxLife))
                {
                    if (currentLife < maxLife)
                    {
                        currentLife++;
                        CurrentLifeBox.Text = currentLife.ToString();
                        AtualizarHpBar();
                        SalvarAutomaticamente(sender, e);

                        // Incrementa a quantidade de cliques positivos e exibe o texto
                        quantidadeCliquesPositivos++;
                        ExibirVariacaoQuantidade($"+{quantidadeCliquesPositivos}", Colors.Green);
                    }
                }
            }
        }

        private void LessVida_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(CurrentLifeBox.Text, out int currentLife))
            {
                if (currentLife > 0)
                {
                    currentLife--;
                    CurrentLifeBox.Text = currentLife.ToString();
                    AtualizarHpBar();
                    SalvarAutomaticamente(sender, e);

                    // Incrementa a quantidade de cliques negativos e exibe o texto
                    quantidadeCliquesNegativos++;
                    ExibirVariacaoQuantidade($"-{quantidadeCliquesNegativos}", Colors.Red);
                }
            }
        }

        private void ExibirVariacaoQuantidade(string texto, Color cor)
        {
            // Atualiza o texto e a cor
            VariacaoQuantidadeTextBlock.Text = texto;
            VariacaoQuantidadeTextBlock.Foreground = new SolidColorBrush(cor);

            // Define a opacidade inicial como 1 (totalmente visível)
            VariacaoQuantidadeTextBlock.Opacity = 1;

            // Cria uma animação de fade-out
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(3), // Duração de 3 segundos
                BeginTime = TimeSpan.FromSeconds(0) // Começa imediatamente
            };

            // Define o evento para resetar a quantidade de cliques quando a animação terminar
            fadeOut.Completed += (s, _) =>
            {
                quantidadeCliquesPositivos = 0; // Reseta a quantidade de cliques positivos
                quantidadeCliquesNegativos = 0; // Reseta a quantidade de cliques negativos
                VariacaoQuantidadeTextBlock.Text = ""; // Limpa o texto
            };

            // Aplica a animação ao TextBlock
            VariacaoQuantidadeTextBlock.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void AtualizarHpBar()
        {
            if (int.TryParse(CurrentLifeBox.Text, out int currentLife) &&
                int.TryParse(MaxLifeBox.Text, out int maxLife))
            {
                HpBar.Maximum = maxLife;
                HpBar.Value = currentLife;
            }
        }



        public class PersonagemData
        {
            // Dados da CharacterView
            public string NomePersonagem { get; set; }
            public string NomeRaca { get; set; }
            public string Antecedente { get; set; }
            public string Classe1 { get; set; }
            public string Subclasse1 { get; set; }
            public int Level1 { get; set; }
            public string Classe2 { get; set; }
            public string Subclasse2 { get; set; }
            public int Level2 { get; set; }
            public int Forca { get; set; }
            public int Destreza { get; set; }
            public int Constituicao { get; set; }
            public int Inteligencia { get; set; }
            public int Sabedoria { get; set; }
            public int Carisma { get; set; }
            public int Deslocamento { get; set; } // Movimento
            public int ClasseArmadura { get; set; } // Classe de Armadura
            public int Iniciativa { get; set; }
            public string Tamanho { get; set; }
            public string HitDice { get; set; }
            public string PercepcaoPassiva { get; set; }
            public string BonusProficiencia { get; set; }
            public string InspiracaoHeroica { get; set; }
            public string TreinamentoArmaduras { get; set; }
            public string TreinamentoArmas { get; set; }
            public string TreinamentoFerramentas { get; set; }
            public string IdiomasConhecidos { get; set; }

            // Dados do HudPersonagem
            public int CurrentLife { get; set; }
            public int MaxLife { get; set; }
            public int CurrentXp { get; set; }
            public int MaxXp { get; set; }
            public int Level { get; set; }
            public string PlayerImagePath { get; set; }
        }
    }
}