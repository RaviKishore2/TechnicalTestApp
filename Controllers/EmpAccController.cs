using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechTestApp.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TechTestApp.Controllers;
using Microsoft.EntityFrameworkCore;


namespace TechTestApp.Controllers
{
    public class EmpAccController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEncryptionService _encryptionService;
        private readonly EmployeeDbContext _dbContext;
        //private readonly TwoFactorAuthenticationController _twoFactorAuthenticationController;


        public EmpAccController(UserManager<Employee> userManager, SignInManager<Employee> signInManager, IConfiguration configuration, IEncryptionService encryptionService, EmployeeDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _encryptionService = encryptionService;
            _dbContext = dbContext;

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var encryptPwd = _encryptionService.Encrypt(model.Password);

                var emp = new Employee
                {
                    EmpName = model.Email,
                    Email = model.Email,
                    UserName = model.Email,
                    Password = encryptPwd,
                    TwoFactorAuthCode = "123456",
                    IsTwoFactorAuthEnabled = false,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 5,
                };
                _dbContext.Users.Add(emp);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Login", "EmpAcc");

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {

                    return RedirectToAction("EnableTwoFactorAuthentication", "TwoFactorAuthenticationController");

                }
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            return View("Login");
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var employees = _dbContext.Employees.ToList();
            List<EmployeeViewModel> employeesList = new List<EmployeeViewModel>();
            if(employees!= null)
            {
                foreach (var employee in employees)
                {
                    var EmployeeViewModel = new EmployeeViewModel()
                    {
                        EmpName = employee.EmpName,
                        Email = employee.Email,

                    };
                    employeesList.Add(EmployeeViewModel);
                }
                return View(employeesList);
            }
            return View(employeesList);
        }

        public async Task<IActionResult> EditEmp(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emp = await _userManager.FindByIdAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteEmp(string id)
        {
            var emp = await _userManager.FindByIdAsync(id);
            if (emp == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(emp);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Dashboard));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(emp);
        }

        private bool UserExists(string id)
        {
            return _userManager.FindByIdAsync(id) != null;
        }
    }
}

