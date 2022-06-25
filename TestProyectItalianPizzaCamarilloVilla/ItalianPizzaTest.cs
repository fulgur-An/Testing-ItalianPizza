using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProyectItalianPizza.TestCamarilloVilla;
using TestProyectItalianPizza.TestCamarilloVilla.Contracts;
using TestProyectItalianPizza.TestCamarilloVilla.Models;

namespace TestProyectItalianPizza
{
    [TestClass]
    public class ItalianPizzaTest
    {
        ItalianPizzaService italianPizzaService = new ItalianPizzaService();

        [TestMethod]
        public void RegisterFoodRecipeTestSuccessful()
        {
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract
            {
                Price = 199,
                Name = "Pizza grande de Pepperoni",
                Description = "1.\n Preperar Masa. 2.\n Hornear pizza a 180°. 3.\n Servir la pizza. ",
                NumberOfServings = 8,
                Status = "Availaible",
                IsEnabled = true,
            };

            List<IngredientContract> ingredientes = new List<IngredientContract>();
            ingredientes.Add(new IngredientContract
            {
                IdFoodRecipe = 1,
                IdItem = 2,
                IngredientName = "Tomate",
                IngredientQuantity = "10"
            });

            int result = italianPizzaService.RegisterFoodRecipe(foodRecipeContract, ingredientes);
            Assert.AreEqual(1, result);           
        }

        [TestMethod]
        public void RegisterFoodRecipeTestUnsuccessful()
        {
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract
            {
                Price = 199,
                Name = "Pizza grande de Pepperoni",
                Description = "1.\n Preperar Masa. 2.\n Hornear pizza a 180°. 3.\n Servir la pizza. ",
                NumberOfServings = 8,
                Status = "Availaible",
                IsEnabled = true,
            };

            List<IngredientContract> ingredientes = new List<IngredientContract>();
            ingredientes.Add(new IngredientContract
            {
                IdFoodRecipe = 1,
                IdItem = 2,
                IngredientName = "Tomate",
                IngredientQuantity = "10"
            });

            int result = italianPizzaService.RegisterFoodRecipe(foodRecipeContract, ingredientes);
            Assert.AreEqual(-1, result);
        }

        [TestMethod]
        public void GetFoodRecipeByIdTestSuccessful()
        {
            var foodRecipeList = italianPizzaService.GetFoodRecipeById(1);
            Assert.AreEqual(foodRecipeList.Name, "Pizza grande de Pepperoni");
        }

        [TestMethod]
        public void GetFoodRecipeByIdTestUnsuccessful()
        {
            var foodRecipeList = italianPizzaService.GetFoodRecipeById(1);
            Assert.AreNotEqual(foodRecipeList.Name, "Fake Food Recipe Name");
        }

        [TestMethod]
        public void GetIngredientsByIdTestSuccessful()
        {
            var ingredients = italianPizzaService.GetIngredientsById(1);
            Assert.IsTrue(ingredients.Count > 0);
        }

        [TestMethod]
        public void GetIngredientsByIdTesUnsuccessful()
        {
            int invalidID = -1;
            var ingredientes = italianPizzaService.GetIngredientsById(invalidID);

            Assert.IsTrue(ingredientes.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetFoodRecipeListSuccessful()
        {
            var foodRecipeList = italianPizzaService.GetFoodRecipeList();

            Assert.IsTrue(foodRecipeList.Count > 0);
            Assert.Fail("Exception Italian Pizza  Db");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetIngredientsSuccessful()
        {
            var ingredientsList = italianPizzaService.GetItemIngredientList();

            Assert.IsTrue(ingredientsList.Count > 0);
            Assert.Fail("Exception Italian Pizza  Db");
        }

        [TestMethod]
        public void DeleteFoodRecipeByIdTestSuccessful()
        {
            int foodRecipeID = 1;
            int result = italianPizzaService.DeleteFoodRecipeById(foodRecipeID);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DeleteFoodRecipeByIdTestUnsuccessful()
        {
            int foodRecipeID = 1;
            int result = italianPizzaService.DeleteFoodRecipeById(foodRecipeID);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void UpdateFoodRecipeTestUnsuccessful()
        {
            FoodRecipeContract foodRecipeContract = new FoodRecipeContract
            {
                IdFoodRecipe = 1,
                Price = 100,
                Name = "Pizza grande de Cheetos",
                Description = "1.\n Preperar Masa. 2.\n Hornear pizza a 180°. 3.\n Servir la pizza. ",
                NumberOfServings = 8,
                Status = "Availaible",
                IsEnabled = true,
            };

            List<IngredientContract> ingredientes = new List<IngredientContract>();
            ingredientes.Add(new IngredientContract
            {
                IdFoodRecipe = 1,
                IdItem = 5,
                IngredientName = "Pepperoni",
                IngredientQuantity = "1"
            });

            int result = italianPizzaService.UpdateFoodRecipe(foodRecipeContract, ingredientes);
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void RegisterOrderTestSuccessful()
        {
            List<QuantityFoodRecipeContract> quantityFoodRecipeContracts = new List<QuantityFoodRecipeContract>();
            List<QuantityItemContract> quantityItemContracts = new List<QuantityItemContract>();

            OrderContract order = new OrderContract
            {
                Date = DateTime.Now,
                Status = "",
                TotalToPay = 1000,
                TableNumber = 8,
                TypeOrder = "Local",
                IdEmployee = 1
            };

            int result = italianPizzaService.RegisterOrder(order, 
                quantityFoodRecipeContracts,
                quantityItemContracts
            );

            Assert.AreEqual(1, result);
        }
    }
}
