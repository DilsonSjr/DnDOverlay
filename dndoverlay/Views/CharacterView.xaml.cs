using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using dndoverlay.Models;
using System.Windows.Media.Animation;

namespace dndoverlay.Views
{
    public partial class CharacterView : Page, INotifyPropertyChanged
    {
        public CharacterStats Character { get; set; }

        public CharacterView()
        {
            InitializeComponent();
            Character = new CharacterStats();
            DataContext = Character;

            // Carregar os dados após a página ser carregada
            Loaded += (s, e) => CarregarDados(); // isso carrega as checkbox nao apagar
        }

        private void SalvarPersonagem()
        {
            try
            {
                var personagem = ColetarDadosPersonagem();
                if (personagem == null)
                {
                    MessageBox.Show("Erro ao coletar dados do personagem.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string json;
                try
                {
                    json = JsonConvert.SerializeObject(personagem, Formatting.Indented);
                }
                catch (JsonSerializationException ex)
                {
                    MessageBox.Show($"Erro ao serializar o personagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Define o caminho do diretório do programa
                string pastaStatus = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Status");

                // Criar a pasta se não existir
                if (!Directory.Exists(pastaStatus))
                    Directory.CreateDirectory(pastaStatus);

                string caminhoArquivo = System.IO.Path.Combine(pastaStatus, "personagem.json");

                try
                {
                    File.WriteAllText(caminhoArquivo, json);
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"Erro ao salvar o arquivo: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (File.Exists(caminhoArquivo))
                {
                    SalvoText.Visibility = Visibility.Visible;
                    IniciarFadeOut(SalvoText);
                }
                else
                {
                    MessageBox.Show("Falha ao salvar o arquivo.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o personagem: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CarregarDados()
        {
            string pastaStatus = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Status");
            string caminhoArquivo = System.IO.Path.Combine(pastaStatus, "personagem.json");

            if (!Directory.Exists(pastaStatus))
                Directory.CreateDirectory(pastaStatus);

            if (!File.Exists(caminhoArquivo))
            {
                var personagemPadrao = new Personagem();
                string jsonPadrao = JsonConvert.SerializeObject(personagemPadrao, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, jsonPadrao);
            }

            try
            {
                string conteudo = File.ReadAllText(caminhoArquivo);
                var dados = JsonConvert.DeserializeObject<Personagem>(conteudo);

                if (dados != null)
                {
                    // Carregar propriedades existentes
                    NomeRacaTextBox.Text = dados.NomeRaca;
                    AntecedenteTextBox.Text = dados.Antecedente;
                    ClassName1.Text = dados.Classe1;
                    Subclass1.Text = dados.Subclasse1;
                    ValorLevel1.Text = dados.Level1.ToString();
                    ClassName2.Text = dados.Classe2;
                    Subclass2.Text = dados.Subclasse2;
                    LevelValor2.Text = dados.Level2.ToString();
                    DeslocamentoTextBox.Text = dados.Deslocamento.ToString();
                    IniciativaTextBox.Text = dados.Iniciativa.ToString();
                    TamanhoTextBox.Text = dados.Tamanho.ToString();
                    HitDiceTextBox.Text = dados.HitDice.ToString();
                    PercepcaoPassivaTextBox.Text = dados.PercepcaoPassiva.ToString();
                    BonusProficienciaTextBox.Text = dados.BonusProficiencia.ToString();
                    InspiracaoHeroicaTextBox.Text = dados.InspiracaoHeroica.ToString();
                    TreinamentoArmadurasTextBox.Text = dados.TreinamentoArmaduras;
                    TreinamentoArmasTextBox.Text = dados.TreinamentoArmas;
                    TreinamentoFerramentasTextBox.Text = dados.TreinamentoFerramentas;
                    IdiomasConhecidosTextBox.Text = dados.IdiomasConhecidos;
                    ClassImage.Source = new BitmapImage(new Uri(dados.ClassImagePath1, UriKind.Absolute));
                    ClassImage1.Source = new BitmapImage(new Uri(dados.ClassImagePath2, UriKind.Absolute));
                    ResistenciasefraquezasTextBox.Text = dados.Resistenciasefraquezas;

                    // Carregar estados das CheckBoxes
                    CarregarCheckBox(dados.SalvaguardasForca, ForcaCheckBox1, ForcaCheckBox2);
                    CarregarCheckBox(dados.SalvaguardasDestreza, DestrezaCheckBox1, DestrezaCheckBox2);
                    CarregarCheckBox(dados.SalvaguardasConstituicao, ConstituicaoCheckBox1, ConstituicaoCheckBox2);
                    CarregarCheckBox(dados.SalvaguardasInteligencia, InteligenciaCheckBox1, InteligenciaCheckBox2);
                    CarregarCheckBox(dados.SalvaguardasSabedoria, SabedoriaCheckBox1, SabedoriaCheckBox2);
                    CarregarCheckBox(dados.SalvaguardasCarisma, CarismaCheckBox1, CarismaCheckBox2);
                    CarregarCheckBox(dados.AtletismoCheckBox, AtletismoCheckBox1, AtletismoCheckBox2);
                    CarregarCheckBox(dados.AcrobaciaCheckBox, AcrobaciaCheckBox1, AcrobaciaCheckBox2);
                    CarregarCheckBox(dados.FurtividadeCheckBox, FurtividadeCheckBox1, FurtividadeCheckBox2);
                    CarregarCheckBox(dados.PrestidigitacaoCheckBox, PrestidigitacaoCheckBox1, PrestidigitacaoCheckBox2);
                    CarregarCheckBox(dados.ArcanismoCheckBox, ArcanismoCheckBox1, ArcanismoCheckBox2);
                    CarregarCheckBox(dados.HistoriaCheckBox, HistoriaCheckBox1, HistoriaCheckBox2);
                    CarregarCheckBox(dados.InvestigacaoCheckBox, InvestigacaoCheckBox1, InvestigacaoCheckBox2);
                    CarregarCheckBox(dados.NaturezaCheckBox, NaturezaCheckBox1, NaturezaCheckBox2);
                    CarregarCheckBox(dados.ReligiaoCheckBox, ReligiaoCheckBox1, ReligiaoCheckBox2);
                    CarregarCheckBox(dados.AtuacaoCheckBox, AtuacaoCheckBox1, AtuacaoCheckBox2);
                    CarregarCheckBox(dados.EnganacaoCheckBox, EnganacaoCheckBox1, EnganacaoCheckBox2);
                    CarregarCheckBox(dados.IntimidacaoCheckBox, IntimidacaoCheckBox1, IntimidacaoCheckBox2);
                    CarregarCheckBox(dados.PersuasaoCheckBox, PersuasaoCheckBox1, PersuasaoCheckBox2);
                    CarregarCheckBox(dados.IntuicaoCheckBox, IntuicaoCheckBox1, IntuicaoCheckBox2);
                    CarregarCheckBox(dados.LidarAnimaisCheckBox, LidarAnimaisCheckBox1, LidarAnimaisCheckBox2);
                    CarregarCheckBox(dados.MedicinaCheckBox, MedicinaCheckBox1, MedicinaCheckBox2);
                    CarregarCheckBox(dados.PercepcaoCheckBox, PercepcaoCheckBox1, PercepcaoCheckBox2);
                    CarregarCheckBox(dados.SobrevivenciaCheckBox, SobrevivenciaCheckBox1, SobrevivenciaCheckBox2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar os dados: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método auxiliar para carregar estados de CheckBox
        private void CarregarCheckBox(bool[] dadosCheckBox, CheckBox checkBox1, CheckBox checkBox2)
        {
            if (dadosCheckBox != null && dadosCheckBox.Length >= 2)
            {
                checkBox1.IsChecked = dadosCheckBox[0];
                checkBox2.IsChecked = dadosCheckBox[1];
            }
            else
            {
                // Define valores padrão caso o array seja nulo ou tenha tamanho insuficiente
                checkBox1.IsChecked = false;
                checkBox2.IsChecked = false;
            }
        }

        // Método para animar o fade-out do TextBlock
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

        // Método para coletar os dados do personagem
        private Personagem ColetarDadosPersonagem()
        {
            var personagem = new Personagem
            {
                // Propriedades existentes
                NomeRaca = NomeRacaTextBox.Text,
                Antecedente = AntecedenteTextBox.Text,
                Classe1 = ClassName1.Text,
                Subclasse1 = Subclass1.Text,
                Level1 = int.TryParse(ValorLevel1.Text, out int level1) ? level1 : 0,
                Classe2 = ClassName2.Text,
                Subclasse2 = Subclass2.Text,
                Level2 = int.TryParse(LevelValor2.Text, out int level2) ? level2 : 0,
                Forca = Character.Strength,
                Destreza = Character.Dexterity,
                Constituicao = Character.Constitution,
                Inteligencia = Character.Intelligence,
                Sabedoria = Character.Wisdom,
                Carisma = Character.Charisma,
                Deslocamento = int.TryParse(DeslocamentoTextBox.Text, out int deslocamento) ? deslocamento : 0,
                Iniciativa = int.TryParse(IniciativaTextBox.Text, out int iniciativa) ? iniciativa : 0,
                Tamanho = TamanhoTextBox.Text,
                HitDice = HitDiceTextBox.Text,
                PercepcaoPassiva = PercepcaoPassivaTextBox.Text,
                BonusProficiencia = BonusProficienciaTextBox.Text,
                InspiracaoHeroica = InspiracaoHeroicaTextBox.Text,
                TreinamentoArmaduras = TreinamentoArmadurasTextBox.Text,
                TreinamentoArmas = TreinamentoArmasTextBox.Text,
                TreinamentoFerramentas = TreinamentoFerramentasTextBox.Text,
                IdiomasConhecidos = IdiomasConhecidosTextBox.Text,
                Resistenciasefraquezas = ResistenciasefraquezasTextBox.Text,
                ClassImagePath1 = ClassImage.Source?.ToString(), 
                ClassImagePath2 = ClassImage1.Source?.ToString(),

                // Coletar estados das CheckBoxes
                SalvaguardasForca = new bool[] { ForcaCheckBox1.IsChecked ?? false, ForcaCheckBox2.IsChecked ?? false },
                SalvaguardasDestreza = new bool[] { DestrezaCheckBox1.IsChecked ?? false, DestrezaCheckBox2.IsChecked ?? false },
                SalvaguardasConstituicao = new bool[] { ConstituicaoCheckBox1.IsChecked ?? false, ConstituicaoCheckBox2.IsChecked ?? false },
                SalvaguardasInteligencia = new bool[] { InteligenciaCheckBox1.IsChecked ?? false, InteligenciaCheckBox2.IsChecked ?? false },
                SalvaguardasSabedoria = new bool[] { SabedoriaCheckBox1.IsChecked ?? false, SabedoriaCheckBox2.IsChecked ?? false },
                SalvaguardasCarisma = new bool[] { CarismaCheckBox1.IsChecked ?? false, CarismaCheckBox2.IsChecked ?? false },
                AtletismoCheckBox = new bool[] { AtletismoCheckBox1.IsChecked ?? false, AtletismoCheckBox2.IsChecked ?? false },
                AcrobaciaCheckBox = new bool[] { AcrobaciaCheckBox1.IsChecked ?? false, AcrobaciaCheckBox2.IsChecked ?? false },
                FurtividadeCheckBox = new bool[] { FurtividadeCheckBox1.IsChecked ?? false, FurtividadeCheckBox2.IsChecked ?? false },
                PrestidigitacaoCheckBox = new bool[] { PrestidigitacaoCheckBox1.IsChecked ?? false, PrestidigitacaoCheckBox2.IsChecked ?? false },
                ArcanismoCheckBox = new bool[] { ArcanismoCheckBox1.IsChecked ?? false, ArcanismoCheckBox2.IsChecked ?? false },
                HistoriaCheckBox = new bool[] { HistoriaCheckBox1.IsChecked ?? false, HistoriaCheckBox2.IsChecked ?? false },
                InvestigacaoCheckBox = new bool[] { InvestigacaoCheckBox1.IsChecked ?? false, InvestigacaoCheckBox2.IsChecked ?? false },
                NaturezaCheckBox = new bool[] { NaturezaCheckBox1.IsChecked ?? false, NaturezaCheckBox2.IsChecked ?? false },
                ReligiaoCheckBox = new bool[] { ReligiaoCheckBox1.IsChecked ?? false, ReligiaoCheckBox2.IsChecked ?? false },
                AtuacaoCheckBox = new bool[] { AtuacaoCheckBox1.IsChecked ?? false, AtuacaoCheckBox2.IsChecked ?? false },
                EnganacaoCheckBox = new bool[] { EnganacaoCheckBox1.IsChecked ?? false, EnganacaoCheckBox2.IsChecked ?? false },
                IntimidacaoCheckBox = new bool[] { IntimidacaoCheckBox1.IsChecked ?? false, IntimidacaoCheckBox2.IsChecked ?? false },
                PersuasaoCheckBox = new bool[] { PersuasaoCheckBox1.IsChecked ?? false, PersuasaoCheckBox2.IsChecked ?? false },
                IntuicaoCheckBox = new bool[] { IntuicaoCheckBox1.IsChecked ?? false, IntuicaoCheckBox2.IsChecked ?? false },
                LidarAnimaisCheckBox = new bool[] { LidarAnimaisCheckBox1.IsChecked ?? false, LidarAnimaisCheckBox2.IsChecked ?? false },
                MedicinaCheckBox = new bool[] { MedicinaCheckBox1.IsChecked ?? false, MedicinaCheckBox2.IsChecked ?? false },
                PercepcaoCheckBox = new bool[] { PercepcaoCheckBox1.IsChecked ?? false, PercepcaoCheckBox2.IsChecked ?? false },
                SobrevivenciaCheckBox = new bool[] { SobrevivenciaCheckBox1.IsChecked ?? false, SobrevivenciaCheckBox2.IsChecked ?? false }
            };

            return personagem;
        }

        // Evento do botão "Salvar Personagem"
        private void SalvarPersonagem_Click(object sender, RoutedEventArgs e)
        {
            SalvarPersonagem();
        }

        // Métodos existentes
        private void ClassButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender == ClassButton)
            {
                ClassPopup.IsOpen = true;
            }
            else if (sender == ClassButton1)
            {
                ClassPopup1.IsOpen = true;
            }
        }

        private void ClassList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == ClassList && ClassList.SelectedItem is ListBoxItem selectedItem)
            {
                SetClass(ClassImage, ClassName1, ClassPopup, selectedItem);
            }
            else if (sender == ClassList1 && ClassList1.SelectedItem is ListBoxItem selectedItem1)
            {
                SetClass(ClassImage1, ClassName2, ClassPopup1, selectedItem1);
            }
        }

        private void SetClass(Image classImage, TextBox className, Popup popup, ListBoxItem selectedItem)
        {
            if (selectedItem != null)
            {
                string classText = selectedItem.Content.ToString();
                string imagePath = selectedItem.Tag.ToString();

                // Converter o caminho relativo para absoluto
                string absoluteImagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imagePath);

                // Verificar se o arquivo existe
                if (File.Exists(absoluteImagePath))
                {
                    classImage.Source = new BitmapImage(new Uri(absoluteImagePath, UriKind.Absolute));
                }
                else
                {
                    MessageBox.Show($"Imagem não encontrada: {absoluteImagePath}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                className.Text = classText;
                popup.IsOpen = false;
            }
        }

        private void Level_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Adicionar lógica aqui se necessário
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    // Classe para representar os dados do personagem
    public class Personagem : INotifyPropertyChanged
    {
        public Personagem()
        {
            // Inicialização padrão para arrays de CheckBoxes
            SalvaguardasForca = new bool[2];
            SalvaguardasDestreza = new bool[2];
            SalvaguardasConstituicao = new bool[2];
            SalvaguardasInteligencia = new bool[2];
            SalvaguardasSabedoria = new bool[2];
            SalvaguardasCarisma = new bool[2];
            AtletismoCheckBox = new bool[2];
            AcrobaciaCheckBox = new bool[2];
            FurtividadeCheckBox = new bool[2];
            PrestidigitacaoCheckBox = new bool[2];
            ArcanismoCheckBox = new bool[2];
            HistoriaCheckBox = new bool[2];
            InvestigacaoCheckBox = new bool[2];
            NaturezaCheckBox = new bool[2];
            ReligiaoCheckBox = new bool[2];
            AtuacaoCheckBox = new bool[2];
            EnganacaoCheckBox = new bool[2];
            IntimidacaoCheckBox = new bool[2];
            PersuasaoCheckBox = new bool[2];
            IntuicaoCheckBox = new bool[2];
            LidarAnimaisCheckBox = new bool[2];
            MedicinaCheckBox = new bool[2];
            PercepcaoCheckBox = new bool[2];
            SobrevivenciaCheckBox = new bool[2];
        }

        // Propriedades existentes
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
        public int Deslocamento { get; set; }
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
        public string Resistenciasefraquezas { get; set; }
        public string ClassImagePath1 { get; set; }
        public string ClassImagePath2 { get; set; } 
        // Propriedades para as CheckBoxes
        public bool[] SalvaguardasForca { get; set; }
        public bool[] SalvaguardasDestreza { get; set; }
        public bool[] SalvaguardasConstituicao { get; set; }
        public bool[] SalvaguardasInteligencia { get; set; }
        public bool[] SalvaguardasSabedoria { get; set; }
        public bool[] SalvaguardasCarisma { get; set; }
        public bool[] AtletismoCheckBox { get; set; }
        public bool[] AcrobaciaCheckBox { get; set; }
        public bool[] FurtividadeCheckBox { get; set; }
        public bool[] PrestidigitacaoCheckBox { get; set; }
        public bool[] ArcanismoCheckBox { get; set; }
        public bool[] HistoriaCheckBox { get; set; }
        public bool[] InvestigacaoCheckBox { get; set; }
        public bool[] NaturezaCheckBox { get; set; }
        public bool[] ReligiaoCheckBox { get; set; }
        public bool[] AtuacaoCheckBox { get; set; }
        public bool[] EnganacaoCheckBox { get; set; }
        public bool[] IntimidacaoCheckBox { get; set; }
        public bool[] PersuasaoCheckBox { get; set; }
        public bool[] IntuicaoCheckBox { get; set; }
        public bool[] LidarAnimaisCheckBox { get; set; }
        public bool[] MedicinaCheckBox { get; set; }
        public bool[] PercepcaoCheckBox { get; set; }
        public bool[] SobrevivenciaCheckBox { get; set; }

        // Dados do HudPersonagem
        public int NameTextBox { get; set; }
        public int CurrentLife { get; set; }
        public int MaxLife { get; set; }
        public int CurrentXp { get; set; }
        public int MaxXp { get; set; }
        public int Level { get; set; }
        public string PlayerImagePath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}