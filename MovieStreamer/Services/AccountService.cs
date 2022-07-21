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
            IQueryable<Users> data = db.Users.AsQueryable();
            if (data.Any())
            {
                if (vm.Email != null)
                    data = data.Where(x => x.Email == vm.Email);
                else
                    data = data.Where(x => x.Phone == vm.Phone);
            }
            else
            {
                return 0;
            }
            return data.Count() == 0? 0 : 1;
        }
        public Users Login(LoginViewModel vm)
        {
            string passwordHash = sha256_hash(vm.Password);
            var rec = db.Users.FirstOrDefault(x => (x.Email == vm.UserName || x.Phone == vm.UserName) && x.Password == passwordHash);
            return rec;
        }

        public SettingViewModel GetOne(string? userId)
        {
            if (userId == null)
                return new SettingViewModel();

            var d = db.Users.FirstOrDefault(x => x.Id.ToString() == userId);
            if (d == null)
                return new SettingViewModel();
            return new SettingViewModel(){
                Username = d.Username,
                Email = d.Email,
                Phone = d.Phone,
                };
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

        public async Task<OperationResult> Settings(SettingViewModel viewModel)
        {
            try
            {
                using (var scope = new TransactionScope(
                   TransactionScopeOption.Required,
                   TimeSpan.FromMinutes(60),
                   TransactionScopeAsyncFlowOption.Enabled
                   )) 
                {
                    var data = db.Users.Find(Convert.ToInt32(viewModel.Id));
                    if (data != null)
                    {
                        data.Username = viewModel.Username;
                        data.Email = viewModel.Email;
                        data.Phone = viewModel.Phone;
                        if (viewModel.Password != null)
                            data.Password = sha256_hash(viewModel.Password);
                        data.ModifiedBy = "SYSTEM";
                        data.ModifiedDate = DateTime.Now;

                        var success = await db.SaveChangesAsync() > 0;
                        if (success)
                        {
                            scope.Complete();
                            return OperationResult.Success();
                        }
                    }
                    return OperationResult.Failed();
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.ToString());
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
