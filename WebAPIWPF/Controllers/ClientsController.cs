using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DomainModel;

namespace WebAPIWPF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly BankContext _context;

        public ClientsController(BankContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<WebTransferClients>>> GetClients()
        {
            List<Client> cls = await _context.Clients.ToListAsync();
            List<WebTransferClients> clients = new List<WebTransferClients>();
            foreach (Client client in cls)
            {
                clients.Add(new WebTransferClients(client.Id, client.FirstName, client.LastName, client.DateOfBirth, client.Street, client.ZipCode, client.City));
            }

            return clients;
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WebTransferClients>> GetClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return new WebTransferClients(client.Id, client.FirstName, client.LastName, client.DateOfBirth, client.Street, client.ZipCode, client.City);
        }

        // GET: api/Clients/NbClient
        //Get Nombre De Client De La Banque
        [HttpGet("NbClient")]
        public async Task<ActionResult<Int32>> GetNbClient()
        {
            return await _context.Clients.CountAsync();
        }

        //Get Nombre De Client De La Banque Par Manager
        [HttpGet("NbClientByManager")]
        public async Task<ActionResult<List<List<string>>>> GetNbClientByManager()
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var manager in await _context.Managers.ToListAsync())
            {
                var nbClientParManager = 0;
                foreach (var employee in await _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToListAsync())
                {
                    nbClientParManager += await _context.Clients.Where(cl => cl.MyEmployee == employee).CountAsync();
                }
                result.Add(new List<string> { $"{manager.FirstName} {manager.LastName}", nbClientParManager.ToString() });
            }
            return result;
        }

        //Get Somme Epargne Par Clients
        [HttpGet("SavingsAmountByClients")]
        public async Task<ActionResult<List<List<string>>>> GetSavingsAmountByClients()
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var client in await _context.Clients.ToListAsync())
            {
                //var ListAccountParClients = client.MyAccounts.Where(acc => acc.AccountOwner == client).ToList();
                decimal epargneParClient = 0M;
                var ListAccountSavingByClient = await _context.Savings.Where(sav => sav.AccountOwner == client).ToListAsync();
                foreach (var saving in ListAccountSavingByClient)
                {
                    epargneParClient += saving.Balance;
                }
                result.Add(new List<string> { $"{client.FirstName} {client.LastName}", epargneParClient.ToString() });
            }
            return result;
        }

        //Get Somme Epargne Par Clients Par Manager
        [HttpGet("SavingsAmountByClientsByManager")]
        public async Task<ActionResult<List<List<string>>>> GetSavingsAmountByClientsByManager()
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var manager in await _context.Managers.ToListAsync())
            {
                decimal epargneParManager = 0M;
                var ListEmployeesParManager = await _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToListAsync();
                foreach (var employee in ListEmployeesParManager)
                {
                    foreach (var client in await _context.Clients.Where(cl => cl.MyEmployee == employee).ToListAsync())
                    {
                        var ListAccountSavingByClient = await _context.Savings.Where(sav => sav.AccountOwner == client).ToListAsync();
                        foreach (var saving in ListAccountSavingByClient)
                        {
                            epargneParManager += saving.Balance;
                        }
                    }
                }
                result.Add(new List<string> { $"{manager.FirstName} {manager.LastName}", epargneParManager.ToString() });
            }
            return result;
        }

        //solde total de l’ensemble des comptes de la banque 
        [HttpGet("TotalBalanceOfBank")]
        public async Task<ActionResult<decimal>> GetTotalBalanceOfBank()
        {
            decimal soldeTotal = 0M;
            foreach (var saving in await _context.Savings.ToListAsync())
            {
                soldeTotal += saving.Balance;
            }
            foreach (var deposit in await _context.Deposits.ToListAsync())
            {
                soldeTotal += deposit.Balance;
            }
            return soldeTotal;
        }

        //Le pourcentage de clients qui possèdent une carte bancaire
        [HttpGet("PercentageOfCustomersWhoHaveCreditCard")]
        public async Task<ActionResult<double>> GetPercentageOfCustomersWhoHaveCreditCard()
        {
            int nbCard = 0;
            foreach (var client in await _context.Clients.ToListAsync())
            {
                foreach (var deposit in await _context.Deposits.Where(sav => sav.AccountOwner == client).ToListAsync())
                {
                    if (deposit != null)
                    {
                        if (await _context.Cards.Where(c => c.CardDeposit == deposit).CountAsync()/*deposit.DepositCards.ToList().Count*/ > 0)
                        {
                            nbCard++;
                        }
                    }
                }
            }
            return Math.Round(((double)nbCard / (double) await _context.Clients.CountAsync()) * 100D,2);
        }

        //Le pourcentage de clients qui possèdent une carte bancaire par manager 
        [HttpGet("PercentageOfCustomersWhoHaveCreditCardByManager")]
        public async Task<ActionResult<List<List<string>>>> GetPercentageOfCustomersWhoHaveCreditCardByManager()
        {
            List<List<string>> pourcentageParManager = new List<List<string>>();
            foreach (var manager in await _context.Managers.ToListAsync())
            {
                int nbCardParManager = 0;
                int nbClientsByManager = 0;
                var ListEmployeesParManager = await _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToListAsync();
                foreach (var employee in ListEmployeesParManager)
                {
                    nbClientsByManager += await _context.Clients.Where(cl => cl.MyEmployee == employee).CountAsync();
                    foreach (var client in await _context.Clients.Where(cl => cl.MyEmployee == employee).ToListAsync())
                    {
                        foreach (var deposit in await _context.Deposits.Where(sav => sav.AccountOwner == client).ToListAsync())
                        {
                            if (deposit != null)
                            {
                                
                                if (await _context.Cards.Where(c => c.CardDeposit == deposit).CountAsync() > 0)
                                {
                                    nbCardParManager++;
                                }
                            }
                        }
                    }
                }
                pourcentageParManager.Add(new List<string> { $"{manager.FirstName} {manager.LastName}", (Math.Round(((double)nbCardParManager / (double)nbClientsByManager) * 100D,2)).ToString() });
            }
            return pourcentageParManager;
        }

        //Le pourcentage de clients qui possèdent un compte d’épargne 
        [HttpGet("PercentageOfCustomersWhoHaveBankAccount")]
        public async Task<ActionResult<double>> GetPercentageOfCustomersWhoHaveBankAccount()
        {
            int nbSaving = 0;
            foreach (var client in await _context.Clients.ToListAsync())
            {
                if (await _context.Savings.Where(sav => sav.AccountOwner == client).CountAsync() > 0)
                {
                    nbSaving++;
                }
            }
            return Math.Round(((double)nbSaving / (double) await _context.Clients.CountAsync()) * 100D,2);
        }

        //Le pourcentage de clients qui possèdent un compte d’épargne par manager
        [HttpGet("PercentageOfCustomersWhoHaveBankAccountByManager")]
        public async Task<ActionResult<List<List<string>>>> GetPercentageOfCustomersWhoHaveBankAccountByManager()
        {
            List<List<string>> result = new List<List<string>>();
            foreach (var manager in await _context.Managers.ToListAsync())
            {
                int nbClientsByManager = 0;
                var ListEmployeesParManager = await _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToListAsync();
                int nbSavingByManager = 0;
                foreach (var employee in ListEmployeesParManager)
                {
                    nbClientsByManager += await _context.Clients.Where(cl => cl.MyEmployee == employee).CountAsync();
                    foreach (var client in await _context.Clients.Where(cl => cl.MyEmployee == employee).ToListAsync())
                    {
                        if (await _context.Savings.Where(sav => sav.AccountOwner == client).CountAsync() > 0)
                        {
                            nbSavingByManager++;
                        }
                    }
                }
                result.Add(new List<string> { $"{manager.FirstName} {manager.LastName}", (Math.Round(((double)nbSavingByManager / (double)nbClientsByManager) * 100D,2)).ToString() });
            }
            return result;
        }

        #region Info Dev


        [HttpGet("Info")]
        public List<string> GetInfo()
        {
            _context.Clients.ToList().Count();
            List<string> result = new List<string>();
            // nombre de client 
            result.Add("nombre de client : " + _context.Clients.ToList().Count());

            // nombre de client par manager
            foreach (var manager in _context.Managers.ToList())
            {
                var nbClientParManager = 0;
                var ListEmployeesParManager = _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToList();
                foreach (var employee in ListEmployeesParManager)
                {
                    nbClientParManager += employee.MyClients.Count();
                }
                result.Add($"nombre de client du {manager.FirstName} {manager.LastName} : {nbClientParManager}");
            }

            // Les sommes épargnées par les clients de la banque 
            foreach (var client in _context.Clients.ToList())
            {
                //var ListAccountParClients = client.MyAccounts.Where(acc => acc.AccountOwner == client).ToList();
                decimal epargneParClient = 0M;
                var ListAccountSavingByClient = _context.Savings.Where(sav => sav.AccountOwner == client).ToList();
                foreach (var saving in ListAccountSavingByClient)
                {
                    epargneParClient += saving.Balance;
                }
                result.Add($"Epargne de {client.FirstName} {client.LastName} : {epargneParClient} €");
            }

            //Les sommes épargnées par les clients par manager 
            foreach (var manager in _context.Managers.ToList())
            {
                decimal epargneParManager = 0M;
                var ListEmployeesParManager = _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToList();
                foreach (var employee in ListEmployeesParManager)
                {
                    foreach (var client in employee.MyClients.ToList())
                    {
                        var ListAccountSavingByClient = _context.Savings.Where(sav => sav.AccountOwner == client).ToList();
                        foreach (var saving in ListAccountSavingByClient)
                        {
                            epargneParManager += saving.Balance;
                        }
                    }
                    //nbClientParManager += employee.MyClients.Count();
                }
                result.Add($"Somme epargne par les clients de {manager.FirstName} {manager.LastName} : {epargneParManager}");
            }

            //Le solde total de l’ensemble des comptes de la banque 
            decimal soldeTotal = 0M;
            foreach (var saving in _context.Savings.ToList())
            {
                soldeTotal += saving.Balance;
            }
            foreach (var deposit in _context.Deposits.ToList())
            {
                soldeTotal += deposit.Balance;
            }
            result.Add($"Le solde total de l’ensemble des comptes de la banque est de {Math.Round(soldeTotal,2)}");


            //Le pourcentage de clients qui possèdent une carte bancaire
            int nbCard = 0;
            foreach (var client in _context.Clients.ToList())
            {
                foreach (var deposit in _context.Deposits.Where(sav => sav.AccountOwner == client).ToList())
                {
                    if (deposit != null)
                    {
                        _context.Cards.ToList();
                        if (deposit.DepositCards.ToList().Count > 0)
                        {
                            nbCard++;
                        }
                    }
                }

            }
            double pourcentage = ((double)nbCard / (double)_context.Clients.Count()) * 100D;
            result.Add($" pourcentage de clients qui possèdent une carte bancaire : {pourcentage}%");

            //Le pourcentage de clients qui possèdent une carte bancaire par manager 
            
            foreach (var manager in _context.Managers.ToList())
            {
                int nbCardParManager = 0;
                int nbClientsByManager = 0;
                var ListEmployeesParManager = _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToList();
                foreach (var employee in ListEmployeesParManager)
                {
                    nbClientsByManager += employee.MyClients.ToList().Count();
                    foreach (var client in employee.MyClients.ToList())
                    {
                        foreach (var deposit in _context.Deposits.Where(sav => sav.AccountOwner == client).ToList())
                        {
                            if (deposit != null)
                            {
                                _context.Cards.ToList();
                                if (deposit.DepositCards.ToList().Count > 0)
                                {
                                    nbCardParManager++;
                                }
                            }
                        }
                    }
                }
                double pourcentageParManager = ((double)nbCardParManager / (double)nbClientsByManager) * 100D;
                result.Add($" pourcentage de clients qui possèdent une carte bancaire par manager : {Math.Round(pourcentageParManager, 2)}%");
            }


            //Le pourcentage de clients qui possèdent un compte d’épargne 
            int nbSaving = 0;
            foreach (var client in _context.Clients.ToList())
            {
                var ListAccountSavingByClient = _context.Savings.Where(sav => sav.AccountOwner == client).ToList();
                if (ListAccountSavingByClient.Count > 0)
                {
                    nbSaving++;
                }
            }
            result.Add($"pourcentage de clients qui possèdent un compte d’épargne : {Math.Round(((double)nbSaving / (double)_context.Clients.Count()) * 100D,2)}%");

            //Le pourcentage de clients qui possèdent un compte d’épargne par manager

            foreach (var manager in _context.Managers.ToList())
            {
                int nbClientsByManager = 0;
                var ListEmployeesParManager = _context.Employees.Where(emp => emp.MyManager.Id == manager.Id).ToList();
                int nbSavingByManager = 0;
                foreach (var employee in ListEmployeesParManager)
                {
                    nbClientsByManager += employee.MyClients.ToList().Count();
                    foreach (var client in employee.MyClients.ToList())
                    {
                        var ListAccountSavingByClient = _context.Savings.Where(sav => sav.AccountOwner == client).ToList();
                        if (ListAccountSavingByClient.Count > 0)
                        {
                            nbSavingByManager++;
                        }
                    }
                }
               result.Add($"pourcentage de clients qui possèdent un compte d’épargne par manager : {Math.Round(((double)nbSavingByManager / (double)nbClientsByManager) * 100D,2)}%");
            }



            return result;
        }
        /*
        // PUT: api/Clients/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.PersonId)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Clients
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.PersonId }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.PersonId == id);
        }
        */

        #endregion
    }
}
