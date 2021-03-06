//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestProyectItalianPizza.TestCamarilloVilla.Models
{
    using System;
    using System.Data.Common;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    //using System.Data.Objects;
    //using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class ItalianPizzaContext : DbContext
    {
        public ItalianPizzaContext()
            : base("name=ItalianPizzaContext")
        {
        }

        public ItalianPizzaContext(DbConnection connection)
            : base(connection, contextOwnsConnection: true)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<DailyBalance> DailyBalances { get; set; }
        public DbSet<MonetaryExpens> MonetaryExpenses { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LogOut> LogOuts { get; set; }
        public DbSet<WorkShift> WorkShifts { get; set; }
        public DbSet<FoodRecipe> FoodRecipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<QuantityFoodRecipe> QuantityFoodRecipes { get; set; }
        public DbSet<QuantityItem> QuantityItems { get; set; }
        public DbSet<Stocktaking> Stocktakings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ProviderOrder> ProviderOrders { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<QuantityProviderOrder> QuantityProviderOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    
        public virtual int RegisterFoodRecipe(Nullable<decimal> price, string name, string description, string numberOfServings, string status, Nullable<int> isEnable)
        {
            var priceParameter = price.HasValue ?
                new ObjectParameter("Price", price) :
                new ObjectParameter("Price", typeof(decimal));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var descriptionParameter = description != null ?
                new ObjectParameter("Description", description) :
                new ObjectParameter("Description", typeof(string));
    
            var numberOfServingsParameter = numberOfServings != null ?
                new ObjectParameter("NumberOfServings", numberOfServings) :
                new ObjectParameter("NumberOfServings", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("Status", status) :
                new ObjectParameter("Status", typeof(string));
    
            var isEnableParameter = isEnable.HasValue ?
                new ObjectParameter("IsEnable", isEnable) :
                new ObjectParameter("IsEnable", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RegisterFoodRecipe", priceParameter, nameParameter, descriptionParameter, numberOfServingsParameter, statusParameter, isEnableParameter);
        }
    
        public virtual int SPI_Customer(string email, string name, string lastName, string phone, string status, Nullable<System.DateTime> dateOfBirth, Nullable<bool> isEnabled, Nullable<int> idEmployee, ObjectParameter estado, ObjectParameter salida)
        {
            var emailParameter = email != null ?
                new ObjectParameter("email", email) :
                new ObjectParameter("email", typeof(string));
    
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("lastName", lastName) :
                new ObjectParameter("lastName", typeof(string));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("phone", phone) :
                new ObjectParameter("phone", typeof(string));
    
            var statusParameter = status != null ?
                new ObjectParameter("status", status) :
                new ObjectParameter("status", typeof(string));
    
            var dateOfBirthParameter = dateOfBirth.HasValue ?
                new ObjectParameter("dateOfBirth", dateOfBirth) :
                new ObjectParameter("dateOfBirth", typeof(System.DateTime));
    
            var isEnabledParameter = isEnabled.HasValue ?
                new ObjectParameter("isEnabled", isEnabled) :
                new ObjectParameter("isEnabled", typeof(bool));
    
            var idEmployeeParameter = idEmployee.HasValue ?
                new ObjectParameter("idEmployee", idEmployee) :
                new ObjectParameter("idEmployee", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SPI_Customer", emailParameter, nameParameter, lastNameParameter, phoneParameter, statusParameter, dateOfBirthParameter, isEnabledParameter, idEmployeeParameter, estado, salida);
        }
    
        public virtual ObjectResult<SPS_Employee_Workshifts_Result> SPS_Employee_Workshifts(Nullable<int> iDEmployee)
        {
            var iDEmployeeParameter = iDEmployee.HasValue ?
                new ObjectParameter("iDEmployee", iDEmployee) :
                new ObjectParameter("iDEmployee", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPS_Employee_Workshifts_Result>("SPS_Employee_Workshifts", iDEmployeeParameter);
        }
    }
}
