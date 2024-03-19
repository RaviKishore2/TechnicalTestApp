using Microsoft.AspNetCore.DataProtection;

namespace TechTestApp.Models
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;

        public EncryptionService(IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtectionProvider = dataProtectionProvider;
        }

        public string Encrypt(string plaintext)
        {
            var protector = _dataProtectionProvider.CreateProtector("Password");
            return protector.Protect(plaintext);
        }

        public string Decrypt(string ciphertext)
        {
            var protector = _dataProtectionProvider.CreateProtector("Password");
            return protector.Unprotect(ciphertext);
        }
    }
}
