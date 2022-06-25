using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using UnitTest_ItalianPizza.TestJavierBlas;
using UnitTest_ItalianPizza.TestJavierBlas.Models;
using UnitTestItalianPizza.TestJavierBlas.Contracts;

namespace UnitTest_ItalianPizza
{

    [TestClass]
    public class ItalianPizzaTest
    {

        private ItalianPizzaService italianPizzaService = new ItalianPizzaService();

        [TestMethod]
        public void RegisterEmployeException()
        {
            //Arrange
            Exception exception = null;
            DateTime fecha = new DateTime(1989, 11, 2);
            try
            {
                CustomerContract customerContract = new CustomerContract
                {
                    DateOfBirth = fecha,
                    Email = "javi@gmail.com",
                    IdUserCustomer = 4,
                    IsEnabled = true,
                    LastName = "Blas Méndez",
                    Name = "Daniel Alberto",
                    Phone = "2288990011",
                    Status = "Available",
                };

                List<AddressContract> addressContract = new List<AddressContract>();
                addressContract.Add(new AddressContract
                {
                    Colony = "Villa Rica",
                    City = "Xalapa",
                    InsideNumber = "8",
                    OutsideNumber = "10",
                    PostalCode = "91030",
                    IdCustomer = 5,
                    StreetName = "Fausto Vega"
                });

                //Act
                int result = italianPizzaService.RegisterCustomer(customerContract, addressContract);
            }
            catch (Exception ex) 
            {
                exception = ex;
            }
            //AssertAssert.IsNotNull(exception);
        }

        [TestMethod]
        public void GetCustomerListSortedByNameSuccesfull()
        {
            //Arrange
            List<CustomerContract> customerContract;
            //Act
            customerContract = italianPizzaService.GetCustomerListSortedByName();
            //Assert
            Assert.IsTrue(customerContract.Count > 0);
        }

        [TestMethod]
        public void DeleteCustomerSuccessful()
        {
            //Arrange
            int idCustomer = 3;
            int result;

            //Act
            result = italianPizzaService.DeleteCustomerById(idCustomer);

            //Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void UpdateCustomerException()
        {
            //Arrange
            Exception exception = null;
            int result = 0;
            DateTime fecha = new DateTime(1989, 11, 2);
            try
            {
                CustomerContract customerContract = new CustomerContract
                {
                    DateOfBirth = fecha,
                    Email = "javi@gmail.com",
                    IdUserCustomer = 4,
                    IsEnabled = true,
                    LastName = "Blas Méndez",
                    Name = "Daniel Alberto",
                    Phone = "2288990011",
                    Status = "Available",
                };

                List<AddressContract> addressContract = new List<AddressContract>();
                addressContract.Add(new AddressContract
                {
                    Colony = "Villa Rica",
                    City = "Xalapa",
                    InsideNumber = "8",
                    OutsideNumber = "10",
                    PostalCode = "91030",
                    IdCustomer = 5,
                    StreetName = "Fausto Vega"
                });

                //Act
                result = italianPizzaService.UpdateCustomer(customerContract, addressContract);
                Assert.AreNotEqual(1, result);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            Assert.AreNotEqual(exception, result);
        }

        [TestMethod]
        public void LoginEmployeeSuccessful()
        {
            //Arrange
            bool result = false;
            
            EmployeeContract employeeContract = new EmployeeContract { 
                Username = "AntonioD",
                Password = "123"
            };

            LogOutContract logOutContract = new LogOutContract
            {
                TimeOfEntry = "7",
                DepartureTime = "0"
            };

            //Act
            result = italianPizzaService.LoginEmployee(employeeContract, logOutContract);
          
            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void LoginEmployeeUnsuccessful()
        {
            //Arrange
            bool result = false;

            EmployeeContract employeeContract = new EmployeeContract
            {
                Username = "AntoniD",
                Password = "12"
            };

            LogOutContract logOutContract = new LogOutContract
            {
                TimeOfEntry = "7",
                DepartureTime = "0"
            };

            //Act
            result = italianPizzaService.LoginEmployee(employeeContract, logOutContract);

            //Assert
            Assert.AreNotEqual(true, result);
        }

        [TestMethod]
        public void UpdateLoginEmployeeSuccessful()
        {
            //Arrange
            int result = 0;

            string username = "AntonioD";

            LogOutContract logOutContract = new LogOutContract
            {
                TimeOfEntry = "08:53:27 p. m.",
                DepartureTime = "0"
            };

            //Act
            result = italianPizzaService.UpdateLogin(username, logOutContract);

            //Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void RegisterEmployeSuccessful()
        {
            //Arrange
            DateTime fecha = new DateTime(1989, 11, 2);

            EmployeeContract employeeContract = new EmployeeContract
            {
                Email = "androsQ@gmail.com",
                Name = "Carlos",
                LastName = "García",
                Phone = "223322330011",
                Status = "Available",
                AdmissionDate = fecha,
                Password = "1234",
                Role = "Mesero",
                Salary = decimal.Parse("1500"),
                Username = "CarlosA"
            };

            WorkshiftContract workshiftContract = new WorkshiftContract
            {
                IdUserEmployee = 4,
                DepartureTime = "19",
                TimeOfEntry = "9",
                Time = "10"
            };

            //Act
            int result = italianPizzaService.RegisterEmployee(employeeContract, workshiftContract);

            //Assert
            Assert.IsTrue(result > 0);
        }

        [TestMethod]
        public void DeleteEmployeeByIdSuccessful()
        {
            //Arrange
            int idEmployee = 3;
            int result;

            //Act
            result = italianPizzaService.DeleteEmployeeById(idEmployee);

            //Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void GetEmployeeListSortedByNameSuccesfull()
        {
            //Arrange
            List<EmployeeContract> employeeContracts;
            //Act
            employeeContracts = italianPizzaService.GetEmployeeListSortedByName();
            //Assert
            Assert.IsTrue(employeeContracts.Count > 0);
        }

        [TestMethod]
        public void UpdateEmployeeException()
        {
            //Arrange
            Exception exception = null;
            int result = 0;
            DateTime fecha = new DateTime(1989, 11, 2);
            try
            {
                EmployeeContract employeeContract = new EmployeeContract
                {
                    Email = "androsQ@gmail.com",
                    Name = "Andros Carlos",
                    LastName = "García Cruz",
                    Phone = "223322330011",
                    Status = "Available",
                    AdmissionDate = fecha,
                    Password = "1234",
                    Role = "Mesero",
                    Salary = decimal.Parse("1500"),
                    Username = "CarlosA"
                };

                WorkshiftContract workshiftContract = new WorkshiftContract
                {
                    IdUserEmployee = 1011,
                    DepartureTime = "19",
                    TimeOfEntry = "9",
                    Time = "10"
                };

                //Act
                result = italianPizzaService.UpdateEmployee(employeeContract, workshiftContract);
                Assert.AreNotEqual(1, result);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            //Assert
            Assert.AreNotEqual(exception, result);
        }

        [TestMethod]
        public void GetEmployeeRoleTestSuccessful()
        {
            //Arrange
            string usernameEmployee = "AntonioD";
            string role;

            //Act
            role = italianPizzaService.GetEmployeeRole(usernameEmployee);

            //Assert
            Assert.AreEqual("Mesero", role);
        }

        [TestMethod]
        public void GetEmployeeWorkshift()
        {
            //Arrage
            int idEmployee = 1;
            WorkshiftContract workshiftContract;
            //Act
            workshiftContract = italianPizzaService.GetEmployeeWorkshift(idEmployee);
            //Assert
            Assert.IsNotNull(workshiftContract);
        }

        [TestMethod]
        public void GetEmployeeLogOutSuccessful()
        {
            //Arrage
            string usernameEmployee = "AntonioD";
            LogOutContract logOutContract; 
            //Act
            logOutContract = italianPizzaService.GetEmployeeLogOut(usernameEmployee);
            //Assert
            Assert.IsNotNull(logOutContract);
        }

        [TestMethod]
        public void GetEmployeeRoleUnsuccessful()
        {
            //Arrange
            string usernameEmployee = "AntonioD";
            string role;

            //Act
            ItalianPizzaService italianPizzaService = new ItalianPizzaService();
            role = italianPizzaService.GetEmployeeRole(usernameEmployee);

            //Assert
            Assert.AreNotEqual("Contador", role);
        }
    }
}
