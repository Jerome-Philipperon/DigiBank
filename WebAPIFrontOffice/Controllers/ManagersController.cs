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
    [Route("api/[controller]")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly BankContext _context;

        public ManagersController(BankContext context)
        {
            _context = context;
        }

        // GET: api/Managers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetManagers()
        {
            return await _context.Managers.ToListAsync();
        }

        // GET: api/Managers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Manager>> GetManager(string id)
        {
            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            return manager;
        }

        [HttpGet("{id}/Employes")]
        public async Task<ActionResult<List<Employee>>>  GetEmployeeByManager(string id)
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
        [HttpGet("{id}/Employes/{id2}")]
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
                if (employee.Id==id2)
                {
                    return employee;
                }
            }
            return NotFound();
        }

        // PUT: api/Managers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
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


        [HttpPut("{id}/Employes/{id2}")]
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

        // POST: api/Managers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
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


        [HttpPost("{id}/Employes")]
        public async Task<ActionResult<Manager>> PostEmployeeByManager(string id, Employee employee)
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
                if (EmployeeExists(manager.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetEmployeeByManager", new { id = manager.Id , id2 = employee.Id}, employee);
        }



        // DELETE: api/Managers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Manager>> DeleteManager(string id)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();

            return manager;
        }

        [HttpDelete("{id}/Employes/{id2}")]
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

            return manager;
        }

        private bool ManagerExists(string id)
        {
            return _context.Managers.Any(e => e.Id == id);
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

    }
}
