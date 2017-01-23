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
    class Admin
    {
        static void Main(string[] args)
        {
            var client = createClient();
            deleteItems(client); //uncomment to clear index
            int currentIndex = 1;
            if (isExists(client, "0", 0))
            {
                currentIndex = Math.Max(currentIndex, client.Get<Item>(g => g
                 .Id("0")).Source.amount);
            }
            else{
                addNewItem(client, new Item("0", "max index", 0, 1, "max index"));
            }
            // Console.WriteLine(currentIndex);
            // fill w/ random data
            /*Random rnd = new Random();
            for (int i = 1; i < 30; i++)
            {
                var it = new Item(currentIndex++.ToString(), rnd.Next(1,20).ToString(), rnd.Next(1,100), rnd.Next(1,20), rnd.Next(0,20).ToString());
                // it.print();
                addNewItem(client, it);
            }*/
            Console.WriteLine("Welcome to the store");
           // Console.WriteLine("Type exit to exit");
            showItems(client);
            string command;
            bool exit = false;
            do
            {
                Console.WriteLine("Available commands: add , delete , show , profit, exit");
                command = Console.ReadLine();
                String id, name, desc;
                int amount, price;
                switch (command)
                {
                    case "add":
                        Console.WriteLine("Add existing item? y/n");
                        string res = Console.ReadLine();
                        if (res == "y")
                        {
                            Console.WriteLine("Input Id");
                            id = Console.ReadLine();
                            if (id.Equals("0")) { Console.WriteLine("Wrong Input"); continue; }
                            Console.WriteLine("Input Amount");
                            if (!Int32.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                            {
                                Console.WriteLine("Wrong Input");
                                continue;
                            }
                            if (!addExistItem(client, id, amount)) { Console.WriteLine("Item does not exists"); continue; }
                        }
                        else if (res == "n")
                        {
                            Console.WriteLine("Item Name");
                            name = Console.ReadLine();
                            Console.WriteLine("Input Desc");
                            desc = Console.ReadLine();
                            Console.WriteLine("Input Price");
                            if (!Int32.TryParse(Console.ReadLine(), out price) || price <= 0)
                            {
                                Console.WriteLine("Wrong Input");
                                continue;
                            }
                            Console.WriteLine("Input Amount");
                            if (!Int32.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                            {
                                Console.WriteLine("Wrong Input");
                                continue;
                            }
                            addNewItem(client, new Item(currentIndex++.ToString(), name, price, amount, desc));
                        }
                        else
                        {
                            Console.WriteLine("Wrong Input");
                            continue;
                        }
                        Console.WriteLine("Done!");
                        break;
                    case "delete":
                        Console.WriteLine("Input Id Item");
                        id = Console.ReadLine();
                        if(id.Equals("0")) { Console.WriteLine("Wrong Input"); continue; }
                        Console.WriteLine("Input Amount Item");
                        if (!Int32.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                        {
                            Console.WriteLine("Wrong Input");
                            continue;
                        }
                        bool success = deleteItem(client, id, amount);
                        if (success) { Console.WriteLine("Done!"); }
                        else
                        { Console.WriteLine("Incorrent Id or Amount"); }
                        break;
                    case "show":
                        showItems(client);
                        break;
                    case "exit":
                        exit = true;
                        break;
                    case "profit":
                        showProfit(client);
                        break;
                    default:
                        Console.WriteLine("Wrong Input");
                        break;
                }
            }
            while (!exit);
            //deleteItem(client, client.Get<Item>(g => g.Id("0")).Source);
            //addNewItem(client, new Item("0", "max index", client.Get<Item>(g => g.Id("0")).Source.price, currentIndex, "profit")); // saving max index and profit for next execution
            deleteItems(client); // delete all items
        }
    }
}
