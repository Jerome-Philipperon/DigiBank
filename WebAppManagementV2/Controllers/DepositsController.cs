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
    public class DepositsController : Controller
    {
        private readonly BankContext _context;

        public DepositsController(BankContext context)
        {
            _context = context;
        }

        // GET: Deposits
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deposits.ToListAsync());
        }

        // GET: Deposits/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Deposits
                .Include("DepositCards")
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Deposits/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,BankCode,BranchCode,AccountNumber,Key,BBAN,IBAN,BIC,Balance,CreationDate,AutorizedOverdraft,OverdraftChargeRate")] Deposit account)
        {
            ModelState.Remove("AccountOwner");
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }

        // GET: Deposits/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Deposits.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Deposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,BankCode,BranchCode,AccountNumber,Key,BBAN,IBAN,BIC,Balance,CreationDate,AutorizedOverdraft,OverdraftChargeRate")] Deposit account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }
            ModelState.Remove("AccountOwner");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
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
            return View(account);
        }

        // GET: Deposits/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Deposits
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Deposits/Delete/5
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Deposits.FindAsync(id);
            _context.Deposits.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Deposits.Any(e => e.AccountId == id);
        }

        // GET: Cards/Create
        [Authorize(Roles = "Manager")]
        public IActionResult CreateNewCard(int? id)
        {
            //var account = _context.Deposits.Find(id);
            //ViewBag.idaccount = id;
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNewCard([Bind("CardId,NetworkIssuer,CardNumber,SecurityCode,ExpirationDate")] Card card, int? id)
        {
            var account = await _context.Deposits
               .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Cards.Add(card);
                if (account.DepositCards==null)
                {
                    account.DepositCards = new List<Card>();
                }
                account.DepositCards.Add(card);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Deposits", new { id = account.AccountId });//TODO : Référence au Deposit en cours pour que la redirection fonctionne correctement
            }
            //
            return View(card);
        }
    }
}
