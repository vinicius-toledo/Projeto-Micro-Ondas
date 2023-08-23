// VINICIUS FIGUEIREDO TOLEDO

// CONSEGUI FAZER OS NIVEIS 1, 2 E 3.
// FALTOU FAZER A EXPORTAÇÃO DE REGRAS PARA A WEB API.

//NESSE PROJETO CONSEGUIMOS ADICIONAR TEMPO E POTENCIA,
//INICIAÇÃO RAPIDA, ALGUNS ALIMENTOS PRÉ-DEFINIDOS,
//E CADASTRO DE MAIS ALIMENTOS COM UM BANCO DE DADOS.




using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.InteropServices;




namespace MicroOndasApp
{
    class Program
    {
        static bool isHeatingPaused = false;
        static double tempoRestante = 0;
        static double tempoDefinido = 0;
        static double potenciaDefinida = 0;
        static List<ProgramaDeAquecimento> programasCustomizados = new List<ProgramaDeAquecimento>();
        static string customProgramsFile = "customPrograms.json";

        public static void CarregarProgramasCustomizados()
        {
            if (File.Exists(customProgramsFile))
            {
                string json = File.ReadAllText(customProgramsFile);
                programasCustomizados = JsonSerializer.Deserialize<List<ProgramaDeAquecimento>>(json);
            }
        }
        static void Main(string[] args)
        {
            CarregarProgramasCustomizados();

            Console.WriteLine("Bem-vindo ao aquecedor!");
            Console.WriteLine("Pressione o botão de aquecimento para iniciar com potência 10 e tempo 30 segundos.");
            Console.WriteLine("Pressione as teclas de 1 a 5 para escolher um programa de aquecimento pré-definido.");
            Console.WriteLine("Pressione R para cadastrar um novo programa customizado.");

            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter) // Botão de início rápido
                {
                    if (tempoRestante == 0)
                    {
                        RealizarAquecimento(30, 10);
                    }
                    else
                    {
                        if (isHeatingPaused)
                        {
                            isHeatingPaused = false;
                            Console.WriteLine("Aquecimento retomado.");
                        }
                        else
                        {
                            tempoRestante += 30;
                            Console.WriteLine($"Aquecimento estendido. Tempo restante: {tempoRestante} segundos.");
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.P) // Botão de pausa
                {
                    if (!isHeatingPaused && tempoRestante > 0)
                    {
                        isHeatingPaused = true;
                        Console.WriteLine("Aquecimento pausado. Pressione Enter para retomar ou P para pausar.");
                    }
                    else if (isHeatingPaused && tempoRestante > 0)
                    {
                        tempoRestante = 0;
                        isHeatingPaused = false;
                        Console.Clear();
                        Console.WriteLine("Aquecimento cancelado.");
                    }
                    else if (tempoRestante == 0)
                    {
                        tempoDefinido = 0;
                        potenciaDefinida = 0;
                        Console.Clear();
                        Console.WriteLine("Informações de tempo e potência limpas.");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.R) // Cadastrar programa customizado
                {
                    CadastrarProgramaCustomizado();
                }
                else if (keyInfo.Key == ConsoleKey.D1) // Programa de aquecimento pré-definido 1
                {
                    DefinirProgramaPreDefinido("Programa 1", "Pipoca (de micro-ondas)", 180, 7,
                                               "Observar o barulho de estouros do milho," +
                                               " caso haja um intervalo de mais de 10 segundos entre um" +
                                               "\r\nestouro e outro, interrompa o aquecimento.");
                }
                else if (keyInfo.Key == ConsoleKey.D2) // Programa de aquecimento pré-definido 2
                {
                    DefinirProgramaPreDefinido("Programa 2", "Leite", 300, 5,
                                               "Cuidado com o aquecimento de líquidos," +
                                               " o choque térmico aliado ao movimento " +
                                               "do recipiente pode causar fervura imediata, causando risco de queimaduras");
                }
                else if (keyInfo.Key == ConsoleKey.D3) // Programa de aquecimento pré-definido 3
                {
                    DefinirProgramaPreDefinido("Programa 3", "Carne de boi", 840, 4,
                                               "Interrompa o processo na metade e vire o conteúdo" +
                                               "com a parte de baixo para cima para o descongelamento uniforme");
                }
                else if (keyInfo.Key == ConsoleKey.D4) // Programa de aquecimento pré-definido 4
                {
                    DefinirProgramaPreDefinido("Programa 4", "Frango", 480, 7,
                                               "Interrompa o processo na metade e vire o conteúdo " +
                                               "com a parte de baixo para cima para o descongelamento uniforme");
                }
                else if (keyInfo.Key == ConsoleKey.D5) // Programa de aquecimento pré-definido 5
                {
                    DefinirProgramaPreDefinido("Programa 5", "Feijão", 480, 9,
                                               "Deixe o recipiente destampado e em casos de plástico," +
                                               " cuidado ao retirar o recipiente pois o mesmo pode perder" +
                                               " resistência em altas temperaturas");
                }

                static void CadastrarProgramaCustomizado()
                {
                    Console.Clear();
                    Console.WriteLine("Cadastro de programa customizado");
                    Console.Write("Digite o nome do programa: ");
                    string nome = Console.ReadLine();

                    Console.Write("Digite o alimento: ");
                    string alimento = Console.ReadLine();

                    Console.Write("Digite o tempo em segundos: ");
                    double tempo = double.Parse(Console.ReadLine());

                    Console.Write("Digite a potência: ");
                    double potencia = double.Parse(Console.ReadLine());

                    Console.Write("Digite as instruções complementares (opcional): ");
                    string instrucoes = Console.ReadLine();

                    ProgramaDeAquecimento programaCustomizado = new ProgramaDeAquecimento(nome, alimento, tempo, potencia, instrucoes);
                    programasCustomizados.Add(programaCustomizado);

                    Console.WriteLine("Programa customizado cadastrado com sucesso!");
                    Console.WriteLine("Pressione Enter para continuar...");
                    Console.ReadLine();
                    Console.Clear();

                    // Salvar programas customizados em arquivo JSON
                    SalvarProgramasCustomizados();
                }

                static void SalvarProgramasCustomizados()
                {
                    string json = JsonSerializer.Serialize(programasCustomizados);
                    File.WriteAllText(customProgramsFile, json);
                }




                static void DefinirProgramaPreDefinido(string nome, string alimento, double tempo, double potencia, string instrucoes)
                {
                    tempoDefinido = tempo;
                    potenciaDefinida = potencia;
                    Console.Clear();
                    Console.WriteLine($"Programa pré-definido escolhido: {nome}");
                    Console.WriteLine($"Alimento: {alimento}");
                    Console.WriteLine($"Tempo: {tempo} segundos");
                    Console.WriteLine($"Potência: {potencia}");
                    Console.WriteLine($"Instruções: {instrucoes}");

                    RealizarAquecimento(tempoDefinido, potenciaDefinida);
                }

                static void RealizarAquecimento(double tempo, double potencia)
                {
                    if (tempoDefinido > 0 && potenciaDefinida > 0)
                    {
                        tempo = tempoDefinido;
                        potencia = potenciaDefinida;
                        tempoDefinido = 0;
                        potenciaDefinida = 0;
                    }

                    if (tempoRestante > 0 && tempoDefinido > 0)
                    {
                        Console.WriteLine($"Não é permitido acrescentar tempo ao programa pré-definido. Aquecimento iniciado com {tempo} segundos.");
                    }
                    else
                    {
                        Console.WriteLine($"Iniciando aquecimento com potência {potencia} por {tempo} segundos...");
                    }

                    int totalCaracteres = (int)tempo * (int)potencia;
                    string progresso = string.Empty;

                    for (int i = 1; i <= totalCaracteres; i++)
                    {
                        if (isHeatingPaused)
                        {
                            Console.WriteLine("\rAquecimento pausado. Pressione Enter para retomar ou P para pausar.");
                            i--;
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (i % (int)potencia == 0)
                        {
                            progresso += ".";
                            Console.Write($"\rProgresso: {progresso}");
                        }

                        MostrarProgresso(i, totalCaracteres);

                        Thread.Sleep(1000 / (int)potencia);
                    }

                    Console.WriteLine("\rAquecimento concluído.                       ");
                    tempoRestante = 0;
                    isHeatingPaused = false;
                    tempoDefinido = 0;
                    potenciaDefinida = 0;
                }

                static void MostrarProgresso(int caracterAtual, int totalCaracteres)
                {
                    int porcentagem = (caracterAtual * 100) / totalCaracteres;
                    string progresso = new string('=', porcentagem / 5) + new string(' ', 20 - (porcentagem / 5));
                    Console.Write($"\rProgresso: [{progresso}] {porcentagem}%");
                }
            }
        }

        class ProgramaDeAquecimento
        {
            public string Nome { get; }
            public string Alimento { get; }
            public double Tempo { get; }
            public double Potencia { get; }
            public string Instrucoes { get; }

            public ProgramaDeAquecimento(string nome, string alimento, double tempo, double potencia, string instrucoes)
            {
                Nome = nome;
                Alimento = alimento;
                Tempo = tempo;
                Potencia = potencia;
                Instrucoes = instrucoes;
            }
        }
    }

    //Faendo a Exportação de regras para WEB API

 

 







}





