using System;
using System.ComponentModel;

namespace dndoverlay.Models
{
    public class PersonagemData : INotifyPropertyChanged
    {
        // Dados da CharacterView
        public string NomePersonagem { get; set; } = "Nome";        /// Nome do personagem.
        public string NomeRaca { get; set; } = "Especie";        /// Raça do personagem.
        public string Antecedente { get; set; } = "Antecedente";        /// Antecedente do personagem.
        public string Classe1 { get; set; } = "Classe 1";        /// Classe primária do personagem.
        public string Subclasse1 { get; set; } = "SubClasse 1";        /// Subclasse da classe primária.
        public int Level1 { get; set; } = 1;        /// Nível da classe primária.
        public string Classe2 { get; set; } = "Classe 2";        /// Classe secundária do personagem.
        public string Subclasse2 { get; set; } = "SubClasse 2";        /// Subclasse da classe secundária.
        public int Level2 { get; set; } = 0;        /// Nível da classe secundária.
        public int Forca { get; set; } = 10;        /// Valor de Força do personagem.
        public int Destreza { get; set; } = 10;        /// Valor de Destreza do personagem.
        public int Constituicao { get; set; } = 10;        /// Valor de Constituição do personagem.
        public int Inteligencia { get; set; } = 10;        /// Valor de Inteligência do personagem.
        public int Sabedoria { get; set; } = 10;        /// Valor de Sabedoria do personagem.
        public int Carisma { get; set; } = 10;        /// Valor de Carisma do personagem.
        public int Deslocamento { get; set; } = 30;         /// Deslocamento (movimento) do personagem em unidades de distância.
        public int ClasseArmadura { get; set; } = 10;        /// Classe de Armadura (CA) do personagem.
        public int Iniciativa { get; set; } = 0;        /// Valor de Iniciativa do personagem.
        public string Tamanho { get; set; } = "Médio";        /// Tamanho do personagem (Pequeno, Médio, Grande, etc.).
        public string HitDice { get; set; } = "1d8";        /// Dados de vida (Hit Dice) do personagem.
        public string PercepcaoPassiva { get; set; } = "10";        /// Percepção passiva do personagem.
        public string BonusProficiencia { get; set; } = "+2";        /// Bônus de proficiência do personagem.
        public string InspiracaoHeroica { get; set; } = "Não";        /// Indica se o personagem possui inspiração heroica.
        public string TreinamentoArmaduras { get; set; } = string.Empty;        /// Treinamento em armaduras do personagem.
        public string TreinamentoArmas { get; set; } = string.Empty;        /// Treinamento em armas do personagem.
        public string TreinamentoFerramentas { get; set; } = string.Empty;        /// Treinamento em ferramentas do personagem.
        public string IdiomasConhecidos { get; set; } = string.Empty;        /// Idiomas conhecidos pelo personagem.
        public string Resistenciasefraquezas { get; set; } = string.Empty;        /// Resistências e fraquezas do personagem.
        public string PlayerImagePath { get; set; } = string.Empty;        /// Caminho da imagem do jogador.

        // Dados do HudPersonagem
        public int CurrentLife { get; set; } = 10;        /// Vida atual do personagem.
        public int MaxLife { get; set; } = 10;        /// Vida máxima do personagem.
        public int CurrentXp { get; set; } = 0;        /// Experiência atual do personagem.
        public int MaxXp { get; set; } = 300;        /// Experiência necessária para o próximo nível.
        public int Level { get; set; } = 1;        /// Nível atual do personagem.

        // Arrays para as CheckBoxes
        public bool[] SalvaguardasForca { get; set; } = new bool[2];        /// Salvaguardas de Força.
        public bool[] SalvaguardasDestreza { get; set; } = new bool[2];        /// Salvaguardas de Destreza.
        public bool[] SalvaguardasConstituicao { get; set; } = new bool[2];        /// Salvaguardas de Constituição.
        public bool[] SalvaguardasInteligencia { get; set; } = new bool[2];        /// Salvaguardas de Inteligência.
        public bool[] SalvaguardasSabedoria { get; set; } = new bool[2];        /// Salvaguardas de Sabedoria.
        public bool[] SalvaguardasCarisma { get; set; } = new bool[2];        /// Salvaguardas de Carisma.
        public bool[] AtletismoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Atletismo.
        public bool[] AcrobaciaCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Acrobacia.
        public bool[] FurtividadeCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Furtividade.
        public bool[] PrestidigitacaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Prestidigitação.
        public bool[] ArcanismoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Arcanismo.
        public bool[] HistoriaCheckBox { get; set; } = new bool[2];        /// CheckBoxes de História.
        public bool[] InvestigacaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Investigação.
        public bool[] NaturezaCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Natureza.
        public bool[] ReligiaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Religião.
        public bool[] AtuacaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Atuação.
        public bool[] EnganacaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Enganação.
        public bool[] IntimidacaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Intimidação.
        public bool[] PersuasaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Persuasão.
        public bool[] IntuicaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Intuição.
        public bool[] LidarAnimaisCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Lidar com Animais.
        public bool[] MedicinaCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Medicina.
        public bool[] PercepcaoCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Percepção.
        public bool[] SobrevivenciaCheckBox { get; set; } = new bool[2];        /// CheckBoxes de Sobrevivência.
        public string ClassImagePath1 { get; set; } = string.Empty;        
        public string ClassImagePath2 { get; set; } = string.Empty;

        // Implementação do INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}