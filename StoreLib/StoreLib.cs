using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Store
{
    public class Store
    {
        public const int MAX_SIZE = 10000;
        public class Item
        {
            public string id { get; set; }
            public string name { get; set; }
            public string desc { get; set; }
            public int price { get; set; }
            public int amount { get; set; }
            public Item(string _Id, string _Name, int _Price, int _Amount, string _Desc)
            {
                name = _Name;
                id = _Id;
                price = _Price;
                amount = _Amount;
                desc = _Desc;
            }
            public override string ToString()
            {
                return "Id:  " + id + " | Name: " + name + " | Price: " + price + " | Amount: " + amount + " | Desc : " + desc;
            }
            public void print()
            {
                Console.WriteLine(ToString());
            }
        }
        public static ElasticClient createClient()
        {
            var local = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(local, "index");
            var client = new ElasticClient(settings);
            client.CreateIndex(ci => ci
            .Index("index")
            .AddMapping<Item>(m => m.MapFromAttributes()));
            return client;
        }
        public static bool isClientNull(ElasticClient client)
        {
            return client == null;
        }
        public static bool correntAmount(int amount)
        {
            return amount >= 0;
        }
        public static bool deleteItems(ElasticClient client)
        {
            if (isClientNull(client)) return false;
            var res = client.Search<Item>(s => s.From(0).Size(MAX_SIZE));
            foreach (var hit in res.Hits)
            {
                deleteItem(client, hit.Source.id, hit.Source.amount);
            }
            return true;
        }
        public static bool addNewItem(ElasticClient client, Item item)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(item.amount)) return false;
            if (isExists(client, item.id, 0))
            {
                return false;
            }
            client.Index(item, p => p
            .Id(item.id));
            return true;
        }
        public static bool deleteItem(ElasticClient client, Item item)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(item.amount)) return false;
            return deleteItem(client, item.id, item.amount);
        }
        public static bool deleteItem(ElasticClient client, string id, int amount)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(amount)) return false;
            if (!isExists(client, id, amount)) return false;
            Item oldItem = client.Get<Item>(g => g
            .Id(id)).Source;
            Item item = new Item(id, oldItem.name, oldItem.price, oldItem.amount - amount, oldItem.desc);
            client.Delete<Item>(g => g
                .Id(item.id));
            if (item.amount > 0)
            {
                if(!addNewItem(client, item)) return false;
            }
            return true;
        }
        public static bool showItems(ElasticClient client)
        {
            if (isClientNull(client)) return false;
            var res = client.Search<Item>(s => s.From(1).Size(10000).Query(q => q.MatchAll()).SortAscending(o => Convert.ToInt32(o.id)));
            foreach (var hit in res.Hits)
            {
                hit.Source.print();
            }
            return true;
        }
        public static bool search(ElasticClient client, string queryString)
        {
            if (isClientNull(client)) return false;
            if (queryString == null) return false;
            var res = client.Search<Item>(s => s
            .Query(q => q
            .Bool(b => b
            .Should(sh =>
               sh.Match(mt1 => mt1.OnField(f1 => f1.name).Query(queryString)) ||

               sh.Match(mt2 => mt2.OnField(f2 => f2.desc).Query(queryString))
            ))));

            foreach (var hit in res.Hits)
            {
                if (!hit.Source.id.Equals("0"))
                {
                    Console.WriteLine(hit.Source);
                }
            }
            return true;
        }
        public static bool showProfit(ElasticClient client)
        {
            if (isClientNull(client)) return false;
            Console.WriteLine("Profit: " + client.Get<Item>(g => g.Id(0)).Source.price);
            return true;
        }
        public static bool isExists(ElasticClient client, string id, int amount)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(amount)) return false;
            var resGet = client.Get<Item>(g => g
            .Id(id));
            if (!resGet.Found) return false;
            if (resGet.Source.amount < amount) return false;
            client.Refresh();
            return true;
        }
        public static bool addExistItem(ElasticClient client, string id, int amount)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(amount)) return false;
            if (!isExists(client, id, 0))  return false;
            Item oldItem = client.Get<Item>(g => g
            .Id(id)).Source;
            addNewItem(client, new Item(oldItem.id, oldItem.name, oldItem.price, oldItem.amount + amount, oldItem.desc));
            client.Refresh();
            return true;
        }
        public static bool buyItem(ElasticClient client, string id, int amount)
        {
            if (isClientNull(client)) return false;
            if (!correntAmount(amount)) return false;
            if (!isExists(client, id, amount)) return false;
            Item zeroIt = client.Get<Item>(g => g.Id("0")).Source;
            int profit = zeroIt.price + amount * client.Get<Item>(g => g
                .Id(id)).Source.price;
            return deleteItem(client, zeroIt) & addNewItem(client, new Item("0", zeroIt.name, profit, zeroIt.amount, zeroIt.desc)) & deleteItem(client, id, amount);
        }
    }
}
