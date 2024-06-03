using System.Text;
using Recallio.Const.Enum;
using Recallio.Const.Errors;
using Recallio.Domain.Models.User;
using Recallio.Kernel.Exceptions;
using Recallio.Kernel.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Recallio.Auth;

    public class RecallioPasswordHasher : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            user.Salt = AppSecurity.GenerateSalt();
            string passwordHash = null;

            try
            {
                byte[] saltBytes = Convert.FromBase64String(user.Salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] saltPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

                passwordBytes.CopyTo(saltPasswordBytes, 0);
                saltBytes.CopyTo(saltPasswordBytes, passwordBytes.Length);

                passwordHash = AppSecurity.GetSHA512(saltPasswordBytes);
            }
            catch (Exception e)
            {
                throw new LoggerException($"{ErrorMessages.HashGenerationError}: [{e.GetError()}]", 500, null, EntityTypeEnum.Session.ToString());
            }

            if (passwordHash == null)
            {
                throw new LoggerException(ErrorMessages.HashGenerationError, 500, null, EntityTypeEnum.Session.ToString());
            }

            return passwordHash;

        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (string.IsNullOrWhiteSpace(providedPassword))
            {
                return PasswordVerificationResult.Failed;
            }
            
            string passwordHash = null;

            try
            {
                byte[] saltBytes = Convert.FromBase64String(user.Salt);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(providedPassword);

                byte[] saltPasswordBytes = new byte[saltBytes.Length + passwordBytes.Length];

                passwordBytes.CopyTo(saltPasswordBytes, 0);
                saltBytes.CopyTo(saltPasswordBytes, passwordBytes.Length);

                passwordHash = AppSecurity.GetSHA512(saltPasswordBytes);
            }
            catch
            {
                return PasswordVerificationResult.Failed;
            }

            if (passwordHash == null)
            {
                return PasswordVerificationResult.Failed;
            }

            if (passwordHash != hashedPassword)
            {
                return PasswordVerificationResult.Failed;
            }

            return PasswordVerificationResult.Success;
        }
    }