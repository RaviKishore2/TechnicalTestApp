using Microsoft.AspNetCore.Mvc;
using TechTestApp.Models;

namespace TechTestApp.Controllers
{
    public class TwoFactorAuthenticationController : Controller
    {
        private readonly IEmailService _emailService;
        public TwoFactorAuthenticationController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        
        [HttpPost]
        public async Task<IActionResult> EnableTwoFactorAuthentication(string email)
        {
            RandomCodeGenerator randomCode = new RandomCodeGenerator();
            
            string code = randomCode.GenerateRandomCode(6);

            
            HttpContext.Session.SetString("TwoFactorCode", code);

            
            await _emailService.SendEmailAsync(email, "Two-Factor Authentication Code", $"Your authentication code is: {code}");

            
            return RedirectToAction("EnterTwoFactorCode");
        }

        
        [HttpGet]
        public string EnterTwoFactorCode()
        {
            return null;
        }

        [HttpPost]
        public IActionResult EnterTwoFactorCode(string code)
        {
            
            string storedCode = HttpContext.Session.GetString("TwoFactorCode");

            
            if (code == storedCode)
            {
                
                return RedirectToAction("Dashboard", "EmpAcc");
            }
            else
            {
                
                ModelState.AddModelError(string.Empty, "Invalid code. Please try again.");
                return View("~/Views/EmpAcc/Login.cshtml");
            }
        }

        
    }
}
