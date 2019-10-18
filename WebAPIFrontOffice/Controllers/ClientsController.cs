using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DomainModel;
using Microsoft.AspNetCore.Cors;

namespace WebAPIFrontOffice.Controllers
{
    [DisableCors]
    //[EnableCors("http://127.0.0.1:4200/")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly BankContext _context;

        public ClientsController(BankContext context)
        {
            _context = context;
        }
        
        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // GET: api/Clients/5
        [HttpGet("{lastName}/{firstName}")]
        public async Task<ActionResult<Client>> GetClientByName(string lastName, string firstName)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(c => c.LastName == lastName && c.FirstName == firstName);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }


        //Get api/Client/deposit                          c'est mo travail

        [HttpGet("{id}/Deposit")]
        public async Task<ActionResult<List<Deposit>>> GetClientByDeposit(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var DepositOfClient = _context.Deposits.Where(depo => depo.AccountOwner == client).ToList();
            if (DepositOfClient == null)
            {
                return NotFound();
            }
            return DepositOfClient;

        }


        //  Get api/Client/Saving
        [HttpGet("{id}/Saving")]
        public async Task<ActionResult<List<Saving>>> GetClientSaving (string id)
            {


            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var SavingByClient = _context.Savings.Where(sav => sav.AccountOwner == client).ToList();

            if (SavingByClient == null)
            {
                return NotFound();
            }

            return SavingByClient;
        }

        // Get api/Client/account

            [HttpGet ("{id}/Account")]

            public async Task<ActionResult<List<Account>>> GetClientByAccount (string id)
        {

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var AccountByClient = _context.Accounts.Where(acc => acc.AccountOwner == client).ToList();

            if (AccountByClient == null)
            {
                return NotFound();
            }
            return AccountByClient;

        }

        // Get api/Client/Card

        [HttpGet("{id}/Card")]
        public async Task<ActionResult<List<Card>>> GetClientByCard(string id)
        {

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var DepositOfClient = _context.Deposits.Where(depo => depo.AccountOwner == client).ToList();

            foreach (var deposit in DepositOfClient)
            {

              var ClientByCard= _context.Cards.Where(cards => cards.CardDeposit == deposit).ToList();
                if (ClientByCard == null)
                {
                    return NotFound();
                }
                return ClientByCard;
            }

            return NotFound();
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(string id, Client client)
        {
            if (id != client.Id)
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
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClientExists(client.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(string id)
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

        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
