using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DomainModel;
using System.Text.Json;


namespace WebAPIFrontOffice.Controllers
{
    [Route("api")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly BankContext _context;

        public ManagersController(BankContext context)
        {
            _context = context;
        }

        //Login
        #region login
        [HttpGet("login/{firstName}/{lastName}")]
        public async Task<ActionResult<IEnumerable<Manager>>> GetLogin(string firstName, string lastName)
        {
            foreach(var emp in  _context.Employees)
            {
                if(emp.FirstName== firstName && emp.LastName==lastName)
                {
                    if(emp is Manager)
                    {
                        //return HttpResponse.Redirect($"Mangers")
                        
                        return CreatedAtAction("GetManager", new { id = emp.Id }, (Manager)emp);
                    }
                    else
                    {
                        return CreatedAtAction("GetEmployee", new { id = emp.Id }, emp);
                    }
                }
            }
            return NotFound();
        }
        #endregion


        // Managers
        #region Action on Manager By Manager
        // GET: api/Managers
        [HttpGet("[controller]")]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManagers()
        {
            return await _context.Managers.ToListAsync();
        }

        // GET: api/Managers/5
        [HttpGet("[controller]/{id}")]
        public async Task<ActionResult<Manager>> GetManager(string id)
        {
            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            return manager;
        }

        // PUT: api/Managers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("[controller]/{id}")]
        public async Task<IActionResult> PutManager(string id, Manager manager)
        {
            if (id != manager.Id)
            {
                return BadRequest();
            }

            _context.Entry(manager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManagerExists(id))
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


        // POST: api/Managers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("[controller]")]
        public async Task<ActionResult<Manager>> PostManager(Manager manager)
        {
            _context.Managers.Add(manager);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ManagerExists(manager.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetManager", new { id = manager.Id }, manager);
        }

        // DELETE: api/Managers/5
        [HttpDelete("[controller]/{id}")]
        public async Task<ActionResult<Manager>> DeleteManager(string id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManagerExists(string id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }
        #endregion

        // Employee
        #region Action On Employee By Manager
        [HttpGet("[controller]/{id}/Employes")]
        public async Task<ActionResult<List<Employee>>> GetEmployeeByManager(string id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            var myEmployees = _context.Employees.Where(emp => emp.MyManager == manager).ToList();

            return myEmployees;
        }

        // test : https://localhost:44310/api/managers/40ea8fd6-d319-4047-bf5f-2502ed9f91a9/employes/972d4067-24f3-45ef-ad26-4b418017dea2
        [HttpGet("[controller]/{id}/Employes/{id2}")]
        public async Task<ActionResult<Employee>> GetEmployeeByManager(string id, string id2)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            var myEmployees = _context.Employees.Where(emp => emp.MyManager == manager).ToList();//.Where(e => e.Id == id2).First();
            foreach (var employee in myEmployees)
            {
                if (employee.Id == id2)
                {
                    return employee;
                }
            }
            return NotFound();
        }

        [HttpGet("Employes/{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(string id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }



        [HttpPut("[controller]/{id}/Employes/{id2}")]
        public async Task<IActionResult> PutEmployeeByManager(string id, string id2, Employee employee)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            if (id2 == employee.Id && id == employee.MyManager.Id)
            {
                _context.Entry(employee).State = EntityState.Modified;
            }
            else
            {
                return BadRequest();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id2))
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


        [HttpPost("[controller]/{id}/Employes")]
        public async Task<ActionResult<Employee>> PostEmployeeByManager(string id, Employee employee)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            employee.MyManager = manager;
            await _context.Employees.AddAsync(employee);


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeByManager", new { id = manager.Id, id2 = employee.Id }, employee);
        }





        [HttpDelete("[controller]/{id}/Employes/{id2}")]
        public async Task<ActionResult<Manager>> DeleteEmployeeByManager(string id, string id2)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            var myEmployees = _context.Employees.Where(emp => emp.MyManager == manager).ToList();//.Where(e => e.Id == id2).First();
            foreach (var emp in myEmployees)
            {
                if (emp.Id == id2)
                {
                    _context.Employees.Remove(emp);
                }
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

        #endregion

        //Clients
        #region Action on Client By Managers and Employees
        [HttpGet("Employees/{id2}/Clients")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients")]
        public async Task<ActionResult<List<Client>>> GetClientsByEmployeeByManager(string id = null, string id2 = null)
        {
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null)
                {
                    return NotFound();
                }

                var myEmployees = _context.Employees.Where(emp => emp.MyManager == manager).ToList();
                List<Client> Clients = new List<Client>();
                foreach (var employee in myEmployees)
                {
                    if (employee.Id == id2)
                    {
                        Clients.AddRange(_context.Clients.Where(cl => cl.MyEmployee == employee).ToList());
                    }
                }
                return Clients;
            }
            var employe = await _context.Employees.FindAsync(id2);
            if (employe == null)
            {
                return NotFound();
            }
            return _context.Clients.Where(cl => cl.MyEmployee == employe).ToList();
        }


        [HttpGet("Employees/{id2}/Clients/{id3}")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients/{id3}")]
        public async Task<ActionResult<Client>> GetClientsByEmployeeByManager(string id = null, string id2 = null, string id3 = null)
        {
            Client client = _context.Clients.Find(id3);
            var employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee ==null && client == null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == id2)
                    {
                        return client;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            
            if (employee == null && client == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == id2)
            {
                return client;
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("Employees/{id2}/Clients/{id3}")]
        [HttpPut("[controller]/{id}/Employes/{id2}/Clients/{id3}")]
        public async Task<ActionResult<Client>> PutClientsByEmployeeByManager(Client client, string id = null, string id2 = null, string id3 = null)
        {
            Client clt = new Client();
            var employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null)
                {
                    return NotFound();
                }

                if(employee.MyManager.Id== manager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        _context.Entry(client).State = EntityState.Modified;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            
            if (employee == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == employee.Id)
            {
                _context.Entry(client).State = EntityState.Modified;
            }
            else
            {
                return BadRequest();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientsExists(id3))
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


        [HttpPost("Employees/{id2}/Clients")]
        [HttpPost("[controller]/{id}/Employes/{id2}/Clients")]
        public async Task<ActionResult<Client>> PostClientsByEmployeeByManager(Client client, string id = null, string id2 = null)
        {
            Employee employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    client.MyEmployee = employee;
                    _context.Entry(client).State = EntityState.Modified;
                }
                else
                {
                    return BadRequest();
                }

            }

            if(employee==null)
            {
                return NotFound();
            }
            client.MyEmployee = employee;
            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientsExists(client.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("Employees/{id2}/Clients/{id3}")]
        [HttpDelete("[controller]/{id}/Employes/{id2}/Clients/{id3}")]
        public async Task<ActionResult<Manager>> DeleteClientsByEmployeeByManager(string id = null, string id2 =null, string id3=null)
        {
            Client client;
            Employee employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null)
                {
                    return NotFound();
                }

                if (manager.Id == employee.MyManager.Id)
                {
                    client = await _context.Clients.FindAsync(id3);
                    if(client.MyEmployee.Id==employee.Id)
                    {
                        _context.Clients.Remove(client);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            if (employee == null)
            {
                return NotFound();
            }
            client = await _context.Clients.FindAsync(id3);
            if (client.MyEmployee.Id == employee.Id)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientsExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
        #endregion


        //Account
        #region Action on Account of Client By Managers and Employees
        [HttpGet("Employees/{id2}/Clients/{id3}/Account")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account")]
        public async Task<ActionResult<List<Account>>> GetAccountByClient(string id = null, string id2 = null, string id3 = null)
        {
            List<Account> accounts = new List<Account>();
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null)
                {
                    return NotFound();
                }

                var myEmployees = _context.Employees.Where(emp => emp.MyManager == manager).ToList();
                
                foreach (var employee in myEmployees)
                {
                    if (employee.Id == id2)
                    {
                        foreach (var client in _context.Clients.Where(cl => cl.MyEmployee == employee).ToList())
                        {
                            if (client.Id == id3)
                            {
                                accounts.AddRange(_context.Accounts.Where(ac => ac.AccountOwner == client).ToList());
                            }
                        }
                    }
                }
            }
            var employe = await _context.Employees.FindAsync(id2);
            if (employe == null)
            {
                return NotFound();
            }
            foreach (var client in _context.Clients.Where(cl => cl.MyEmployee == employe).ToList())
            {
                if (client.Id == id3)
                {
                    accounts.AddRange(_context.Accounts.Where(ac => ac.AccountOwner == client).ToList());
                }
            }

            return accounts;
        }

        /*
        [HttpGet("Employees/{id2}/Clients/{id3}")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients/{id3}")]
        public async Task<ActionResult<Client>> GetClientsByEmployeeByManager(string id = null, string id2 = null, string id3 = null)
        {
            Client client = _context.Clients.Find(id3);
            var employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == id2)
                    {
                        return client;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }

            if (employee == null && client == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == id2)
            {
                return client;
            }
            else
            {
                return BadRequest();
            }
        }

        */
        [HttpPut("Employees/{id2}/Clients/{id3}/Account/{id4}")]
        [HttpPut("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}")]
        public async Task<ActionResult<Client>> PutAccountOfClientsByEmployeeByManager(Account account, int id4, string id = null, string id2 = null, string id3 = null )
        {
            var client = await _context.Clients.FindAsync(id3);
            var employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client==null)
                {
                    return NotFound();
                }

                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        if(account.AccountOwner.Id== client.Id)
                        {
                            _context.Entry(account).State = EntityState.Modified;
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            if (employee == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == employee.Id)
            {
                if (account.AccountOwner.Id == client.Id)
                {
                    _context.Entry(account).State = EntityState.Modified;
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id4))
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


        [HttpPost("Employees/{id2}/Clients/{id3}/Account")]
        [HttpPost("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account")]
        public async Task<ActionResult<Account>> PostAccountOfClientsByEmployeeByManager(Account account, string id = null, string id2 = null, string id3 = null)
        {
            Client client = await _context.Clients.FindAsync(id3);
            Employee employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    if(employee.Id==client.MyEmployee.Id)
                    {
                        if(client.Id==account.AccountOwner.Id)
                        {
                            if(account is Saving)
                            {
                                _context.Entry((Saving)account).State = EntityState.Modified;
                            }
                            else
                            {
                                if (account is Deposit)
                                {
                                    _context.Entry((Deposit)account).State = EntityState.Modified;
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }

            }

            if (employee == null && client == null)
            {
                return NotFound();
            }
            if (employee.Id == client.MyEmployee.Id)
            {
                if (client.Id == account.AccountOwner.Id)
                {
                    if (account is Saving)
                    {
                        _context.Entry((Saving)account).State = EntityState.Modified;
                    }
                    else
                    {
                        if (account is Deposit)
                        {
                            _context.Entry((Deposit)account).State = EntityState.Modified;
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.AccountId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("Employees/{id2}/Clients/{id3}/Account/{id4}")]
        [HttpDelete("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}")]
        public async Task<ActionResult<Manager>> DeleteAccountOfClientsByEmployeeByManager(int id4, string id= null, string id2=null, string id3=null)
        {
            Account account = await _context.Accounts.FindAsync(id4);
            Client client = await _context.Clients.FindAsync(id3);
            Employee employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client==null)
                {
                    return NotFound();
                }

                if (manager.Id == employee.MyManager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        if (client.Id == account.AccountOwner.Id)
                        {
                            if(account is Saving)
                            {
                                _context.Savings.Remove((Saving)account);
                            }
                            else
                            {
                                if (account is Deposit)
                                {
                                    _context.Deposits.Remove((Deposit)account);
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }

                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            if (employee == null && client == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == employee.Id)
            {
                if (client.Id == account.AccountOwner.Id)
                {
                    if (account is Saving)
                    {
                        _context.Savings.Remove((Saving)account);
                    }
                    else
                    {
                        if (account is Deposit)
                        {
                            _context.Deposits.Remove((Deposit)account);
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                return BadRequest();
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
        #endregion

        // Cards

        #region Action on Account of Client By Managers and Employees
        [HttpGet("Employees/{id2}/Clients/{id3}/Account/{id4}/Cards")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}/Cards")]
        public async Task<ActionResult<List<Card>>> GetCardByAccountByClient(int id4, string id = null, string id2 = null, string id3 = null)
        {
            List<Card> cards = new List<Card>();
            var employee = await _context.Employees.FindAsync(id2);
            var client = await _context.Clients.FindAsync(id3);
            var account = await _context.Accounts.FindAsync(id4);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null && account == null)
                {
                    return NotFound();
                }

                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        if (account.AccountOwner.Id == client.Id)
                        {
                            if (account is Deposit)
                            {
                                cards.AddRange(_context.Cards.Where(c => c.CardDeposit.AccountId == account.AccountId).ToList());
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                if (employee == null)
                {
                    return NotFound();
                }
                if (client.MyEmployee.Id == employee.Id)
                {
                    if (account.AccountOwner.Id == client.Id)
                    {
                        if (account is Deposit)
                        {
                            cards.AddRange(_context.Cards.Where(c => c.CardDeposit.AccountId == account.AccountId).ToList());
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            return cards;
        }

        /*
        [HttpGet("Employees/{id2}/Clients/{id3}")]
        [HttpGet("[controller]/{id}/Employes/{id2}/Clients/{id3}")]
        public async Task<ActionResult<Client>> GetClientsByEmployeeByManager(string id = null, string id2 = null, string id3 = null)
        {
            Client client = _context.Clients.Find(id3);
            var employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == id2)
                    {
                        return client;
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }

            if (employee == null && client == null)
            {
                return NotFound();
            }
            if (client.MyEmployee.Id == id2)
            {
                return client;
            }
            else
            {
                return BadRequest();
            }
        }

        */
        [HttpPut("Employees/{id2}/Clients/{id3}/Account/{id4}/Cards/{id5}")]
        [HttpPut("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}/Cards/{id5}")]
        public async Task<ActionResult<Card>> PutCardsByAccountOfClientsByEmployeeByManager(Card card, int id4, int id5, string id = null, string id2 = null, string id3 = null)
        {
            var client = await _context.Clients.FindAsync(id3);
            var employee = await _context.Employees.FindAsync(id2);
            var account = await _context.Accounts.FindAsync(id4);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null && account==null)
                {
                    return NotFound();
                }

                if (employee.MyManager.Id == manager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        if (account.AccountOwner.Id == client.Id)
                        {
                            if(account is Deposit)
                            {
                                if((Deposit)account == card.CardDeposit)
                                {
                                    _context.Entry(card).State = EntityState.Modified;
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                if (employee == null && client == null)
                {
                    return NotFound();
                }
                if (client.MyEmployee.Id == employee.Id)
                {
                    if (account.AccountOwner.Id == client.Id)
                    {
                        if (account is Deposit)
                        {
                            if ((Deposit)account == card.CardDeposit)
                            {
                                _context.Entry(card).State = EntityState.Modified;
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id5))
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


        [HttpPost("Employees/{id2}/Clients/{id3}/Account/{id4}/Cards")]
        [HttpPost("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}/Cards")]
        public async Task<ActionResult<Card>> PostCardByAccountOfClientsByEmployeeByManager(Card card, int id4, string id = null, string id2 = null, string id3 = null)
        {
            Client client = await _context.Clients.FindAsync(id3);
            Employee employee = await _context.Employees.FindAsync(id2);
            Account account = await _context.Accounts.FindAsync(id4);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null && account==null)
                {
                    return NotFound();
                }
                if (employee.MyManager.Id == manager.Id)
                {
                    if (employee.Id == client.MyEmployee.Id)
                    {
                        if (client.Id == account.AccountOwner.Id)
                        {
                            if (account is Deposit)
                            {
                                if(((Deposit)account).AccountId==card.CardDeposit.AccountId)
                                {
                                    _context.Entry(card).State = EntityState.Modified;
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            else
            {
                if(employee == null && client == null && account == null)
                {
                    return NotFound();
                }
                if (employee.Id == client.MyEmployee.Id)
                {
                    if (client.Id == account.AccountOwner.Id)
                    {
                        if (account is Deposit)
                        {
                            if (((Deposit)account).AccountId == card.CardDeposit.AccountId)
                            {
                                _context.Entry(card).State = EntityState.Modified;
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(card.CardId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("Employees/{id2}/Clients/{id3}/Account/{id4}/Cards/{id5}")]
        [HttpDelete("[controller]/{id}/Employes/{id2}/Clients/{id3}/Account/{id4}/Cards/{id5}")]
        public async Task<ActionResult<Manager>> DeleteCardByAccountOfClientsByEmployeeByManager(int id4, int id5, string id = null, string id2 = null, string id3 = null)
        {
            Card card = await _context.Cards.FindAsync(id5);
            Account account = await _context.Accounts.FindAsync(id4);
            Client client = await _context.Clients.FindAsync(id3);
            Employee employee = await _context.Employees.FindAsync(id2);
            if (id != null)
            {
                var manager = await _context.Managers.FindAsync(id);
                if (manager == null && employee == null && client == null && account == null && account == null) 
                {
                    return NotFound();
                }

                if (manager.Id == employee.MyManager.Id)
                {
                    if (client.MyEmployee.Id == employee.Id)
                    {
                        if (client.Id == account.AccountOwner.Id)
                        {
                            if (account is Deposit)
                            {
                                if (((Deposit)account).AccountId == card.CardDeposit.AccountId)
                                {
                                    _context.Cards.Remove(card);
                                }
                                else
                                {
                                    return BadRequest();
                                }
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                if (employee == null && client == null && account == null && account == null)
                {
                    return NotFound();
                }
                if (client.MyEmployee.Id == employee.Id)
                {
                    if (client.Id == account.AccountOwner.Id)
                    {
                        if (account is Deposit)
                        {
                            if (((Deposit)account).AccountId == card.CardDeposit.AccountId)
                            {
                                _context.Cards.Remove(card);
                            }
                            else
                            {
                                return BadRequest();
                            }
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
        #endregion

    }
}
