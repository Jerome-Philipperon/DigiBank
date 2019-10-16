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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace WebAppManagement.Controllers
{
    public class SavingsController : Controller
    {
        private readonly BankContext _context;

        public SavingsController(BankContext context)
        {
            _context = context;
        }

        // GET: Savings
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Index()
        {
            List<Saving> savings = new List<Saving>();
            Employee emp = await _context.Employees
                .Include("MyManager")
                .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);
            if (emp is Manager)
            {
                Manager man = await _context.Managers
                .SingleOrDefaultAsync(e => e.UserName == User.Identity.Name);
                savings = await _context.Savings
                .Where(s => s.AccountOwner.MyEmployee.MyManager.Id == man.Id).ToListAsync();
            }
            else
            {
                savings = await _context.Savings
                .Where(s => s.AccountOwner.MyEmployee.Id == emp.Id).ToListAsync();
            }
            return View(savings);
        }

        // GET: Savings/Details/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Savings
                .Include("AccountOwner")
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Savings/Create
        [Authorize(Roles = "Manager, Employee")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Savings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,BankCode,BranchCode,AccountNumber,Key,BBAN,IBAN,BIC,Balance,MinimumAmount,InterestRate")] Saving account)
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

        // GET: Savings/Edit/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Savings.Include(a => a.AccountOwner).SingleOrDefaultAsync(a => a.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Savings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,BankCode,BranchCode,AccountNumber,Key,BBAN,IBAN,BIC,Balance,MinimumAmount,InterestRate")] Saving account)
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

        // GET: Savings/Delete/5
        [Authorize(Roles = "Manager, Employee")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Savings
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Savings/Delete/5
        [Authorize(Roles = "Manager, Employee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Savings.FindAsync(id);
            _context.Savings.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Savings.Any(e => e.AccountId == id);
        }
    }
}
