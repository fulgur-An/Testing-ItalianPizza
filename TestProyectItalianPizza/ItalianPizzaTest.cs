using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestProyectItalianPizza.TestAntonio;
using TestProyectItalianPizza.TestAntonio.Contracts;
using TestProyectItalianPizza.TestAntonio.Models;

namespace TestProyectItalianPizza
{
    [TestClass]
    public class UnitTest1
    {

        ItalianPizzaService italianPizzaService = new ItalianPizzaService();

        #region successful
        [TestMethod]
        public void GetItemListTestSuccessful()
        {
            string filter = "harina";
            int filterInt = 1;
            int option = 1;


            List<ItemContract> itemList;

            itemList = italianPizzaService.GetItemList(filter, filterInt, option);

            Assert.IsTrue(itemList.Count > 0);

        }

        [TestMethod]
        public void GetItemSpecificationTestSuccessful()
        {
            int idItem = 2;


            ItemContract item;

            item = italianPizzaService.GetItemSpecification(idItem);

            Assert.AreEqual(item.IdItem, idItem);

        }


        [TestMethod]
        public void RegisterItemTestSuccessful()
        {
            ItemContract item = new ItemContract
            {
                Name = "Harina",
                Description = "Prueba de harina",
                Sku = 1,
                Price = 1,
                Quantity = 1,
                UnitOfMeasurement = "Gm",
                IsIngredient = true,
                IsEnabled = true,

            };
            int result = italianPizzaService.RegisterItem(item);

            Assert.AreEqual(result, 1);
        }


        [TestMethod]
        public void UpdateItemTestSuccessful()
        {
            ItemContract item = new ItemContract
            {
                IdItem = 9, 
                Name = "Harina updated",
                Description = "Prueba de harina updated",
                Sku = 2,
                Price = 2,
                Quantity = 2,
                UnitOfMeasurement = "Gm",
                IsIngredient = false,
                IsEnabled = true,

            };
            int result = italianPizzaService.UpdateItem(item);

            Assert.AreEqual(result, 1);
        }


        [TestMethod]
        public void DeleteItemTestSuccesful()
        {
            int idItem = 9;
            int result = italianPizzaService.DeleteItem(idItem);

            Assert.AreEqual(result, 1);
        }



        [TestMethod]
        public void RegisterStockTakingTestSuccessful()
        {
            List<StockTakingContract> stockTakingList = new List<StockTakingContract>();
            stockTakingList.Add(new StockTakingContract
            {
                CurrentAmount = 1,
                PhysicalAmount = 1,
                Description = "",
                Date = DateTime.Now,
                IdItem = 2,

            });
            int result = italianPizzaService.RegisterStockTaking(stockTakingList);

            Assert.AreEqual(result, 1);
        }



        [TestMethod]
        public void GetStockTakingTestSuccesful()
        {
            DateTime date = DateTime.Now;
            List<StockTakingContract> stockTakingList;
            stockTakingList = italianPizzaService.GetStockTaking(date);

            Assert.IsTrue(stockTakingList.Count > 0);
        }



        [TestMethod]
        public void GetItemsForStocktakingTestSuccessful()
        {

            List<StockTakingContract> stockTakingContractList;

            stockTakingContractList = italianPizzaService.GetItemsForStocktaking();

            Assert.IsTrue(stockTakingContractList.Count > 0);

        }



        [TestMethod]
        public void RegisterMonetaryExpeditureTestSuccessful()
        {
            MonetaryExpeditureContract monetaryExpediture = new MonetaryExpeditureContract
            {
                Amount = 1,
                Description = "Descrpción de la prueba",
                Date = DateTime.Now,
                IdEmployee = 1,

            };
            int result = italianPizzaService.RegisterMonetaryExpediture(monetaryExpediture);

            Assert.AreEqual(result, 1);
        }



        [TestMethod]
        public void GetMonetaryExpeditureTestSuccessful()
        {
            DateTime date = DateTime.Now;
            List<MonetaryExpeditureContract> monetaryExpensContractList;

            monetaryExpensContractList = italianPizzaService.GetMonetaryExpediture(date);

            Assert.IsTrue(monetaryExpensContractList.Count > 0);

        }



        [TestMethod]
        public void RegisterDailyBalanceTestSuccessful()
        {
            DailyBalanceContract dailyBalance = new DailyBalanceContract
            {
                EntryBalance = 1,
                ExitBalance = 1,
                InitialBalance = 1,
                CashBalance = 1,
                PhsycalBalance = 1,
                CurrentDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                IdEmployee = 1
            };
            int result = italianPizzaService.RegisterDailyBalance(dailyBalance);

            Assert.AreEqual(result, 1);
        }



        [TestMethod]
        public void GetDailyBalanceTestSuccessful()
        {
            DateTime date = DateTime.Now;
            List<DailyBalanceContract> dailyBalancesContractList;

            dailyBalancesContractList = italianPizzaService.GetDailyBalance(date);

            Assert.IsTrue(dailyBalancesContractList.Count > 0);

        }


        [TestMethod]
        public void CalculatateDialyEntrys()
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            decimal total = italianPizzaService.CalculatateDialyEntrys(date);

            Assert.IsNotNull(total);

        }



        [TestMethod]
        public void CalculateDialyExits()
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            decimal total = italianPizzaService.CalculateDialyExits(date);

            Assert.IsNotNull(total);

        }



        #endregion
    }
}
