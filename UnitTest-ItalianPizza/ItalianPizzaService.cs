using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestItalianPizza.TestJavierBlas.Contracts;
using System.Data.Entity.Validation;
using UnitTest_ItalianPizza.TestJavierBlas.Models;

namespace UnitTest_ItalianPizza.TestJavierBlas
{
    public class ItalianPizzaService
    {
        private ItalianPizzaContext context = new ItalianPizzaContext();

        public bool IdentifyChangesInAddresses(List<AddressContract> addressContracts, int customerId)
        {
            List<Address> addresses = context.Addresses.ToList();
            int customerAddressNumber = context.Addresses.Where(x => x.IdCustomer == customerId).Count();
            int newCustomerAddress = addressContracts.Count;
            bool newAddress = customerAddressNumber == addressContracts.Count ? false : true;

            foreach (AddressContract addressContract in addressContracts)
            {
                if (addressContract.IdAddresses == 0)
                {
                    newAddress = true;
                }
            }

            return newAddress;
        }

        public int RegisterCustomerAddresses(List<AddressContract> addressContracts, int customerId)
        {
            List<Address> addresses = new List<Address>();

            foreach (AddressContract address in addressContracts)
            {
                context.Addresses.Add(new Address
                {
                    Colony = address.Colony,
                    City = address.City,
                    InsideNumber = address.InsideNumber,
                    OutsideNumber = address.OutsideNumber,
                    PostalCode = address.PostalCode,
                    IdCustomer = customerId,
                    StreetName = address.StreetName
                });
            }

            int result = context.SaveChanges();

            return result;
        }

        public int RegisterCustomer(CustomerContract customerContract, List<AddressContract> addresses)
        {
            int result = 0;

            bool isExisting = context.Customers.Where(x => x.Name.Equals(customerContract.Name) &&
                x.LastName.Equals(customerContract.LastName) && x.IsEnabled).Count() > 0;

            Customer customer = new Customer
            {
                Email = customerContract.Email,
                Name = customerContract.Name,
                LastName = customerContract.LastName,
                Phone = customerContract.Phone,
                Status = "Available",
                DateOfBirth = customerContract.DateOfBirth,
                IsEnabled = true,
                IdEmployee = customerContract.IdEmployee
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.Customers.Add(customer);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int customerId = context.Customers.Max(x => x.IdUserCustomer);
                        result = RegisterCustomerAddresses(addresses, customerId);
                    }
                    else
                    {
                        context.Customers.Remove(customer);
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

        public int DeleteCustomerById(int idCustomer)
        {
            Customer customer = context.Customers.Where(x => x.IdUserCustomer == idCustomer).FirstOrDefault();
            bool haveOrders = context.Orders.Where(order => order.IdCustomer == customer.IdUserCustomer).Count() > 0;
            int result = 0;

            if (customer != null && !haveOrders)
            {
                customer.IsEnabled = false;
                customer.Status = "Deleted";
                result = context.SaveChanges();
            }
            else
            {
                result = -1;
            }

            return result;
        }

        public List<CustomerContract> GetCustomerListSortedByName()
        {
            List<Customer> customers = context.Customers.
                Where(x => (x.IsEnabled == true) && x.Status.Equals("Available")).
                OrderBy(x => x.Name).ToList();
            List<CustomerContract> customerContracts = new List<CustomerContract>();

            List<Address> addresses = context.Addresses.ToList();
            List<AddressContract> addressContracts = new List<AddressContract>();

            foreach (Customer customer in customers)
            {
                customerContracts.Add(new CustomerContract
                {
                    IdUserCustomer = customer.IdUserCustomer,
                    Email = customer.Email,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Phone = customer.Phone,
                    Status = customer.Status,
                    DateOfBirth = customer.DateOfBirth,
                    IsEnabled = customer.IsEnabled,
                    IdEmployee = customer.IdEmployee
                });
            }

            foreach (Address address in addresses)
            {
                addressContracts.Add(new AddressContract
                {
                    Colony = address.Colony,
                    City = address.City,
                    InsideNumber = address.InsideNumber,
                    OutsideNumber = address.OutsideNumber,
                    PostalCode = address.PostalCode,
                    IdCustomer = address.IdCustomer,
                    StreetName = address.StreetName,
                    IdAddresses = address.IdAddresses
                });
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetCustomerListSortedByName(
            //    customerContracts, addressContracts
            //);
            return customerContracts;
        }

        public int UpdateCustomerAddresses(int idCustomer)
        {
            int result = 0;

            List<Address> addresses = context.Addresses.ToList();

            foreach (Address address in addresses)
            {
                if (address.IdCustomer == idCustomer)
                {
                    context.Addresses.Remove(address);
                    result = context.SaveChanges();
                }
            }

            return result;
        }

        public int UpdateCustomer(CustomerContract customerContract, List<AddressContract> addressContracts)
        {
            Customer customer = context.Customers.Where(x => x.IdUserCustomer == customerContract.IdUserCustomer).FirstOrDefault();

            int result = 0;
            bool isExisting = context.Customers.Where(x => x.Name.Equals(customerContract.Name) &&
                    x.LastName.Equals(customerContract.LastName) && x.IsEnabled &&
                        x.IdUserCustomer != customer.IdUserCustomer).Count() > 0;
            bool newAddress = IdentifyChangesInAddresses(addressContracts, customer.IdUserCustomer);

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (customer != null)
                {
                    customer.Email = customerContract.Email;
                    customer.Name = customerContract.Name;
                    customer.LastName = customerContract.LastName;
                    customer.Phone = customerContract.Phone;
                    customer.Status = customerContract.Status;
                    customer.DateOfBirth = customerContract.DateOfBirth;
                    customer.IsEnabled = customerContract.IsEnabled;

                    result = context.SaveChanges();
                    context.SaveChanges();
                }

                if ((result > 0) || newAddress)
                {
                    result = UpdateCustomerAddresses(customerContract.IdUserCustomer);

                    if (result > 0)
                    {
                        result = RegisterCustomerAddresses(addressContracts, customerContract.IdUserCustomer);
                    }
                }
            }

            return result;
        }

        public bool LoginEmployee(EmployeeContract employee, LogOutContract logOutContract)
        {

            //Employee employeeLogin = context.Employees.FirstOrDefault(u => u.Username.Equals(employee.Username));
            Employee employeeLogin = context.Employees.Where(u => u.Username.Equals(employee.Username)).FirstOrDefault();
            //EmployeeContract employeeContract = new EmployeeContract();
            bool confirmLogin = false;

            if (employeeLogin != null)
            {
                employee.IdUserEmployee = employeeLogin.IdUserEmployee;
                employee.Name = employeeLogin.Name;
                employee.LastName = employeeLogin.LastName;
                employee.Role = employeeLogin.Role;
                employee.Username = employeeLogin.Username;
                employee.Password = employeeLogin.Password;
                employee.Email = employeeLogin.Email;

                context.LogOuts.Add(new LogOut
                {
                    DepartureTime = "0",
                    TimeOfEntry = logOutContract.DepartureTime,
                    IdUserEmployee = employeeLogin.IdUserEmployee
                });

                context.SaveChanges();

                if (employeeLogin.Username.Equals(employee.Username) && employeeLogin.Password.Equals(employee.Password))
                {
                    confirmLogin = true;
                }
            }
            else
            {
                Console.WriteLine("Fallo :u");
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().LoginEmployee(employee, confirmLogin);
            return confirmLogin;
        }

        public int UpdateLogin(string usernameEmployee, LogOutContract logOutContract)
        {
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();
            int employeeID = employee.IdUserEmployee;
            string departureTime = "0";
            LogOut logOut = context.LogOuts.Where(x => x.IdUserEmployee.Equals(employeeID) && x.DepartureTime.Equals(departureTime)).FirstOrDefault();
            string timeExit = DateTime.Now.ToString("T");

            if (logOut != null)
            {
                logOut.DepartureTime = timeExit;
            }

            int result = context.SaveChanges();

            return result;
        }

        public int RegisterEmployeeWorkshifts(WorkshiftContract workshiftsContracts, int employeeId)
        {
            context.WorkShifts.Add(new WorkShift
            {
                DepartureTime = workshiftsContracts.DepartureTime,
                Time = workshiftsContracts.Time,
                TimeOfEntry = workshiftsContracts.TimeOfEntry,
                IdUserEmployee = employeeId
            });

            int result = context.SaveChanges();

            return result;
        }

        public int RegisterEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContract)
        {
            int result = 0;

            bool isExisting = context.Employees.Where(x => x.Name.Equals(employeeContract.Name) &&
            x.LastName.Equals(employeeContract.LastName)).Count() > 0;

            Employee employee = new Employee
            {
                Email = employeeContract.Email,
                Name = employeeContract.Name,
                LastName = employeeContract.LastName,
                Phone = employeeContract.Phone,
                Status = "Available",
                AdmissionDate = employeeContract.AdmissionDate,
                Password = employeeContract.Password,
                Role = employeeContract.Role,
                Salary = employeeContract.Salary,
                Username = employeeContract.Username
            };

            try
            {
                if (isExisting)
                {
                    result = -1;
                }
                else
                {
                    context.Employees.Add(employee);
                    result = context.SaveChanges();

                    if (result > 0)
                    {
                        int employeeId = context.Employees.Max(x => x.IdUserEmployee);
                        result = RegisterEmployeeWorkshifts(workshiftContract, employeeId);
                    }
                    else
                    {
                        context.Employees.Remove(employee);
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

        public int DeleteEmployeeById(int idEmployee)
        {
            Employee employee = context.Employees.Where(x => x.IdUserEmployee == idEmployee).FirstOrDefault();
            int result = 0;

            if (employee != null)
            {
                employee.Status = "Leave";
                result = context.SaveChanges();
            }
            else
            {
                result = -1;
            }
            return result;
        }

        public List<EmployeeContract> GetEmployeeListSortedByName()
        {
            List<Employee> employees = context.Employees.
            Where(x => x.Status.Equals("Available")).
            OrderBy(x => x.Name).ToList();
            List<EmployeeContract> employeeContracts = new List<EmployeeContract>();

            foreach (Employee employee in employees)
            {

                WorkShift workshift = context.WorkShifts.Where(w => w.IdUserEmployee.Equals(employee.IdUserEmployee)).FirstOrDefault();

                employeeContracts.Add(new EmployeeContract
                {
                    IdUserEmployee = employee.IdUserEmployee,
                    Email = employee.Email,
                    Name = employee.Name,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Status = employee.Status,
                    AdmissionDate = employee.AdmissionDate,
                    Password = employee.Password,
                    Role = employee.Role,
                    Salary = employee.Salary,
                    Username = employee.Username,
                    Workshift = workshift.TimeOfEntry + "-" + workshift.DepartureTime
                });
            }

            //OperationContext.Current.GetCallbackChannel<IItalianPizzaServiceCallback>().GetEmployeeListSortedByName(employeeContracts);
            return employeeContracts;
        }

        public int UpdateEmployeeWorkshift(int idEmployee, WorkshiftContract workshiftContracts)
        {
            int result = 1;

            WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(idEmployee)).FirstOrDefault();

            workShift.DepartureTime = workshiftContracts.DepartureTime;
            workShift.Time = workshiftContracts.Time;
            workShift.TimeOfEntry = workshiftContracts.TimeOfEntry;

            context.SaveChanges();

            return result;
        }

        public int UpdateEmployee(EmployeeContract employeeContract, WorkshiftContract workshiftContracts)
        {
            Employee employee = context.Employees.Where(x => x.IdUserEmployee.Equals(employeeContract.IdUserEmployee)).FirstOrDefault();
            //WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(employeeContract.IdUserEmployee)).FirstOrDefault();

            int result = 0;
            bool isExisting = context.Employees.Where(x => x.Name.Equals(employeeContract.Name) &&
            x.LastName.Equals(employeeContract.LastName) && x.IdUserEmployee != employee.IdUserEmployee).Count() > 0;

            if (isExisting)
            {
                result = -1;
            }
            else
            {
                if (employee != null)
                {
                    employee.Email = employeeContract.Email;
                    employee.Name = employeeContract.Name;
                    employee.LastName = employeeContract.LastName;
                    employee.Phone = employeeContract.Phone;
                    employee.Status = employeeContract.Status;
                    employee.AdmissionDate = employeeContract.AdmissionDate;
                    employee.Password = employeeContract.Password;
                    employee.Role = employeeContract.Role;
                    employee.Salary = employeeContract.Salary;
                    UpdateEmployeeWorkshift(employeeContract.IdUserEmployee, workshiftContracts);

                    result = context.SaveChanges();
                }
                if ((result >= 0))
                {
                    result = UpdateEmployeeWorkshift(employeeContract.IdUserEmployee, workshiftContracts);
                }
            }
            return result;
        }

        public WorkshiftContract GetEmployeeWorkshift(int idEmployee)
        {
            WorkshiftContract workshiftContracts = new WorkshiftContract();
            WorkShift workShift = context.WorkShifts.Where(x => x.IdUserEmployee.Equals(idEmployee)).FirstOrDefault();

            workshiftContracts.DepartureTime = workShift.DepartureTime;
            workshiftContracts.TimeOfEntry = workShift.TimeOfEntry;
            workshiftContracts.Time = workShift.Time;

            return workshiftContracts;
        }

        public LogOutContract GetEmployeeLogOut(string usernameEmployee)
        {
            EmployeeContract employeeContract = new EmployeeContract();
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();
            LogOutContract logOutContract = new LogOutContract();
            LogOut logOut = context.LogOuts.Where(x => x.IdUserEmployee.Equals(employee.IdUserEmployee)).FirstOrDefault();

            logOutContract.IdUserEmployee = logOut.IdUserEmployee;
            logOutContract.TimeOfEntry = logOut.TimeOfEntry;
            logOutContract.DepartureTime = logOut.DepartureTime;
            logOutContract.IdLogOut = logOut.IdLogOut;

            return logOutContract;
        }

        public string GetEmployeeRole(string usernameEmployee)
        {
            Employee employee = context.Employees.Where(x => x.Username.Equals(usernameEmployee)).FirstOrDefault();

            string employeeRole = employee.Role;

            return employeeRole;
        }
    }
}
