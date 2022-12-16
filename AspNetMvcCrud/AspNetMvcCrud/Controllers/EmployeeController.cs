using AspNetMvcCrud.Data;
using AspNetMvcCrud.Models;
using AspNetMvcCrud.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;


namespace AspNetMvcCrud.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;

        public EmployeeController(MVCDemoDbContext mvcDemoDbContext) 
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Birth = addEmployeeRequest.Birth,
                Department = addEmployeeRequest.Department
            };
            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            
            if(employee != null) 
            {
                   var viewModel = new UpDateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Birth = employee.Birth,
                    Department = employee.Department
                }; 
                return await Task.Run(() => View("View",viewModel));
            }
            
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpDateEmployeeViewModel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if(employee != null) 
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.Birth = model.Birth;
                employee.Department = model.Department;
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpDateEmployeeViewModel model) 
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if(employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
