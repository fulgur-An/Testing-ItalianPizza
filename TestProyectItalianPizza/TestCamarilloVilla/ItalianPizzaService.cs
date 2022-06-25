using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProyectItalianPizza.TestCamarilloVilla.Contracts;
using TestProyectItalianPizza.TestCamarilloVilla.Models;

namespace TestProyectItalianPizza.TestCamarilloVilla
{
    public class ItalianPizzaService
    {
        private ItalianPizzaContext context = new ItalianPizzaContext();

        public FoodRecipeContract GetFoodRecipeById(int idFoodRecipe)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.FirstOrDefault(food => food.IdFoodRecipe.Equals(idFoodRecipe));
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract();

            foodRecipeContract.IdFoodRecipe = foodRecipe.IdFoodRecipe;
            foodRecipeContract.Price = foodRecipe.Price;
            foodRecipeContract.Name = foodRecipe.Name;
            foodRecipeContract.Description = foodRecipe.Description;
            foodRecipeContract.NumberOfServings = foodRecipe.NumberOfServings;
            foodRecipeContract.Status = foodRecipe.Status;
            foodRecipeContract.IsEnabled = foodRecipe.IsEnabled;

            return foodRecipeContract;
        }

        public List<FoodRecipeContract> GetFoodRecipeList()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.Where(foodRecipe => foodRecipe.IsEnabled.Equals("Available")).ToList();
            List<FoodRecipeContract> foodRecipeContracts = new List<FoodRecipeContract>();

            List<Ingredient> ingredients = context.Ingredients.ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foreach (FoodRecipe foodRecipe in foodRecipes)
            {
                foodRecipeContracts.Add(new FoodRecipeContract
                {
                    IdFoodRecipe = foodRecipe.IdFoodRecipe,
                    Price = foodRecipe.Price,
                    Name = foodRecipe.Name,
                    Description = foodRecipe.Description,
                    NumberOfServings = foodRecipe.NumberOfServings,
                    Status = foodRecipe.Status,
                    IsEnabled = foodRecipe.IsEnabled
                });
            }

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }


            return foodRecipeContracts;
            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetFoodRecipeList(
            //    foodRecipeContracts, ingredientContracts
            //);
        }

        public List<IngredientContract> GetIngredientsById(int idFoodRecipe)
        {
            List<Item> items = context.Items.ToList();
            List<Ingredient> ingredients = context.Ingredients.Where(ingredient => ingredient.IdFoodRecipe == idFoodRecipe).ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foreach (Ingredient ingredient in ingredients)
            {
                byte[] ingredientPhoto = null;
                foreach (Item item in items)
                {
                    if (ingredient.IdItem == item.IdItem)
                    {
                        ingredientPhoto = item.Photo;
                    }
                }

                ingredientContracts.Add(new IngredientContract
                {
                    IdIngredient = ingredient.IdIngredient,
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName,
                    IngredientPhoto = ingredientPhoto
                });
            }

            return ingredientContracts;
        }

        public List<ItemContract> GetItemIngredientList()
        {
            List<Item> items = context.Items.Where(item => item.IsIngredient).ToList();

            List<ItemContract> itemContracts = new List<ItemContract>();

            foreach (Item item in items)
            {
                itemContracts.Add(new ItemContract
                {
                    IdItem = item.IdItem,
                    Photo = item.Photo,
                    Name = item.Name
                });
            }

            return itemContracts;
        }

        public int RegisterFoodRecipe(FoodRecipeContract foodRecipeContract, List<IngredientContract> ingredients)
        {
            int result = 0;

            bool isExisting = context.FoodRecipes.Where(recipe => recipe.Name.Equals(foodRecipeContract.Name) &&
                recipe.IsEnabled == true).Count() > 0;

            FoodRecipe foodRecipe = new FoodRecipe
            {
                Price = foodRecipeContract.Price,
                Name = foodRecipeContract.Name,
                Description = foodRecipeContract.Description,
                NumberOfServings = foodRecipeContract.NumberOfServings,
                Status = foodRecipeContract.Status,
                IsEnabled = foodRecipeContract.IsEnabled
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.FoodRecipes.Add(foodRecipe);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int foodRecipeId = context.FoodRecipes.Max(recipe => recipe.IdFoodRecipe);
                        result = RegisterFoodRecipeIngredients(ingredients, foodRecipeId);
                    }
                    else
                    {
                        context.FoodRecipes.Remove(foodRecipe);
                        context.SaveChanges();
                        result = 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }

            return result;
        }

        public int RegisterFoodRecipeIngredients(List<IngredientContract> ingredientsContract, int foodRecipeId)
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            foreach (IngredientContract ingredient in ingredientsContract)
            {
                context.Ingredients.Add(new Ingredient
                {
                    IdFoodRecipe = foodRecipeId,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }
            int result = context.SaveChanges();

            return result;
        }

        public bool IdentifyChangesInIngredients(List<IngredientContract> ingredientContracts, int idFoodRecipe)
        {
            List<Ingredient> ingredients = context.Ingredients.ToList();
            int foodRecipeIngredientsNumber = context.Ingredients.Where(x => x.IdFoodRecipe == idFoodRecipe).Count();
            int newFoodRecipeIngredients = ingredientContracts.Count;
            bool newIngredient = foodRecipeIngredientsNumber == ingredientContracts.Count ? false : true;

            foreach (IngredientContract ingredientContract in ingredientContracts)
            {
                if (ingredientContract.IdIngredient == 0)
                {
                    newIngredient = true;
                }
            }

            return newIngredient;
        }

        public List<FoodRecipeContract> GetFoodRecipeListSortedByName()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();

            List<FoodRecipeContract> foodRecipeContracts = new List<FoodRecipeContract>();

            List<Ingredient> ingredients = context.Ingredients.ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            foreach (FoodRecipe foodRecipe in foodRecipes)
            {
                foodRecipeContracts.Add(new FoodRecipeContract
                {
                    IdFoodRecipe = foodRecipe.IdFoodRecipe,
                    Price = foodRecipe.Price,
                    Name = foodRecipe.Name,
                    Description = foodRecipe.Description,
                    NumberOfServings = foodRecipe.NumberOfServings,
                    Status = foodRecipe.Status,
                    IsEnabled = foodRecipe.IsEnabled
                });
            }

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            return foodRecipeContracts;
        }

        public List<FoodRecipeContract> GetRecipesAvailable()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();

            List<FoodRecipeContract> foodRecipesAvailable = new List<FoodRecipeContract>();
            List<Item> items = context.Items.ToList();

            foreach (FoodRecipe foodRecipe in foodRecipes)
            {
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                int foodRecipeIngredients = ingredients.Count;
                int count = 0;

                foreach (Ingredient ingredient in ingredients)
                {
                    foreach (Item item in items)
                    {
                        int ingredientQuantity = int.Parse(ingredient.IngredientQuantity);
                        if ((item.IdItem == ingredient.IdItem) && (item.Quantity >= ingredientQuantity))
                        {
                            count++;
                        }
                    }
                }

                if (count == foodRecipeIngredients)
                {
                    foodRecipesAvailable.Add(new FoodRecipeContract
                    {
                        IdFoodRecipe = foodRecipe.IdFoodRecipe,
                        Price = foodRecipe.Price,
                        Name = foodRecipe.Name,
                        Description = foodRecipe.Description,
                        NumberOfServings = foodRecipe.NumberOfServings,
                        Status = foodRecipe.Status,
                        IsEnabled = foodRecipe.IsEnabled
                    });
                }
            }

            return foodRecipesAvailable;
        }

        public int DeleteFoodRecipeById(int idFoodRecipe)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.Where(recipe => recipe.IdFoodRecipe == idFoodRecipe).FirstOrDefault();
            int result = 0;

            if (foodRecipe != null)
            {
                foodRecipe.IsEnabled = false;
                foodRecipe.Status = "Deleted";
                result = context.SaveChanges();

            }

            return result;
        }

        public int UpdateFoodRecipeIngredients(List<IngredientContract> ingredientContracts, int idFoodRecipe)
        {
            int result = 0;

            List<Ingredient> ingredients = context.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredients)
            {
                if (ingredient.IdFoodRecipe == idFoodRecipe)
                {
                    context.Ingredients.Remove(ingredient);
                    result = context.SaveChanges();
                }
            }

            return result;
        }

        public int UpdateFoodRecipe(FoodRecipeContract foodRecipeContract, List<IngredientContract> ingredients)
        {
            FoodRecipe foodRecipe = context.FoodRecipes.Where(recipe => recipe.IdFoodRecipe == foodRecipeContract.IdFoodRecipe).FirstOrDefault();

            int result = 0;
            bool isExisting = context.FoodRecipes.Where(recipe => recipe.Name.Equals(foodRecipeContract.Name) &&
                recipe.IsEnabled && recipe.IdFoodRecipe != foodRecipe.IdFoodRecipe).Count() > 0;
            bool newIngredients = IdentifyChangesInIngredients(ingredients, foodRecipe.IdFoodRecipe);

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (foodRecipe != null)
                {
                    foodRecipe.Price = foodRecipeContract.Price;
                    foodRecipe.Name = foodRecipeContract.Name;
                    foodRecipe.Description = foodRecipeContract.Description;
                    foodRecipe.NumberOfServings = foodRecipeContract.NumberOfServings;
                    foodRecipe.IsEnabled = foodRecipeContract.IsEnabled;
                    foodRecipe.Status = foodRecipeContract.Status;

                    result = context.SaveChanges();
                    context.SaveChanges();
                }
                if ((result > 0) || newIngredients)
                {
                    result = UpdateFoodRecipeIngredients(ingredients, foodRecipeContract.IdFoodRecipe);
                    if (result > 0)
                    {
                        result = RegisterFoodRecipeIngredients(ingredients, foodRecipeContract.IdFoodRecipe);
                    }
                }
            }

            return result;
        }

        public List<ItemContract> GetItemsSortedByName()
        {
            List<Item> items = context.Items.Where(item => !item.IsIngredient && item.IsEnabled).ToList();
            List<ItemContract> itemContracts = new List<ItemContract>();

            foreach (Item item in items)
            {
                itemContracts.Add(new ItemContract
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
                });
            }

            return itemContracts;
        }

        public List<string> DecreaseNumberOfIngredients(List<QuantityFoodRecipeContract> quantityFoodRecipeContracts)
        {
            List<Item> items = context.Items.ToList();
            List<string> errors = new List<string>();

            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipeContract.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    if (ingredient.IdFoodRecipe == quantityFoodRecipeContract.IdFoodRecipe)
                    {
                        Item item = context.Items.Find(ingredient.IdItem);
                        int auxiliary = item.Quantity;
                        auxiliary = auxiliary - quantityFoodRecipeContract.QuantityOfFoodRecipes;
                        if (!(auxiliary >= 0))
                        {
                            errors.Add("Disminuye la cantidad de " + foodRecipe.Name + ". No hay suficientes ingredientes en inventario");
                        }
                    }
                }
            }

            return errors;
        }

        public void ApplyDecreaseNumberOfIngredients(List<QuantityFoodRecipeContract> quantityFoodRecipeContracts)
        {
            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipeContract.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();
                foreach (Ingredient ingredient in ingredients)
                {
                    if (ingredient.IdFoodRecipe == quantityFoodRecipeContract.IdFoodRecipe)
                    {
                        Item item = context.Items.Find(ingredient.IdItem);
                        item.Quantity = item.Quantity - quantityFoodRecipeContract.QuantityOfFoodRecipes;
                        context.SaveChanges();
                    }
                }
            }
        }

        public List<string> DecreaseNumberOfItems(List<QuantityItemContract> quantityItemContracts)
        {
            List<string> errors = new List<string>();

            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                Item item = context.Items.Find(quantityItemContract.IdItem);
                int auxiliary = item.Quantity;
                auxiliary = auxiliary - quantityItemContract.QuantityOfItems;
                if (!(auxiliary >= 0))
                {
                    errors.Add("Disminuye la cantidad de " + item.Name + ". No hay suficientes ingredientes en inventario");
                }
            }

            return errors;
        }

        public void ApplyDecreaseNumberOfItems(List<QuantityItemContract> quantityItemContracts)
        {
            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                Item item = context.Items.Find(quantityItemContract.IdItem);
                item.Quantity = item.Quantity - quantityItemContract.QuantityOfItems;
                context.SaveChanges();
            }
        }

        public int RegisterOrder(OrderContract orderContract, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts)
        {
            int result = 0;

            Order order = new Order
            {
                Date = orderContract.Date,
                Status = orderContract.Status,
                TotalToPay = orderContract.TotalToPay,
                TableNumber = orderContract.TableNumber,
                TypeOrder = orderContract.TypeOrder,
                IdEmployee = orderContract.IdEmployee,
            };

            if (order.TypeOrder.Equals("Domicilio"))
            {
                order.IdCustomer = orderContract.IdCustomer;
            }

            if (orderContract.Address != null)
            {
                order.IdAddress = orderContract.Address.IdAddresses;
            }

            context.Orders.Add(order);
            result = context.SaveChanges();

            List<string> foodRecipeErrors = DecreaseNumberOfIngredients(quantityFoodRecipeContracts);
            List<string> itemErrors = DecreaseNumberOfItems(quantityItemContracts);

            if ((result > 0) && (foodRecipeErrors.Count == 0) && (itemErrors.Count == 0))
            {
                int orderId = context.Orders.Max(x => x.IdOrder);

                foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
                {
                    context.QuantityFoodRecipes.Add(new QuantityFoodRecipe
                    {
                        IdOrder = orderId,
                        IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                        QuantityOfFoodRecipes = quantityFoodRecipeContract.QuantityOfFoodRecipes,
                        Price = quantityFoodRecipeContract.Price,
                        IsDone = false
                    });
                }

                result = context.SaveChanges();

                foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
                {
                    context.QuantityItems.Add(new QuantityItem
                    {
                        IdOrder = orderId,
                        IdItem = quantityItemContract.IdItem,
                        QuantityOfItems = quantityItemContract.QuantityOfItems,
                        Price = quantityItemContract.Price,
                        IsDone = false
                    });
                }

                result = context.SaveChanges();

                ApplyDecreaseNumberOfIngredients(quantityFoodRecipeContracts);
                ApplyDecreaseNumberOfItems(quantityItemContracts);
            }
            else
            {
                context.Orders.Remove(order);
                context.SaveChanges();
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().RegisterOrder(
            //   result, foodRecipeErrors, itemErrors
            //);

            return result;
        }

        public int GetIdEmployeeByName(string fullNameEmployee)
        {
            int idEmployee = context.Employees.FirstOrDefault(x => (x.Name + " " + x.LastName).Equals(fullNameEmployee)).IdUserEmployee;

            return idEmployee;
        }

        public List<OrderContract> GetOrderList()
        {
            List<Order> orders = context.Orders.OrderBy(x => x.Date).ToList();
            List<OrderContract> orderContracts = new List<OrderContract>();

            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();
            List<QuantityFoodRecipeContract> quantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();

            List<QuantityItem> quantityItems = context.QuantityItems.ToList();
            List<QuantityItemContract> quantityItemContracts = new List<QuantityItemContract>();

            foreach (Order order in orders)
            {
                var customer = context.Customers.Find(order.IdCustomer);
                string customerFullName = "";

                if (customer != null)
                {
                    customerFullName = customer.Name + " " + customer.LastName;
                }
                else
                {
                    customerFullName = "Mesa número " + order.TableNumber;
                }

                Address address = context.Addresses.Find(order.IdAddress);

                OrderContract orderContract = new OrderContract
                {
                    IdOrder = order.IdOrder,
                    Date = order.Date,
                    Status = order.Status,
                    TotalToPay = order.TotalToPay,
                    TableNumber = order.TableNumber,
                    TypeOrder = order.TypeOrder,
                    IdEmployee = order.IdEmployee,
                    CustomerFullName = customerFullName
                };

                if (address != null)
                {
                    orderContract.Address = new AddressContract
                    {
                        Colony = order.Address.Colony,
                        City = order.Address.City,
                        InsideNumber = address.InsideNumber,
                        OutsideNumber = address.OutsideNumber,
                        PostalCode = address.PostalCode,
                        IdCustomer = address.IdCustomer,
                        StreetName = address.StreetName,
                        IdAddresses = address.IdAddresses
                    };
                }

                orderContracts.Add(orderContract);
            }

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                string name = (context.FoodRecipes.Find(quantityFoodRecipe.IdFoodRecipe)).Name;
                quantityFoodRecipeContracts.Add(new QuantityFoodRecipeContract
                {
                    IdOrder = quantityFoodRecipe.IdOrder,
                    IdFoodRecipe = quantityFoodRecipe.IdFoodRecipe,
                    QuantityOfFoodRecipes = quantityFoodRecipe.QuantityOfFoodRecipes,
                    Price = quantityFoodRecipe.Price,
                    IdQuantityFoodRecipe = quantityFoodRecipe.IdQuantityFoodRecipe,
                    Name = name
                });
            }

            foreach (QuantityItem quantityItem in quantityItems)
            {
                string name = (context.Items.Find(quantityItem.IdItem)).Name;
                quantityItemContracts.Add(new QuantityItemContract
                {
                    IdItem = quantityItem.IdItem,
                    IdOrder = quantityItem.IdOrder,
                    QuantityOfItems = quantityItem.QuantityOfItems,
                    Price = quantityItem.Price,
                    IdQuantityItem = quantityItem.IdQuantityItem,
                    Name = name
                });
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetOrderList(
            //    orderContracts, quantityFoodRecipeContracts, quantityItemContracts
            //);

            return orderContracts;
        }

        public int DeleteOrderById(int orderId)
        {
            int result = 0;
            Order order = context.Orders.Find(orderId);
            order.Status = "Cancelada";

            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.Where(x => x.IdOrder == order.IdOrder).ToList();

            List<QuantityItem> quantityItems = context.QuantityItems.Where(x => x.IdOrder == order.IdOrder).ToList();
            List<Item> items = context.Items.ToList();

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                FoodRecipe foodRecipe = context.FoodRecipes.Find(quantityFoodRecipe.IdFoodRecipe);
                List<Ingredient> ingredients = context.Ingredients.Where(x => x.IdFoodRecipe == foodRecipe.IdFoodRecipe).ToList();

                foreach (Ingredient ingredient in ingredients)
                {
                    Item item = items.Single(x => x.IdItem == ingredient.IdItem);
                    item.Quantity += int.Parse(ingredient.IngredientQuantity);
                }
            }

            foreach (QuantityItem quantityItem in quantityItems)
            {
                Item item = context.Items.Find(quantityItem.IdItem);
                item.Quantity += quantityItem.QuantityOfItems;
            }

            result = context.SaveChanges();

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().DeleteOrderById(
            //    result
            //);

            return result;
        }

        public List<FoodRecipeContract> GetFoodRecipeListToPrepare()
        {
            List<FoodRecipe> foodRecipes = context.FoodRecipes.
                Where(foodRecipe => foodRecipe.IsEnabled && foodRecipe.Status.Equals("Available")).
                OrderBy(foodRecipe => foodRecipe.Name).ToList();
            List<FoodRecipeContract> foodRecipeContracts = new List<FoodRecipeContract>();
            List<Order> ordersToPrepare = context.Orders.Where(x => x.Status.Equals("En proceso")).ToList();
            List<QuantityFoodRecipe> foodRecipesToPrepare = context.QuantityFoodRecipes.Where(x => !x.IsDone).ToList();
            List<int> idFoodRecipesToPrepare = new List<int>();
            List<string> customerFullNames = new List<string>();
            List<int> quantity = new List<int>();

            foreach (Order order in ordersToPrepare)
            {
                foreach (QuantityFoodRecipe foodRecipe in foodRecipesToPrepare)
                {
                    if (foodRecipe.IdOrder == order.IdOrder)
                    {
                        Customer customer = context.Customers.Find(order.IdCustomer);
                        string fullName = "";

                        if (customer != null)
                        {
                            fullName = order.IdOrder + " / " + customer.Name + " " + customer.LastName;
                        }
                        else
                        {
                            fullName = order.IdOrder + " / " + "Número de Mesa" + " " + order.TableNumber;
                        }

                        idFoodRecipesToPrepare.Add(foodRecipe.IdFoodRecipe);
                        customerFullNames.Add(fullName);
                        quantity.Add(foodRecipe.QuantityOfFoodRecipes);
                    }
                }
            }

            List<Ingredient> ingredients = context.Ingredients.ToList();
            List<IngredientContract> ingredientContracts = new List<IngredientContract>();

            int count = 0;

            foreach (int idFoodRecipe in idFoodRecipesToPrepare)
            {
                foreach (FoodRecipe foodRecipe in foodRecipes)
                {
                    if (idFoodRecipe == foodRecipe.IdFoodRecipe)
                    {
                        foodRecipeContracts.Add(new FoodRecipeContract
                        {
                            IdFoodRecipe = foodRecipe.IdFoodRecipe,
                            Price = foodRecipe.Price,
                            Name = quantity[count] + " / " + foodRecipe.Name,
                            Description = foodRecipe.Description,
                            NumberOfServings = foodRecipe.NumberOfServings,
                            Status = foodRecipe.Status,
                            IsEnabled = foodRecipe.IsEnabled,
                            CustomerName = customerFullNames[count]
                        });
                    }
                }
                count++;
            }

            foreach (Ingredient ingredient in ingredients)
            {
                ingredientContracts.Add(new IngredientContract
                {
                    IdFoodRecipe = ingredient.IdFoodRecipe,
                    IdItem = ingredient.IdItem,
                    IngredientQuantity = ingredient.IngredientQuantity,
                    IngredientName = ingredient.IngredientName
                });
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetFoodRecipeListToPrepare(
            //    foodRecipeContracts, ingredientContracts
            //);

            return foodRecipeContracts;
        }

        public int UpdateOrder(int orderId, List<QuantityFoodRecipeContract> quantityFoodRecipeContracts,
            List<QuantityItemContract> quantityItemContracts)
        {
            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();
            List<QuantityItem> quantityItems = context.QuantityItems.ToList();

            int orderQuantityFoodRecipes = context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId).Count();
            int orderQuantityItems = context.QuantityItems.Where(x => x.IdOrder == orderId).Count();

            int newQuantityFoodRecipes = quantityFoodRecipeContracts.Count;
            int newQuantityItems = quantityItemContracts.Count;

            bool newFoodRecipesQuantities = orderQuantityFoodRecipes != newQuantityFoodRecipes;
            bool newItemsQuantities = orderQuantityItems != newQuantityItems;

            foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
            {
                if (quantityFoodRecipeContract.IdOrder == 0)
                {
                    newFoodRecipesQuantities = true;
                }
            }

            foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
            {
                if (quantityItemContract.IdOrder == 0)
                {
                    newItemsQuantities = true;
                }
            }

            int result;

            if (newFoodRecipesQuantities || newItemsQuantities)
            {
                foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
                {
                    if (quantityFoodRecipe.IdOrder == orderId)
                    {
                        context.QuantityFoodRecipes.Remove(quantityFoodRecipe);
                    }
                }

                foreach (QuantityItem quantityItem in quantityItems)
                {
                    if (quantityItem.IdOrder == orderId)
                    {
                        context.QuantityItems.Remove(quantityItem);
                    }
                }

                result = context.SaveChanges();

                if (result > 0)
                {
                    Order order = context.Orders.Find(orderId);
                    decimal price = 0;

                    foreach (QuantityFoodRecipeContract quantityFoodRecipeContract in quantityFoodRecipeContracts)
                    {
                        context.QuantityFoodRecipes.Add(new QuantityFoodRecipe
                        {
                            IdOrder = orderId,
                            IdFoodRecipe = quantityFoodRecipeContract.IdFoodRecipe,
                            QuantityOfFoodRecipes = quantityFoodRecipeContract.QuantityOfFoodRecipes,
                            Price = quantityFoodRecipeContract.Price,
                            IsDone = false
                        });

                        price += quantityFoodRecipeContract.Price * quantityFoodRecipeContract.QuantityOfFoodRecipes;
                    }

                    foreach (QuantityItemContract quantityItemContract in quantityItemContracts)
                    {
                        context.QuantityItems.Add(new QuantityItem
                        {
                            IdOrder = orderId,
                            IdItem = quantityItemContract.IdItem,
                            QuantityOfItems = quantityItemContract.QuantityOfItems,
                            Price = quantityItemContract.Price,
                            IsDone = false
                        });

                        price += quantityItemContract.Price * quantityItemContract.QuantityOfItems;
                    }

                    order.TotalToPay = price;

                    result = context.SaveChanges();
                }
            }
            else
            {
                result = -1;
            }


            return result;
        }

        public int MarkRecipeAsDone(int orderId, int foodRecipeId)
        {
            Order order = context.Orders.Find(orderId);
            List<QuantityFoodRecipe> quantityFoodRecipes = context.QuantityFoodRecipes.ToList();

            foreach (QuantityFoodRecipe quantityFoodRecipe in quantityFoodRecipes)
            {
                if ((quantityFoodRecipe.IdFoodRecipe == foodRecipeId) && (order.IdOrder == orderId))
                {
                    quantityFoodRecipe.IsDone = true;
                }
            }

            int result = context.SaveChanges();
            int foodRecipesMade = context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId && x.IsDone).ToList().Count;
            bool foodRecipeMade = foodRecipesMade == context.QuantityFoodRecipes.Where(x => x.IdOrder == orderId).ToList().Count;
            string information = "";

            if (foodRecipeMade)
            {
                order.Status = "Realizado";
                context.SaveChanges();

                information = order.TableNumber != null
                    ? "Pedido ID " + order.IdOrder + " de la Mesa " + order.TableNumber
                    : "Pedido ID " + order.IdOrder + " del Cliente" + context.Customers.Find(order.IdCustomer).Name + " " +
                        context.Customers.Find(order.IdCustomer).LastName;
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().MarkRecipeAsDone(
            //    result, foodRecipeMade, information
            //);

            return result;
        }
    }
}