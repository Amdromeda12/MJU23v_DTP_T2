﻿using System.Diagnostics;
using System.IO.Enumeration;
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
            string filename = @"..\..\..\links\links.lis";
            AddtoList(filename);
            Console.WriteLine("Välkommen till länklistan! Skriv 'hjälp' för hjälp!");
            do
            {
                Console.Write("> ");
                string cmd = Console.ReadLine().Trim();
                string[] arg = cmd.Split();
                string command = arg[0];
                if (command == "sluta")
                {
                    Console.WriteLine("Hej då! Välkommen åter!");
                }
                else if (command == "hjälp")
                {
                    printHelp();
                }
                else if (command == "ladda")
                {
                    if (arg.Length == 2)
                    {
                        filename = $@"..\..\..\links\{arg[1]}";
                    }
                    AddtoList(filename);
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
                    Console.Write("  ange kategori: ");
                    string category = Console.ReadLine();
                    Console.Write("  ange grupp: ");
                    string group = Console.ReadLine();
                    Console.Write("  ange namn: ");
                    string name = Console.ReadLine();
                    Console.Write("  ange beskrivning: ");
                    string descr = Console.ReadLine();
                    Console.Write("  ange länk: ");
                    string line = Console.ReadLine();
                    Link L = new Link(line);
                    links.Add(L);
                }
                else if (command == "spara")
                {
                    if (arg.Length == 2)
                    {
                        filename = $@"..\..\..\links\{arg[1]}";
                    }
                    AddtoList(filename);
                }
                else if (command == "ta")
                {
                    if (arg[1] == "bort")
                    {
                        links.RemoveAt(Int32.Parse(arg[2]));
                    }
                }
                else if (command == "öppna")
                {
                    if (arg[1] == "länk")
                    {
                        foreach (Link L in links)
                        {
                            if (L.link == arg[2])
                            {
                                L.OpenLink();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Okänt kommando: '{command}'");
                }
            } while (true);
        }

        private static void printHelp()
        {
            Console.WriteLine("hjälp           - skriv ut den här hjälpen");
            Console.WriteLine("sluta           - avsluta programmet");
            //TODO: lägg in alla kommandos som saknas
        }

        private static void AddtoList(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                int i = 0;
                string line = sr.ReadLine();
                while (line != null)
                {
                    Console.WriteLine(line);
                    Link L = new Link(line);
                    links.Add(L);
                    line = sr.ReadLine();
                }
            }
        }
    }
}