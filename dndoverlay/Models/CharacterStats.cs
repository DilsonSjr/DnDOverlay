using System;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;

namespace dndoverlay.Models
{
    public class CharacterStats : INotifyPropertyChanged
    {
        private int _strength;
        private int _dexterity;
        private int _constitution;
        private int _intelligence;
        private int _wisdom;
        private int _charisma;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Strength
        {
            get => _strength;
            set => SetProperty(ref _strength, value, nameof(Strength), nameof(StrengthMod));
        }

        public int Dexterity
        {
            get => _dexterity;
            set => SetProperty(ref _dexterity, value, nameof(Dexterity), nameof(DexterityMod));
        }

        public int Constitution
        {
            get => _constitution;
            set => SetProperty(ref _constitution, value, nameof(Constitution), nameof(ConstitutionMod));
        }

        public int Intelligence
        {
            get => _intelligence;
            set => SetProperty(ref _intelligence, value, nameof(Intelligence), nameof(IntelligenceMod));
        }

        public int Wisdom
        {
            get => _wisdom;
            set => SetProperty(ref _wisdom, value, nameof(Wisdom), nameof(WisdomMod));
        }

        public int Charisma
        {
            get => _charisma;
            set => SetProperty(ref _charisma, value, nameof(Charisma), nameof(CharismaMod));
        }

        public int StrengthMod => CalculateModifier(Strength);
        public int DexterityMod => CalculateModifier(Dexterity);
        public int ConstitutionMod => CalculateModifier(Constitution);
        public int IntelligenceMod => CalculateModifier(Intelligence);
        public int WisdomMod => CalculateModifier(Wisdom);
        public int CharismaMod => CalculateModifier(Charisma);

        private int CalculateModifier(int valueBase) => (valueBase / 2) - 5;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetProperty(ref int field, int value, string propertyName, string modifierPropertyName)
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged(modifierPropertyName);
            }
        }

        // 🔹 Construtor: Carrega os valores do JSON
        public CharacterStats()
        {
            LoadStatsFromJson();
        }

        // 🔹 Função para carregar os atributos do JSON
        private void LoadStatsFromJson()
        {
            string pastaStatus = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Status");
            string caminhoArquivo = Path.Combine(pastaStatus, "personagem.json");

            // Se o arquivo não existir, cria um JSON padrão
            if (!Directory.Exists(pastaStatus))
                Directory.CreateDirectory(pastaStatus);

            if (!File.Exists(caminhoArquivo))
            {
                var personagemPadrao = new
                {
                    Forca = 10,
                    Destreza = 10,
                    Constituicao = 10,
                    Inteligencia = 10,
                    Sabedoria = 10,
                    Carisma = 10
                };

                string jsonPadrao = JsonConvert.SerializeObject(personagemPadrao, Formatting.Indented);
                File.WriteAllText(caminhoArquivo, jsonPadrao);
            }

            // Agora tenta carregar os valores do arquivo
            try
            {
                string conteudo = File.ReadAllText(caminhoArquivo);
                dynamic dados = JsonConvert.DeserializeObject<dynamic>(conteudo);

                // Usar SetProperty para garantir que os eventos sejam acionados
                SetProperty(ref _strength, (int)dados.Forca, nameof(Strength), nameof(StrengthMod));
                SetProperty(ref _dexterity, (int)dados.Destreza, nameof(Dexterity), nameof(DexterityMod));
                SetProperty(ref _constitution, (int)dados.Constituicao, nameof(Constitution), nameof(ConstitutionMod));
                SetProperty(ref _intelligence, (int)dados.Inteligencia, nameof(Intelligence), nameof(IntelligenceMod));
                SetProperty(ref _wisdom, (int)dados.Sabedoria, nameof(Wisdom), nameof(WisdomMod));
                SetProperty(ref _charisma, (int)dados.Carisma, nameof(Charisma), nameof(CharismaMod));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar os atributos: {ex.Message}");

                // Se falhar, usa valores padrão
                SetProperty(ref _strength, 10, nameof(Strength), nameof(StrengthMod));
                SetProperty(ref _dexterity, 10, nameof(Dexterity), nameof(DexterityMod));
                SetProperty(ref _constitution, 10, nameof(Constitution), nameof(ConstitutionMod));
                SetProperty(ref _intelligence, 10, nameof(Intelligence), nameof(IntelligenceMod));
                SetProperty(ref _wisdom, 10, nameof(Wisdom), nameof(WisdomMod));
                SetProperty(ref _charisma, 10, nameof(Charisma), nameof(CharismaMod));
            }
        }
    }
}
