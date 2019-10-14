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
using WebAppManagementV2.Models;

namespace WebAppManagement.Controllers
{
    public class ClientsController : Controller
    {
        private readonly BankContext _context;
        
        public ClientsController(BankContext context)
        {
            _context = context;
        }

        // GET: Clients
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clients.ToListAsync());
        }

        // GET: Clients/Details/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Clients/Create
        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            CreateClientViewModel cC = new CreateClientViewModel();
            Employee emp = _context.Employees.SingleOrDefault(e => e.Email == this.User.Identity.Name);
            cC.Employees = new List<Employee>() { emp };
            if (emp is Employee)
            {
                cC.Employees.AddRange(_context.Employees.Where(e => e.MyManager.Id == emp.Id).ToList());
            }
            return View(cC);
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Client.PersonId,Client.FirstName,Client.LastName,Client.DateOfBirth,Client.Street,Client.ZipCode,City,Client.MyEmployee")] CreateClientViewModel person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person.Client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateClientViewModel cC = new CreateClientViewModel();
            cC.Client = await _context.Clients.FindAsync(id);

            if (cC.Client == null)
            {
                return NotFound();
            }

            Employee emp = await _context.Employees.SingleOrDefaultAsync(e => e.Email == this.User.Identity.Name);
            cC.Employees = new List<Employee>() { emp };
            if(emp is Employee)
            {
                cC.Employees.AddRange(_context.Employees.Where(e => e.MyManager.Id == emp.Id).ToList());
            }
            return View(cC);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Client.PersonId,Client.FirstName,Client.LastName,Client.DateOfBirth,Client.Street,Client.ZipCode,City,Client.MyEmployee")] CreateClientViewModel person)
        {
            if (id.ToString() != person.Client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Client.Id))
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
            return View(person);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Clients
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Clients/Delete/5
        [Authorize(Roles = "Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Clients.FindAsync(id);
            _context.Clients.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
