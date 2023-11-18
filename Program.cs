using System.Diagnostics;
using System.IO.Enumeration;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Xml.Linq;

namespace MJU23v_DTP_T2
{
    internal class Program
    {
        static List<Link> links = new List<Link>();
        class Link
        {
            public string category, group, name, descr, link;
            public Link(string line)
            {
                string[] part = line.Split('|');
                category = part[0];
                group = part[1];
                name = part[2];
                descr = part[3];
                link = part[4];
            }
            public void Print(int i)
            {
                Console.WriteLine($"|{i,-2}|{category,-10}|{group,-10}|{name,-20}|{descr,-40}| {link, -30}");
            }
            public void OpenLink()
            {
                Process application = new Process();
                application.StartInfo.UseShellExecute = true;
                application.StartInfo.FileName = link;
                application.Start();
                application.WaitForExit();
            }
            public string ToString()
            {
                return $"{category}|{group}|{name}|{descr}|{link}";
            }
        }
        static void Main(string[] args)
        {
            bool igång = true;
            string filename = @"..\..\..\links\links.lis";
            AddtoList(filename);
            Console.WriteLine("Välkommen till länklistan! Skriv 'hjälp' för hjälp!");
            do
            {
                Console.Write("> ");
                string[] Inputs = Console.ReadLine().Split();
                string command = Inputs[0];
                if (command == "sluta")
                {
                    Console.WriteLine("Hej då! Välkommen åter!");
                    igång = false;
                }
                else if (command == "hjälp")
                {
                    printHelp();
                }
                else if (command == "ladda")
                {
                    try
                    {
                        if (Inputs.Length == 2) //FIXME: Fel text format i ladd filen ger en "System.IndexOutOfRangeException"
                        {
                            filename = $@"..\..\..\links\{Inputs[1]}";
                        }
                        AddtoList(filename);
                    }
                    catch(FileNotFoundException)
                    {
                        Console.WriteLine($"Gick ej att hitta fil '{Inputs[1]}'!");
                    }
                }
                else if (command == "lista")
                {
                    int i = 0;
                    foreach (Link L in links)
                        L.Print(i++);
                }
                else if (command == "ny")
                {
                    Console.WriteLine("Skapa en ny länk:");
                    string category = nyString("category");
                    string group = nyString("grupp");
                    string name = nyString("namn");
                    string descr = nyString("descr");
                    string link = nyString("länk");
                    Link newLink = new Link($"{category}|{group}|{name}|{descr}|{link}");
                    links.Add(newLink);
                }
                else if (command == "ta")
                {
                    try
                    {
                        if (Inputs[1] == "bort")
                        {
                            for (int i = links.Count - 1; i >= 0; i--)
                            {
                                Link L = links[i];
                                if (L.link == Inputs[2])
                                {
                                    links.RemoveAt(i);
                                    Console.WriteLine("Gaming");
                                }
                            }
                        }
                    }
                    catch(IndexOutOfRangeException)
                    {
                        Console.WriteLine($"Okänt kommando: '{command}'");
                    }
                }
                else if (command == "öppna")
                {
                    try
                    {
                        if (Inputs[1] == "länk")
                        {
                            foreach (Link L in links)
                            {
                                if (L.link == Inputs[2])
                                {
                                    L.OpenLink();
                                }
                            }
                        }
                    }
                    catch (IndexOutOfRangeException)
                    {
                        Console.WriteLine($"Var vänligen att skriva vad du vill öppna");
                    }
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: '{command}'");
                }
            } while (igång == true);
        }

        private static string nyString(string nyString)
        {
            Console.Write($"ange {nyString}: ");
            string userInput = Console.ReadLine();
            return userInput;
        }

        private static void printHelp()
        {
            Console.WriteLine("hjälp                     - skriv ut den här hjälpen");
            Console.WriteLine("ladda 'filnamn'           - sparar innehållet i listan");
            Console.WriteLine("lista                     - visar sparat innehåll i listan");
            Console.WriteLine("ny                        - skapar en ny länk i listan");
            Console.WriteLine("ta bort 'länknamn'        - tar bort en länk i listan");
            Console.WriteLine("öppna länk 'länknamn'     - öppnar den valda länken");
            Console.WriteLine("sluta                     - avsluta programmet");
        }

        private static void AddtoList(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    Link L = new Link(line);
                    links.Add(L);
                    line = sr.ReadLine();
                }
            }
        }
    }
}