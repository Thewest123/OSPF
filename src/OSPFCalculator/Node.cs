using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace OSPFCalculator
{
    public class Node
    {
        public string Name;
        public List<Route> Routes = new List<Route>();

        [JsonIgnore] //Aby se tento List neukladal to vysledneho JSON souboru
        public List<Node> Neighbors = new List<Node>();

        private List<Route> NewRoutesToAdd = new List<Route>();
        private List<Route> NewRoutesToRemove = new List<Route>();

        //Constructor
        public Node(string name, List<Route> routes)
        {
            Name = name;
            Routes = routes;
        }

        //Metoda pro pridani nove routy
        public void AddNewRoute(string destination, int cost, string nexthop, bool isChanged = false, bool isNew = false)
        {
            NewRoutesToAdd.Add(new Route(destination, cost, nexthop, isChanged, isNew));
        }

        //Metoda pro smazani stare routy
        public void RemoveRoute(Route r)
        {
            NewRoutesToRemove.Add(r);
        }

        //Ulozeni aktualizace o novych a smazanych routach
        public void SaveNewInfo()
        {
            Routes.AddRange(NewRoutesToAdd);
            NewRoutesToAdd.Clear();

            foreach (var route in NewRoutesToRemove)
            {
                Routes.Remove(route);
                Debug.WriteLine($"Nodu {Name} smazána cesta k {route.Destination} přes {route.NextHop} s cennou {route.Cost}");
            }
            NewRoutesToRemove.Clear();
        }

        //Ziskani routovaci tabulky od nodu n
        public void GetRoutesFrom(Node n)
        {
            foreach (var novaRoute in n.Routes)
            {
                //Kontrola, aby node nepridaval cestu sam na sebe
                if (novaRoute.Destination != Name)
                {
                    //Kontrola, zda jiz dannou cestu node v tabulce ma
                    List<Route> existujiciRouty = Routes.FindAll(x => x.Destination == novaRoute.Destination);

                    //Pokud ji ma, kontroluje cenu, jestli je nova route vyhodnejsi
                    if (existujiciRouty.Count > 0)
                    {
                        foreach (var existujiciRoute in existujiciRouty)
                        {
                            //Kontrola ceny
                            if (novaRoute.Cost + Routes.Find(x => x.Destination == n.Name).Cost < existujiciRoute.Cost)
                            {
                                string dest = novaRoute.Destination;
                                int cost = novaRoute.Cost + Routes.Find(x => x.Destination == n.Name).Cost;
                                string nexthop = n.Name;

                                //Smazani existujici cesty
                                RemoveRoute(existujiciRoute);

                                //Pokud existujici cesta byla v teto stage pridana jako nova, nechat zmenenou cestu take jako novou
                                if (existujiciRoute.IsNew)
                                {
                                    AddNewRoute(dest, cost, nexthop, isNew: true);
                                }
                                //Jinak pokud byla jen zemenena, oznacit jako zmenenou
                                else
                                {
                                    AddNewRoute(dest, cost, nexthop, isChanged: true);
                                }
                                
                                Debug.WriteLine($"Nodu {Name} PŘEPSÁNA cesta - nová vede do {dest} přes {nexthop} s cennou {cost}");
                            }
                        }
                    }
                    //Pokud ji nema, pridava ji jako novou
                    else
                    {
                        string dest = novaRoute.Destination;
                        int cost = novaRoute.Cost + Routes.Find(x => x.Destination == n.Name).Cost;
                        string nexthop = n.Name;

                        //Nastaveni cesty jako nove
                        AddNewRoute(dest, cost, nexthop, isNew: true);

                        Debug.WriteLine($"Nodu {Name} přidána NOVÁ cesta do {dest} přes {nexthop} s cennou {cost}");
                    }
                }

                //Ulozeni aktualizace o novych a smazanych routach
                SaveNewInfo();
            }
        }

        public bool DoContinue()
        {
            if (Routes.Exists(r => r.IsChanged && r.IsNew)) return true;
            else return false;
        }
    }
}
