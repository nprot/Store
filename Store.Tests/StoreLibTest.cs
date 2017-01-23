using System;
using Elasticsearch.Net;
using static Store.Store;
using Nest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Store.Tests
{
    [TestClass]
    public class StoreLibTest
    {
        private static ElasticClient client;
        [TestInitialize]
        public void init()
        {
            client = createClient();
            deleteItems(client);
            for (int i = 0; i < 10; i++)
            {
                addNewItem(client, new Item(i.ToString(), i.ToString(), i, i, i.ToString()));
            }
        }
        [TestMethod]
        public void deleteItemTest()
        {
            bool actual = deleteItem(client,new Item("10","10",10,10,"10"));
            bool expected = false;
            Assert.AreEqual(expected, actual);

            actual = deleteItem(client, new Item("2", "2", 2, 2, "2"));
            expected = true;
            Assert.AreEqual(expected, actual);

            actual = deleteItem(client, new Item("3", "3", 3, 111, "3"));
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void addNewItemTest()
        {
            bool actual = addNewItem(client, new Item("10", "10", 10, 10, "10"));
            bool expected = true;
            Assert.AreEqual(expected, actual);

            actual = addNewItem(client, new Item("2", "2", 2, 2, "2"));
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = addNewItem(client, new Item("22", "22", 22, -10, "22"));
            expected = false;
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void addExistItemTest()
        {
            bool actual = addExistItem(client,"10",666);
            bool expected = false;
            Assert.AreEqual(expected, actual);

            actual = addExistItem(client,"2",10);
            expected = true;
            Assert.AreEqual(expected, actual);

            actual = addExistItem(client, "2", -1);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void isClientNullTest()
        {
            bool actual = isClientNull(client);
            bool expected = false;
            Assert.AreEqual(expected, actual);

            client = null;
            actual = isClientNull(client);
            expected = true;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void correntAmountTest()
        {
            bool actual = correntAmount(10);
            bool expected = true;
            Assert.AreEqual(expected, actual);

            actual = correntAmount(-10);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void deleteItemsTest()
        {
            bool actual = deleteItems(client);
            bool expected = true;
            Assert.AreEqual(expected, actual);

            client = null;
            actual = deleteItems(client);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void showItemsTest()
        {
            bool actual = showItems(client);
            bool expected = true;
            Assert.AreEqual(expected, actual);

            client = null;
            actual = showItems(client);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void searchTest()
        {
            bool actual = search(client, "3");
            bool expected = true;
            Assert.AreEqual(expected, actual);

            actual = search(client, null);
            expected = false;
            Assert.AreEqual(expected, actual);

            client = null;
            actual = search(client, "3");
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void showProfitTest()
        {
            bool actual = showProfit(client);
            bool expected = true;
            Assert.AreEqual(expected, actual);
       
            client = null;
            actual = showProfit(client);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void isExistsTest()
        {
            bool actual = isExists(client, "2",2);
            bool expected = true;
            Assert.AreEqual(expected, actual);

            actual = isExists(client, "2", 4);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = isExists(client, "10", 0);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = isExists(client, "2", -1);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = isExists(null, "2", 2);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void buyItemTest()
        {
            bool actual = buyItem(client, "2",2);
            bool expected = true;
            Assert.AreEqual(expected, actual);

            actual = buyItem(client, "10", 10);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = buyItem(client, "3", -1);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = buyItem(client, "3", 4);
            expected = false;
            Assert.AreEqual(expected, actual);

            actual = buyItem(null, "3", 3);
            expected = false;
            Assert.AreEqual(expected, actual);
        }
    }
}
