using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Newtonsoft.Json;

namespace OSPFCalculator
{
    public class Program
    {
        public static Node nodeToShow;
        static void Main(string[] args)
        {
            //Vstup od uzivatele, zadani cesty k souboru
            Console.Write($"Zadejte cestu k input souboru (\"input.json\") (pro defaultní stiskněte Enter): ");
            string vstup = Console.ReadLine();
            string inputFilePath = vstup == "" ? "input.json" : vstup;

            //Kontrola existence souboru
            if (!File.Exists(inputFilePath))
            {
                throw new FileNotFoundException($"Musí existovat soubor \"{inputFilePath}\" se vstupní topologií (sousedy)", inputFilePath);
            }

            //Nacteni informaci ze souboru
            List<Node> nodes = JsonConvert.DeserializeObject<List<Node>>(File.ReadAllText(inputFilePath));

            //Vstup od uzivatele, jaky node chce zobrazovaz
            Console.Write($"Zadejte jméno nodu, pro který chcete zobrazovat stage routovací tabulky: ");
            string nodeToShowString = Console.ReadLine().ToUpper();

            //Prirazeni nodu se stejnym jmenem jako byl vstup
            nodeToShow = nodes.Find(x => x.Name == nodeToShowString);


            //Vypis
            Console.WriteLine();
            Console.WriteLine($"Routovací tabulky nodu: {nodeToShow.Name}");
            Console.WriteLine($"Formát tabulky je ve tvaru: Destination | Cost | Next-Hop");
            Console.WriteLine();

            //Vypis puvodniho vstupu
            Console.WriteLine($"--- [Input - neighbors - Stage 0] ---");
            foreach (var n in nodes)
            {
                foreach (var r in n.Routes)
                {
                    //Nastavenich puvodnich hodnot vstupu jako sousedy
                    n.Neighbors.Add(nodes.Find(x => x.Name == r.NextHop));
                    
                    //Vypis pouze nodu, ktery si zvolil uzivatel
                    if (n == nodeToShow)
                    {
                        Console.WriteLine($"{r.Destination,10} | {r.Cost,3} | {r.NextHop,-10}");
                    }
                }
            }

            //Vypis vypocitanych stage
            int stage = 0;
            bool pokracovat = true;
            while (pokracovat)
            {
                Console.WriteLine($"------------- [Stage {++stage:##}] -------------");
                Debug.WriteLine($"------------- [Stage {stage:##}] -------------");

                pokracovat = false;
                bool kontrolovat = true;
                foreach (var n in nodes)
                {
                    foreach (var neigh in n.Neighbors)
                    {
                        n.GetRoutesFrom(neigh);
                    }

                    //Kontrola, zda ma jakykoliv node routu, ktera je nova nebo zmenena. Pokud ne, je vse spocitano a program lze ukoncit
                    bool Match(Route r) => r.IsChanged || r.IsNew;
                    if (n.Routes.Exists(Match) && kontrolovat)
                    {
                        pokracovat = true;
                        kontrolovat = false;
                    }

                    foreach (var r in n.Routes)
                    {
                        //Vypis pouze nodu, ktery si zvolil uzivatel
                        if (n == nodeToShow)
                        {
                            Console.WriteLine($"{r.Destination,10} | {r.Cost,3} | {r.NextHop,-10} {(r.IsNew ? "(nová)" : "")}{(r.IsChanged ? "(změněná)" : "")}");
                        }

                        r.IsChanged = false;
                        r.IsNew = false;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Stiskněte Enter pro ukončení");
            //Ulozeni finalnich routovacich tabulek do souboru
            File.WriteAllText("output.json", JsonConvert.SerializeObject(nodes, Formatting.Indented));

            Console.ReadLine();
        }
    }
}
