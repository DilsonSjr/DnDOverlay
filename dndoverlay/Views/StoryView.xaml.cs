using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace dndoverlay.Views
{
    public partial class StoryView : Page
    {
        private readonly string _pastaStatus;
        private readonly string _caminhoArquivo;

        public StoryView()
        {
            _pastaStatus = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Status");
            _caminhoArquivo = Path.Combine(_pastaStatus, "historia.json");

            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            if (File.Exists(_caminhoArquivo))
            {
                try
                {
                    string conteudo = File.ReadAllText(_caminhoArquivo);
                    var dados = JsonConvert.DeserializeObject<DadosHistoria>(conteudo);

                    if (dados != null)
                    {
                        NomeTextBox.Text = dados.Nome;
                        AlinhamentoTextBox.Text = dados.Alinhamento;
                        IdadeTextBox.Text = dados.Idade;
                        GeneroTextBox.Text = dados.Genero;
                        EspecieTextBox.Text = dados.Especie;
                        FacaoTextBox.Text = dados.Facao;
                        AparenciaTextBox.Text = dados.Aparencia;
                        DeidadeTextBox.Text = dados.Deidade;
                        PersonalidadeTextBox.Text = dados.Personalidade;
                        ObjetivoTextBox.Text = dados.Objetivo;
                        BackstoryTextBox.Text = dados.Backstory;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao carregar os dados: {ex.Message}");
                }
            }
        }

        private void SalvarDados()
        {
            try
            {
                if (!Directory.Exists(_pastaStatus))
                    Directory.CreateDirectory(_pastaStatus);

                var dados = new DadosHistoria
                {
                    Nome = NomeTextBox.Text,
                    Alinhamento = AlinhamentoTextBox.Text,
                    Idade = IdadeTextBox.Text,
                    Genero = GeneroTextBox.Text,
                    Especie = EspecieTextBox.Text,
                    Facao = FacaoTextBox.Text,
                    Aparencia = AparenciaTextBox.Text,
                    Deidade = DeidadeTextBox.Text,
                    Personalidade = PersonalidadeTextBox.Text,
                    Objetivo = ObjetivoTextBox.Text,
                    Backstory = BackstoryTextBox.Text
                };

                string json = JsonConvert.SerializeObject(dados, Formatting.Indented);
                File.WriteAllText(_caminhoArquivo, json);

                TextoSalvo.Visibility = Visibility.Visible;
                IniciarFadeOut(TextoSalvo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar os dados: {ex.Message}");
            }
        }

        private static void IniciarFadeOut(UIElement elemento)
        {
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };

            timer.Tick += (s, e) =>
            {
                elemento.Visibility = Visibility.Collapsed;
                timer.Stop();
            };

            timer.Start();
        }

        private void SalvarButton_Click(object sender, RoutedEventArgs e)
        {
            SalvarDados();
        }

        private class DadosHistoria
        {
            public string Nome { get; set; } = string.Empty;
            public string Alinhamento { get; set; } = string.Empty;
            public string Idade { get; set; } = string.Empty;
            public string Genero { get; set; } = string.Empty;
            public string Especie { get; set; } = string.Empty;
            public string Facao { get; set; } = string.Empty;
            public string Aparencia { get; set; } = string.Empty;
            public string Deidade { get; set; } = string.Empty;
            public string Personalidade { get; set; } = string.Empty;
            public string Objetivo { get; set; } = string.Empty;
            public string Backstory { get; set; } = string.Empty;
        }
    }
}