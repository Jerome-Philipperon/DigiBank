using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DomainModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public static class BankInitializer
    {
        /// <summary>
        /// Initializer and choose if drop create always
        /// </summary>
        /// <param name="context"></param>
        /// <param name="dataCreateAlways">want create always or not</param>
        public static void Initialization(this BankContext context, bool dataCreateAlways)
        {
            if (dataCreateAlways)
                context.Database.EnsureDeleted();

            context.Database.EnsureCreated();

            if (context.Clients.Any())
                return;

            #region Managers
            var managers = new List<Manager>()
            {
                new Manager
                {
                    FirstName = "Loth",
                    LastName = "Dorcanie",
                    DateOfBirth = new DateTime(1970,04,30),
                    OfficeName = "Lyon 2",
                    IsJunior = false,
                    //MyEmployees = {employees[0], employees[1], employees[2]},
                },
                new Manager
                {
                    FirstName = "Leodagan",
                    LastName = "De Carmelide",
                    DateOfBirth = new DateTime(1968,07,22),
                    OfficeName = "Lyon 2",
                    IsJunior = true,
                    //MyEmployees = {employees[0], employees[1], employees[2]},
                },
            };

            //Todo tentative d'implementation des password a suppr ou si on trouve
            //UserStore<Manager> store = new UserStore<Manager>(context);
            //store.CreateAsync(managers.First());

            //UserManager<Manager> userManager = new UserManager<Manager>(store, null, null, null);
            
            context.Managers.AddRange(managers);
            #endregion

            #region Employees
            var employees = new List<Employee>()
            {
                new Employee
                {
                    FirstName = "Venec",
                    LastName = "Bandit",
                    DateOfBirth = new DateTime(1987,07,15),
                    OfficeName = "Lyon 2",
                    IsJunior = false,
                    MyManager = managers[0],
                },
                new Employee
                {
                    FirstName = "Attila",
                    LastName = "Chefdeshuns",
                    DateOfBirth = new DateTime(1996,06,19),
                    OfficeName = "Lyon 2",
                    IsJunior = true,
                    MyManager = managers[0],
                },
                new Employee
                {
                    FirstName = "Blaise",
                    LastName = "Cure",
                    DateOfBirth = new DateTime(1973,08,15),
                    OfficeName = "Lyon 2",
                    IsJunior = false,
                    MyManager = managers[1],
                },
            };
            context.Employees.AddRange(employees);
            #endregion

            #region Clients
            var clients = new List<Client>()
            {
                new Client
                {
                    FirstName = "Arthur",
                    LastName = "Pendragon",
                    DateOfBirth = new DateTime(1963,08,15),
                    Street = "15 Rue des Alizés",
                    ZipCode = "68100",
                    City = "Mulhouse",
                    MyEmployee = employees[0],
                    //MyAccounts = {savings[0]},
                },
                new Client
                {
                    FirstName = "Lancelot",
                    LastName = "Dulac",
                    DateOfBirth = new DateTime(1986,03,08),
                    Street = "168 avenue Charles de Gaulle",
                    ZipCode = "59000",
                    City = "Lille",
                    MyEmployee = employees[1],
                    //MyAccounts = {savings[1], deposits[0]},
                },
                new Client
                {
                    FirstName = "Guenièvre",
                    LastName = "Reinedebretagne",
                    DateOfBirth = new DateTime(1995,10,09),
                    Street = "5 impasse des lilas",
                    ZipCode = "69001",
                    City = "Lyon",
                    MyEmployee = employees[1],
                    //MyAccounts = {deposits[1]},
                },
                new Client
                {
                    FirstName = "Perceval",
                    LastName = "Legallois",
                    DateOfBirth = new DateTime(1948,02,16),
                    Street = "259 cours Benjamin Franklin",
                    ZipCode = "06200",
                    City = "Nice",
                    MyEmployee = employees[2],
                    //MyAccounts = {deposits[2]},
                },
                new Client
                {
                    FirstName = "Karadoc",
                    LastName = "Devannes",
                    DateOfBirth = new DateTime(1956,03,03),
                    Street = "39 rue de Marignan",
                    ZipCode = "45100",
                    City = "Blois",
                    MyEmployee = employees[2],
                    //MyAccounts = {deposits[3]},
                },
            };
            context.Clients.AddRange(clients);
            #endregion

            #region Saving
            var savings = new List<Saving>()
            {
                new Saving
                {
                    BankCode = "13458",
                    BranchCode = "79613",
                    AccountNumber = "46237816492",
                    Key = "13",
                    BBAN = "13458796134623781649213",
                    IBAN = "FR7613458796134623781649213",
                    BIC = "D789A41EUI9",
                    Balance = 519.42M,
                    AccountOwner = clients[0],
                    MinimumAmount = 300,
                    MaximumAmount = 2000,
                    InterestRate = 1.5,
                    MaximumDate = new DateTime(2025,01,01),
                },
                new Saving
                {
                    BankCode = "46893",
                    BranchCode = "13486",
                    AccountNumber = "46279634192",
                    Key = "42",
                    BBAN = "46893134864627963419242",
                    IBAN = "FR7646893134864627963419242",
                    BIC = "D779B31DS64",
                    Balance = 3482.10M,
                    AccountOwner = clients[1],
                    MinimumAmount = 300,
                    InterestRate = 1.5,
                },
            };
            context.Savings.AddRange(savings);
            #endregion

            #region Cards
            var cards = new List<Card>()
            {
                new Card
                {
                    NetworkIssuer = "Visa",
                    CardNumber = "4109429309081349",
                    SecurityCode = "8996",
                    ExpirationDate = new DateTime(2021,08,31),
                },
                new Card
                {
                    NetworkIssuer = "Mastercard",
                    CardNumber = "5896124367815623",
                    SecurityCode = "1664",
                    ExpirationDate = new DateTime(2022,04,30),
                },
                new Card
                {
                    NetworkIssuer = "Amex",
                    CardNumber = "8974014163969462",
                    SecurityCode = "1128",
                    ExpirationDate = new DateTime(2020,07,31),
                },
                new Card
                {
                    NetworkIssuer = "Visa",
                    CardNumber = "4109795164846232",
                    SecurityCode = "4856",
                    ExpirationDate = new DateTime(2021,10,31),
                },
                new Card
                {
                    NetworkIssuer = "Visa",
                    CardNumber = "4109636346596301",
                    SecurityCode = "0509",
                    ExpirationDate = new DateTime(2021,12,31),
                },
            };
            context.Cards.AddRange(cards);
            #endregion

            #region Deposits
            var deposits = new List<Deposit>()
            {
                new Deposit
                {
                    BankCode = "75692",
                    BranchCode = "13467",
                    AccountNumber = "93445624525",
                    Key = "99",
                    BBAN = "75692134679344562452599",
                    IBAN = "FR7675692134679344562452599",
                    BIC = "D46FQ13452A",
                    Balance = 23.55M,
                    AccountOwner = clients[1],
                    CreationDate = new DateTime(1999,08,05),
                    AutorizedOverdraft = 1000,
                    FreeOverdraft = 50,
                    OverdraftChargeRate = 5.00M,
                    DepositCards = new List<Card> {cards[3], cards[4]},
                },
                new Deposit
                {
                    BankCode = "19753",
                    BranchCode = "64987",
                    AccountNumber = "15648535665",
                    Key = "78",
                    BBAN = "19753649871564853566578",
                    IBAN = "FR7619753649871564853566578",
                    BIC = "DQ4589FG19P",
                    Balance = 618.98M,
                    AccountOwner = clients[2],
                    CreationDate = new DateTime(2018,12,13),
                    AutorizedOverdraft = 500,
                    OverdraftChargeRate = 5.00M,
                    DepositCards = new List<Card> {cards[2]},
                },
                new Deposit
                {
                    BankCode = "98135",
                    BranchCode = "46215",
                    AccountNumber = "94534561234",
                    Key = "16",
                    BBAN = "98135462159453456123416",
                    IBAN = "FR7698135462159453456123416",
                    BIC = "PM7954ER23F",
                    Balance = 992.10M,
                    AccountOwner = clients[3],
                    CreationDate = new DateTime(2013,04,24),
                    AutorizedOverdraft = 200,
                    OverdraftChargeRate = 5.00M,
                    DepositCards = new List<Card> {cards[1]},
                },
                new Deposit
                {
                    BankCode = "79256",
                    BranchCode = "13456",
                    AccountNumber = "49756324561",
                    Key = "46",
                    BBAN = "79256134564975632456146",
                    IBAN = "FR7679256134564975632456146",
                    BIC = "JM4682DS12Q",
                    Balance = 111.11M,
                    AccountOwner = clients[4],
                    CreationDate = new DateTime(2015,01,14),
                    AutorizedOverdraft = 500,
                    OverdraftChargeRate = 5.00M,
                    DepositCards = new List<Card> { cards[0] },
                },
            };
            context.Deposits.AddRange(deposits);
            #endregion

            context.SaveChanges();
        }
    }
}
