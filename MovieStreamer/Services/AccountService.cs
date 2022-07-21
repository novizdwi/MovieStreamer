using MovieStreamer.Models;
using MovieStreamer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MovieStreamer.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext db;
        public AccountService(ApplicationDbContext dbContexts)
        {
            db = dbContexts;
        }

        public int CekExist(RegisterViewModel vm)
        {
            var rec = db.Users.FirstOrDefault(x => x.Email  == vm.Email || x.Phone == vm.Phone);
            return rec!=null ? 1 : 0;
        }
        public Users Login(LoginViewModel vm)
        {
            string passwordHash = sha256_hash(vm.Password);
            var rec = db.Users.FirstOrDefault(x => (x.Email == vm.UserName || x.Phone == vm.UserName) && x.Password == passwordHash);
            return rec;
        }

        public async Task<LoginOperation> Register(RegisterViewModel vm)
        {
            try
            {
                using (var scope = new TransactionScope(
                   TransactionScopeOption.Required,
                   TimeSpan.FromMinutes(60),
                   TransactionScopeAsyncFlowOption.Enabled
                   )) 
                {
                    var data = new Users()
                    {
                        Email= vm.Email,
                        Phone= vm.Phone,
                        Password= sha256_hash(vm.Password),
                        CreatedBy = "SYSTEM",
                        CreatedDate = DateTime.Now
                    };

                    db.Users.Add(data);
                    var success = await db.SaveChangesAsync() > 0;

                    if (success)
                    {
                        scope.Complete();
                        return LoginOperation.Success(Convert.ToString(data.Id), data.Email, data.Phone);
                    }
                    return LoginOperation.Failed();
                }
            }
            catch (Exception ex) {
                
                return LoginOperation.Failed(ex.ToString());
            }
        }

        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }


    }
}
