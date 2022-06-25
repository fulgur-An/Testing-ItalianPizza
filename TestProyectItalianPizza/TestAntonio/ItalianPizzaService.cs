
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TestProyectItalianPizza.TestAntonio.Contracts;
using TestProyectItalianPizza.TestAntonio.Models;

namespace TestProyectItalianPizza.TestAntonio
{ 
    public class ItalianPizzaService
    {
        private ItalianPizzaContext context = new ItalianPizzaContext();

        #region Items service

        private List<Item> filterItems(string filter, int filterInt, int option)
        {
            List<Item> items = new List<Item>();
            switch (option)
            {
                case 1:
                    items = context.Items.Where(i => i.Name.Equals(filter)).ToList();
                    break;
                case 2:
                    items = context.Items.Where(i => i.Sku.Equals(filterInt)).ToList();
                    break;
                case 3:
                    items = context.Items.Where(i => i.Price.Equals(filterInt)).ToList();
                    break;
                case 4:
                    items = context.Items.Where(i => i.Quantity.Equals(filterInt)).ToList();
                    break;
            }
            return items;
        }

        public List<ItemContract> GetItemList(string filterString, int filterInt, int option)
        {

            //List<Item> items = context.Items.Where(i => i.IsEnabled.Equals(true)).ToList();
            //Item item = items.Where(i => i.IdItem.Equals(stockTaking.IdItem)).FirstOrDefault();
            //item.last

            List<Item> items = filterItems(filterString, filterInt, option);
            List<ItemContract> itemsContract = new List<ItemContract>();

            foreach (Item item in items)
            {
                if (item.IsEnabled)
                {
                    Stocktaking stockTaking = context.Stocktakings.Where(st => st.IdItem.Equals(item.IdItem)).FirstOrDefault();

                    itemsContract.Add(new ItemContract
                    {
                        IdItem = item.IdItem,
                        Name = item.Name,
                        Description = item.Description,
                        Sku = item.Sku,
                        Photo = item.Photo,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Restrictions = item.Restrictions,
                        UnitOfMeasurement = item.UnitOfMeasurement,
                        NeedsFoodRecipe = item.NeedsFoodRecipe,
                        IsIngredient = item.IsIngredient,
                        IsEnabled = item.IsEnabled,
                        //LastStockTaking = stockTaking.Date
                    });
                }
            }

            return itemsContract;
        }

        public ItemContract GetItemSpecification(int idItem)
        {
            Item item = context.Items.FirstOrDefault(i => i.IdItem.Equals(idItem));

            ItemContract itemContract = new ItemContract
            {
                IdItem = item.IdItem,
                Name = item.Name,
                Description = item.Description,
                Sku = item.Sku,
                Photo = item.Photo,
                Price = item.Price,
                Quantity = item.Quantity,
                Restrictions = item.Restrictions,
                UnitOfMeasurement = item.UnitOfMeasurement,
                NeedsFoodRecipe = item.NeedsFoodRecipe,
                IsIngredient = item.IsIngredient,
                IsEnabled = item.IsEnabled
            };

            return itemContract;
        }

        public int RegisterItem(ItemContract itemContract)
        {
            context.Items.Add(new Item
            {
                Name = itemContract.Name,
                Description = itemContract.Description,
                Sku = itemContract.Sku,
                Photo = itemContract.Photo,
                Price = itemContract.Price,
                Quantity = itemContract.Quantity,
                Restrictions = itemContract.Restrictions,
                UnitOfMeasurement = itemContract.UnitOfMeasurement,
                NeedsFoodRecipe = itemContract.NeedsFoodRecipe,
                IsIngredient = itemContract.IsIngredient,
                IsEnabled = itemContract.IsEnabled
            });
            int result = context.SaveChanges();
            return result;
        }

        public int UpdateItem(ItemContract itemContract)
        {
            Item item = context.Items.Where(i => i.IdItem.Equals(itemContract.IdItem)).Select(i => i).FirstOrDefault();

            item.Name = itemContract.Name;
            item.Description = itemContract.Description;
            item.Sku = itemContract.Sku;
            item.Photo = itemContract.Photo;
            item.Price = itemContract.Price;
            item.Quantity = itemContract.Quantity;
            item.Restrictions = itemContract.Restrictions;
            item.UnitOfMeasurement = itemContract.UnitOfMeasurement;
            item.NeedsFoodRecipe = itemContract.NeedsFoodRecipe;
            item.IsIngredient = itemContract.IsIngredient;
            item.IsEnabled = itemContract.IsEnabled;

            int result = context.SaveChanges();
            return result;
        }

        public int DeleteItem(int idItem)
        {
            Item item = context.Items.Where(i => i.IdItem.Equals(idItem)).FirstOrDefault();
            if (item != null)
            {
                item.IsEnabled = false;
                int result = context.SaveChanges();
                return result;
            }
            return 0;
        }

        #endregion


        #region stocktaking service

        public List<StockTakingContract> GetStockTaking(DateTime date)
        {
            List<Stocktaking> stockTakings = new List<Stocktaking>();
            try
            {
                stockTakings = context.Stocktakings.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Data);
            }

            Console.WriteLine(date);

            List<StockTakingContract> stockTakingsContract = new List<StockTakingContract>();

            foreach (Stocktaking stockTaking in stockTakings)
            {

                //Console.WriteLine(stockTaking);
                Item item = context.Items.Where(i => i.IsEnabled.Equals(true) && i.IdItem.Equals(stockTaking.IdItem)).SingleOrDefault();
                stockTakingsContract.Add(new StockTakingContract
                {
                    Name = item.Name,
                    Sku = item.Sku,
                    CurrentAmount = stockTaking.CurrentAmount,
                    PhysicalAmount = stockTaking.PhysicalAmount,
                    Description = stockTaking.Description,
                    Date = stockTaking.Date,
                    IdItem = stockTaking.IdItem,
                });
            }
            return stockTakingsContract;
        }

        public int RegisterStockTaking(List<StockTakingContract> stockTakings)
        {

            foreach (StockTakingContract stockTaking in stockTakings)
            {
                context.Stocktakings.Add(new Stocktaking
                {
                    CurrentAmount = stockTaking.CurrentAmount,
                    PhysicalAmount = stockTaking.PhysicalAmount,
                    Description = stockTaking.Description,
                    Date = stockTaking.Date,
                    IdItem = stockTaking.IdItem,
                });

            }


            int result = context.SaveChanges();
            return result;
        }


        public List<StockTakingContract> GetItemsForStocktaking()
        {
            List<Item> items = context.Items.Where(i => i.IsEnabled.Equals(true)).ToList();
            List<StockTakingContract> itemsContract = new List<StockTakingContract>();

            foreach (Item item in items)
            {
                if (item.IsEnabled)
                {
                    itemsContract.Add(new StockTakingContract
                    {
                        Name = item.Name,
                        Sku = item.Sku,
                        Date = DateTime.Now,
                        CurrentAmount = item.Quantity,
                        IdItem = item.IdItem,
                    });
                }
            }
            return itemsContract;
        }

        #endregion

        #region monetary expediture
        public List<MonetaryExpeditureContract> GetMonetaryExpediture(DateTime date)
        {
            List<MonetaryExpens> monetaryExpens = context.MonetaryExpenses.ToList();
            List<MonetaryExpeditureContract> monetaryExpensContract = new List<MonetaryExpeditureContract>();

            foreach (MonetaryExpens monetaryExpen in monetaryExpens)
            {
                monetaryExpensContract.Add(new MonetaryExpeditureContract
                {
                    IdMonetaryExpenditure = monetaryExpen.IdMonetaryExpenditure,
                    Amount = monetaryExpen.Amount,
                    Description = monetaryExpen.Description,
                    Date = Convert.ToDateTime(monetaryExpen.Date.ToString("d")),
                    IdEmployee = monetaryExpen.IdEmployee,
                });
            }
            return monetaryExpensContract;
        }

        public int RegisterMonetaryExpediture(MonetaryExpeditureContract monetaryExpediture)
        {
            context.MonetaryExpenses.Add(new MonetaryExpens
            {
                //date1.ToString("d",
                //  CultureInfo.CreateSpecificCulture("en-NZ")
                Amount = monetaryExpediture.Amount,
                Description = monetaryExpediture.Description,
                Date = Convert.ToDateTime(monetaryExpediture.Date.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"))),
                IdEmployee = monetaryExpediture.IdEmployee
            });

            int result = context.SaveChanges();
            return result;
        }

        #endregion

        #region Daily balance

        public List<DailyBalanceContract> GetDailyBalance(DateTime date)
        {
            List<DailyBalance> dailyBalances = context.DailyBalances.ToList();
            List<DailyBalanceContract> dailyBalancesContract = new List<DailyBalanceContract>();

            foreach (DailyBalance dailyBalance in dailyBalances)
            {
                dailyBalancesContract.Add(new DailyBalanceContract
                {
                    EntryBalance = dailyBalance.EntryBalance,
                    ExitBalance = dailyBalance.ExitBalance,
                    InitialBalance = dailyBalance.InitialBalance,
                    CashBalance = dailyBalance.CashBalance,
                    PhsycalBalance = dailyBalance.PhsycalBalance,
                    CurrentDate = Convert.ToDateTime(dailyBalance.CurrentDate.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"))),
                    IdEmployee = dailyBalance.IdEmployee
                });
            }
            return dailyBalancesContract;
        }

        public int RegisterDailyBalance(DailyBalanceContract dailyBalance)
        {
            context.DailyBalances.Add(new DailyBalance
            {
                EntryBalance = dailyBalance.EntryBalance,
                ExitBalance = dailyBalance.ExitBalance,
                InitialBalance = dailyBalance.InitialBalance,
                CashBalance = dailyBalance.CashBalance,
                PhsycalBalance = dailyBalance.PhsycalBalance,
                CurrentDate = Convert.ToDateTime(dailyBalance.CurrentDate.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ"))),
                IdEmployee = dailyBalance.IdEmployee
            });

            int result = context.SaveChanges();
            return result;
        }

        public decimal CalculatateDialyEntrys(DateTime date)
        {
            List<Order> orders = context.Orders.Where(o => o.Date.Equals(date) && o.Status.Equals("Delivered")).ToList();
            decimal total = 0;
            foreach (Order order in orders)
            {
                total = total + order.TotalToPay;
            }

            return total;
        }


        public decimal CalculateDialyExits(DateTime date)
        {
            List<ProviderOrder> providerOrders = context.ProviderOrders.Where(po => po.DeliveryDate.Equals(date) && po.Status.Equals("Received")).ToList();
            List<MonetaryExpens> monetaryExpenses = context.MonetaryExpenses.Where(me => me.Date.Equals(date)).ToList();
            decimal total = 0;
            foreach (ProviderOrder providerOrder in providerOrders)
            {
                total = total + providerOrder.TotalToPay;
            }
            foreach (MonetaryExpens monetaryExpense in monetaryExpenses)
            {
                total = total + monetaryExpense.Amount;
            }
            return total;
        }

        public List<decimal> GetAmounts()
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.ToString("d", CultureInfo.CreateSpecificCulture("en-NZ")));

            decimal dialyEntrys = CalculatateDialyEntrys(date);

            decimal dialyExits = CalculateDialyExits(date);

            decimal cashBalance = 1000 + dialyEntrys - dialyExits;

            List<decimal> resultList = new List<decimal>();
            resultList.Add(dialyEntrys);
            resultList.Add(dialyExits);
            resultList.Add(cashBalance);
            return resultList;

        }


        #endregion
    }
}