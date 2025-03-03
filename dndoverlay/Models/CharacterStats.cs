using System;
using System.ComponentModel;

namespace dndoverlay.Models
{
    public class CharacterStats : INotifyPropertyChanged
    {
        // Campos privados para os atributos
        private int _strength;
        private int _dexterity;
        private int _constitution;
        private int _intelligence;
        private int _wisdom;
        private int _charisma;

        // Evento para notificar mudanças nas propriedades
        public event PropertyChangedEventHandler PropertyChanged;

        // Propriedades públicas para os atributos
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

        // Propriedades para os modificadores de atributos (calculados automaticamente)
        public int StrengthMod => CalculateModifier(Strength);
        public int DexterityMod => CalculateModifier(Dexterity);
        public int ConstitutionMod => CalculateModifier(Constitution);
        public int IntelligenceMod => CalculateModifier(Intelligence);
        public int WisdomMod => CalculateModifier(Wisdom);
        public int CharismaMod => CalculateModifier(Charisma);

        // Método para calcular o modificador de um atributo
        private int CalculateModifier(int valueBase) => (valueBase / 2) - 5;

        // Método para notificar mudanças nas propriedades
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Método para definir propriedades e notificar mudanças
        private void SetProperty(ref int field, int value, string propertyName, string modifierPropertyName)
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(propertyName);
                OnPropertyChanged(modifierPropertyName);
            }
        }

        // Construtor: Carrega os dados do personagem ao inicializar
        public CharacterStats()
        {
            CarregarDados();
        }

        // Método para carregar os dados do personagem
        public void CarregarDados()
        {
            // Carrega os dados do personagem usando o serviço
            PersonagemData personagem = PersonagemDataService.CarregarDados();

            // Mapeia os dados para as propriedades da classe
            Strength = personagem.Forca;
            Dexterity = personagem.Destreza;
            Constitution = personagem.Constituicao;
            Intelligence = personagem.Inteligencia;
            Wisdom = personagem.Sabedoria;
            Charisma = personagem.Carisma;
        }

        // Método para salvar os dados do personagem
        public void SalvarDados()
        {
            // Cria um objeto PersonagemData com os dados atuais
            PersonagemData personagem = new PersonagemData
            {
                Forca = Strength,
                Destreza = Dexterity,
                Constituicao = Constitution,
                Inteligencia = Intelligence,
                Sabedoria = Wisdom,
                Carisma = Charisma
            };

            // Salva os dados usando o serviço
            PersonagemDataService.SalvarDados(personagem);
        }
    }
}