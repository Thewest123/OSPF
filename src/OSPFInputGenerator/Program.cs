using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OSPFCalculator;

namespace OSPFInputGenerator
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Pro ukončení zadávání NODŮ nic nezadávejte, jen stiskněte Enter");
            Console.WriteLine("Pro ukončení zadávání SOUSEDŮ nic nezadávejte, jen stiskněte Enter");
            Console.WriteLine();

            List<Node> nodes = new List<Node>();
            while (true)
            {
                Console.Write("Zadejte jméno nodu: ");
                string nodeName = Console.ReadLine().ToUpper();

                if (nodeName == "") break;

                List<Route> routes = new List<Route>();
                while (true)
                {
                    Console.WriteLine();
                    Console.Write("     Zadejte jméno souseda: ");
                    string destinationName = Console.ReadLine().ToUpper();

                    if (destinationName == "") break;

                    Console.Write("     Zadejte cenu k sousedovi: ");
                    string costString = Console.ReadLine();
                    int costInt = Convert.ToInt32(costString);

                    routes.Add(new Route(destinationName, costInt, destinationName));
                }

                Console.WriteLine();

                nodes.Add(new Node(nodeName, routes));
            }

            File.WriteAllText("input.json", JsonConvert.SerializeObject(nodes, Formatting.Indented));
        }
    }
}
