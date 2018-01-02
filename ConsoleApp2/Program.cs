using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static List<int> GerarDezenas()
        {
            var random = new Random();
            var list = new List<int>
            {
                random.Next(1, 99),
                random.Next(1, 99),
                random.Next(1, 99),
                random.Next(1, 99),
                random.Next(1, 99),
                random.Next(1, 99)
            };
            return list;
        }

        static List<List<int>> LerAquivo(string path)
        {
            var list = new List<List<int>>();
            using (var streamReader = new System.IO.StreamReader(path))
            {
                while (streamReader.EndOfStream == false)
                {
                    var linha = streamReader.ReadLine();
                    var dezenas = linha.Split('|')
                        .Select(dezena => Convert.ToInt32(dezena))
                        .ToList();
                    if (dezenas.Count() == 6)
                    {
                        list.Add(dezenas);
                    }
                }
            }
            return list;
        }

        static void ImprimeDezenas(List<int> dezenas)
        {
            Console.WriteLine(string.Join("|",
                dezenas.Select(x => x.ToString("D2"))));
        }

        static bool VerificaDezenaDuplicada(List<List<int>> listDezenas, List<int> dezenas)
        {
            foreach (var num in listDezenas)
            {
                if (ComparaDezenas(num, dezenas))
                {
                    return true;
                }
            }
            return false;
        }

        static bool ComparaDezenas(List<int> dezenas1, List<int> dezenas2)
        {
            if (dezenas1.Count == 6 && dezenas2.Count == 6)
            {
                for (var indice = 0; indice < 6; indice++)
                {
                    if (dezenas1[indice] != dezenas2[indice])
                        return false;
                }
                return true;
            }
            return false;
        }

        static void SalvarArquivo(List<List<int>> listDezenas, string caminhoArquivo)
        {
            var arquivo = string.Join("\n", 
                listDezenas.Select(
                    dezenas => string.Join("|", 
                        dezenas
                            .Select(
                                dezena => dezena.ToString("D2")))));

            var diretorio = System.IO.Path.GetDirectoryName(caminhoArquivo);
            if (!System.IO.Directory.Exists(diretorio))
            {
                System.IO.Directory.CreateDirectory(diretorio);
            }

            System.IO.File.WriteAllText(caminhoArquivo, arquivo);
        }

        static void Main(string[] args)
        {
            var caminho = System.IO.Directory.GetCurrentDirectory();
            var arquivo = "numeros_mega_sena.txt";
            var path = System.IO.Path.Combine(caminho, arquivo);

            var numHistoricoMega = LerAquivo(path);
           
            Console.WriteLine("=============================================");
            Console.WriteLine("GERADOR DE APOSTAS");
            Console.WriteLine("=============================================");

            Console.WriteLine("INFORME O NÚMERO DE APOSTAS:");
            var numApostas = Convert.ToInt32(Console.ReadLine());

            var numerosGerados = new List<List<int>>();

            for (var indice = 0; indice < numApostas; indice++)
            {
                while (true)
                {
                    var dezenaGerada = GerarDezenas();

                    if (VerificaDezenaDuplicada(numerosGerados, dezenaGerada))
                        continue;

                    if (VerificaDezenaDuplicada(numHistoricoMega, dezenaGerada))
                        continue;

                    numerosGerados.Add(dezenaGerada);
                    break;
                }
            }

            foreach(var num in numerosGerados)
            {
                ImprimeDezenas(num);
            }

            Console.ReadKey();
        }
    }
}
