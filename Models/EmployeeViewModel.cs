﻿namespace TechTestApp.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string TwoFactorAuthCode { get; set; }
        public bool IsTwoFactorAuthEnabled { get; set; }
        public string EmpName { get; internal set; }
    }
}
