using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using DomainModel;
using Microsoft.AspNetCore.Authorization;

namespace WebAppManagement.Controllers
{
    public class CardsController : Controller
    {
        private readonly BankContext _context;

        public CardsController(BankContext context)
        {
            _context = context;
        }

        // GET: Cards
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Index()
        {
            List<Card> cards = new List<Card>();
            Employee emp = await _context.Employees
                .Include("MyManager")
                .SingleOrDefaultAsync(e => e.Email == User.Identity.Name);
            if (emp is Manager)
            {
                Manager man = await _context.Managers
                .SingleOrDefaultAsync(e => e.Email == User.Identity.Name);
                cards = await _context.Cards
                .Where(c => c.CardDeposit.AccountOwner.MyEmployee.MyManager.Id == man.Id).ToListAsync();
            }
            else
            {
                cards = await _context.Cards
                .Where(c => c.CardDeposit.AccountOwner.MyEmployee.Id == emp.Id).ToListAsync();
            }
            return View(cards);
        }

        // GET: Cards/Details/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .Include("CardDeposit")
                .Include("CardDeposit.AccountOwner")
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // GET: Cards/Create
        [Authorize(Roles = "Manager, Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CardId,NetworkIssuer,CardNumber,SecurityCode,ExpirationDate")] Card card)
        {
            if (ModelState.IsValid)
            {
                _context.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction($"Details/{card.CardDeposit.AccountId}", "Deposits"); //TODO : Référence au Deposit en cours pour que la redirection fonctionne correctement
            }
            return View(card);
        }

        // GET: Cards/Edit/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CardId,NetworkIssuer,CardNumber,SecurityCode,ExpirationDate")] Card card)
        {
            if (id != card.CardId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(card);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(card.CardId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(card);
        }

        // GET: Cards/Delete/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var card = await _context.Cards
                .FirstOrDefaultAsync(m => m.CardId == id);
            if (card == null)
            {
                return NotFound();
            }

            return View(card);
        }

        // POST: Cards/Delete/5
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.Cards.FindAsync(id);
            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.CardId == id);
        }
    }
}
