using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagyarorszagAdatbazis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EgyedisegElemzese("helyforr.dat");
            EgyedisegElemzese("iranyitoszamok.csv");
            EgyedisegElemzese("koordinatak.csv");
            Console.ReadKey();
        }

        static string Ekezetlenites(string ekezetes)
        {
            char[] atalakitando = { 'á', 'Á', 'é', 'É', 'ó', 'Ó', 'í', 'Í', 'ö', 'Ö', 'ő', 'Ő', 'ü', 'Ü', 'ű', 'Ű', 'ú', 'Ú' };
            char[] atalakitott = { 'a', 'A', 'e', 'E', 'o', 'O', 'i', 'I', 'o', 'O', 'o', 'O', 'u', 'U', 'u', 'U', 'u', 'U' };
            Dictionary<char, char> ek = atalakitando.Zip(atalakitott, (e, t) => new { e, t }).ToDictionary(x => x.e, x => x.t);

            StringBuilder ekezetlen = new StringBuilder();
            foreach (char c in ekezetes)
            {
                ekezetlen.Append(ek.ContainsKey(c) ? ek[c] : c);
            }
            return ekezetlen.ToString();
        }

        static void EgyedisegElemzese(string file)
        {
            int pozicio = -1;
            int kezdo = -1;
            char szeparator = '\0';

            switch (file)
            {
                case "helyforr.dat":
                    pozicio = 3;
                    szeparator = ' ';
                    kezdo = 0;
                    break;
                case "iranyitoszamok.csv":
                    pozicio = 1;
                    szeparator = ';';
                    kezdo = 1;
                    break;
                case "koordinatak.csv":
                    pozicio = 0;
                    szeparator = ';';
                    kezdo = 1;
                    break;
                default:
                    return;
            }

            Dictionary<string, int> szotar = new Dictionary<string, int>();

            using (StreamReader olvaso = new StreamReader(file))
            {
                int db = -1;
                while (!olvaso.EndOfStream)
                {
                    string sor = olvaso.ReadLine();
                    db++;
                    if (db >= kezdo)
                    {
                        string[] reszek = sor.Split(szeparator);
                        string key = Ekezetlenites(reszek[pozicio]);
                        if (szotar.TryGetValue(key, out int count))
                        {
                            szotar[key] = count + 1;
                        }
                        else
                        {
                            szotar[key] = 1;
                        }
                    }
                }
            }

            var ismetlodo = szotar.Where(x => x.Value > 1).Select(x => x.Key).ToList();


            if (ismetlodo.Any())
            {
                Console.WriteLine($"Duplicated entries in {file}:");
                foreach (var item in ismetlodo)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine($"No duplicates found in {file}.");
            }
        }
    }
}

