using System;

namespace dndoverlay.Models
{
    /// Representa os dados de um personagem, incluindo atributos, habilidades, informações de vida, experiência e imagem.
    public class PersonagemData
    {
        // Dados da CharacterView

        public string NomePersonagem { get; set; } = string.Empty;        /// Nome do personagem.
        public string NomeRaca { get; set; } = string.Empty;        /// Raça do personagem.
        public string Antecedente { get; set; } = string.Empty;        /// Antecedente do personagem.
        public string Classe1 { get; set; } = string.Empty;        /// Classe primária do personagem.
        public string Subclasse1 { get; set; } = string.Empty;        /// Subclasse da classe primária.
        public int Level1 { get; set; } = 1;        /// Nível da classe primária.
        public string Classe2 { get; set; } = string.Empty;        /// Classe secundária do personagem.
        public string Subclasse2 { get; set; } = string.Empty;        /// Subclasse da classe secundária.
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
        public string PlayerImagePath { get; set; } = string.Empty;        /// Caminho da imagem do jogador.

        // Dados do HudPersonagem
        public int CurrentLife { get; set; } = 10;        /// Vida atual do personagem.
        public int MaxLife { get; set; } = 10;        /// Vida máxima do personagem.
        public int CurrentXp { get; set; } = 0;        /// Experiência atual do personagem.
        public int MaxXp { get; set; } = 300;        /// Experiência necessária para o próximo nível.
        public int Level { get; set; } = 1;        /// Nível atual do personagem.     
        }
    }
