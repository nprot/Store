using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Store.Store;


namespace Store
{
    class Client
    {
        static void Main(string[] args)
        {
            //var local = new Uri("http://localhost:9200");
            // var settings = new ConnectionSettings(local, "index");
            //var client = new ElasticClient(settings);
            var client = createClient();
            Console.WriteLine("Welcome Customer");
            string command;
            bool exit = false;
            do
            {
                Console.WriteLine("Available commands: buy , search , show, exit");
                command = Console.ReadLine();
                int amount;
                string id;
                bool success;
                switch (command)
                {
                    case "buy":
                        Console.WriteLine("Item Id");
                        id = Console.ReadLine();
                        Console.WriteLine("Input Amount");
                        if (!Int32.TryParse(Console.ReadLine(), out amount) || amount<= 0)
                        {
                            Console.WriteLine("Wrong Input");
                            continue;
                        }
                        success = buyItem(client, id, amount);
                        if (!success) { Console.WriteLine("Incorrect Id or Amount"); continue; }
                        Console.WriteLine("Done!");
                        break;
                    case "search":
                        Console.WriteLine("Input query");
                        do
                        {
                            string query = Console.ReadLine();
                            search(client, query);
                            Console.WriteLine("\nPress ESC for exit or ENTER to continue");

                        } while (Console.ReadKey().Key != ConsoleKey.Escape);
                        break;
                    case "show":
                        showItems(client);
                        break;
                    case "exit":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }
            }
            while (!exit);
        }
    }
}
