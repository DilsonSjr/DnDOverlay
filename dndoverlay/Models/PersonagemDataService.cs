using Newtonsoft.Json;
using System;
using System.IO;

namespace dndoverlay.Models
{
    public static class PersonagemDataService
    {
        // Caminho para a pasta "Status" no diretório do programa
        private static readonly string PastaStatus = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Status");

        // Caminho completo para o arquivo JSON dentro da pasta "Status"
        private static readonly string CaminhoArquivo = Path.Combine(PastaStatus, "personagem.json");

        // Método para salvar os dados do personagem no arquivo JSON
        public static void SalvarDados(PersonagemData personagem)
        {
            try
            {
                // Verifica se a pasta "Status" existe; se não, cria a pasta
                if (!Directory.Exists(PastaStatus))
                {
                    Directory.CreateDirectory(PastaStatus);
                }

                // Serializa os dados do personagem para JSON, com formatação legível
                string json = JsonConvert.SerializeObject(personagem, Formatting.Indented);

                // Salva os dados no arquivo JSON
                File.WriteAllText(CaminhoArquivo, json);
                Console.WriteLine("Dados do personagem salvos com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar os dados do personagem: {ex.Message}");
            }
        }

        // Método para carregar os dados do personagem a partir do arquivo JSON
        public static PersonagemData CarregarDados()
        {
            try
            {
                // Verifica se a pasta "Status" existe; se não, cria a pasta
                if (!Directory.Exists(PastaStatus))
                {
                    Directory.CreateDirectory(PastaStatus);
                }

                // Verifica se o arquivo JSON existe
                if (File.Exists(CaminhoArquivo))
                {
                    // Lê o arquivo JSON
                    string json = File.ReadAllText(CaminhoArquivo);

                    // Desserializa o conteúdo do JSON para um objeto PersonagemData
                    PersonagemData personagem = JsonConvert.DeserializeObject<PersonagemData>(json);
                    Console.WriteLine("Dados do personagem carregados com sucesso.");
                    return personagem;
                }
                else
                {
                    Console.WriteLine("Arquivo não encontrado. Criando um novo arquivo de personagem.");
                    return new PersonagemData();  // Se o arquivo não existir, retorna um novo objeto
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar os dados do personagem: {ex.Message}");
                return new PersonagemData();  // Retorna um novo objeto em caso de erro
            }
        }
    }
}