using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStreamer.Models;
using MovieStreamer.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace MovieStreamer.Services
{
    public class SubscriptionService
    {
        private readonly ApplicationDbContext db;
        public SubscriptionService(ApplicationDbContext dbContexts)
        {
            db = dbContexts;
        }

        public List<SelectListItem> GetSubscriptionType()
        {
            var data = typeof(SubscriptionType)
              .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
              .Where(x => x.IsLiteral && !x.IsInitOnly) // constants, not readonly
              .Where(x => x.FieldType == typeof(string)) // of type string
              .Select(x => new SelectListItem
              {
                  Value = x.Name,
                  Text = x.GetValue(null) as string
              }).ToList();

            return data.ToList();
        }

        public List<SelectListItem> GetPaymentMethod()
        {
            var data = typeof(PaymentMethod)
              .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
              .Where(x => x.IsLiteral && !x.IsInitOnly) // constants, not readonly
              .Where(x => x.FieldType == typeof(string)) // of type string
              .Select(x => new SelectListItem
              {
                  Value = x.Name,
                  Text = x.GetValue(null) as string
              }).ToList();

                        return data.ToList();
        }

        public int CountTotalPage(int recordPerPage, int userId)
        {
            var ret = db.Subcriptions.Where(x => x.UserId == userId).Count();
            return ret / recordPerPage;
        }

        public List<SubscriptionViewModel> GetAll(int userId, int page, int recordPerPage)
        {
            List<SelectListItem> subscriptionType = GetPaymentMethod();
            List<SelectListItem> paymentMethod = GetPaymentMethod();
            var ret = (from s in db.Subcriptions.Where(x => x.UserId == userId)
                       orderby s.CreatedDate descending
                       select new SubscriptionViewModel()
                       {
                           Id = s.Id,
                           SubscriptionType = s.SubscriptionType,
                           //SubscriptionTypeText = subscriptionType.Where(x => x.Value == s.SubscriptionType).Select(x => x.Text).FirstOrDefault(),
                           SubscriptionDate = s.SubscriptionDate,
                           PaymentMethod = s.PaymentMethod,
                           //PaymentMethodText = paymentMethod.Where(x => s.PaymentMethod.Contains(x.Value)).Select(x => x.Text).FirstOrDefault(),
                           Price = s.Price,
                           ExpiredDate = s.ExpiredDate,
                       }).ToList();
            ret.Skip((page - 1) * recordPerPage).Take(recordPerPage);
            return ret;

        }

        public async Task<OperationResult> Subscribe(SubscriptionViewModel vm)
        {
            try
            {
                using (var scope = new TransactionScope(
                   TransactionScopeOption.Required,
                   TimeSpan.FromMinutes(60),
                   TransactionScopeAsyncFlowOption.Enabled
                   ))
                {
                    var data = new Subscription()
                    {
                        UserId = vm.UserId,
                        SubscriptionType = vm.SubscriptionType,
                        SubscriptionDate = DateTime.Now,
                        PaymentMethod = vm.PaymentMethod,
                        Price = vm.Price,
                        ExpiredDate = vm.ExpiredDate,
                        CreatedDate = DateTime.Now,
                        CreatedBy = vm.UserId.ToString()
                    };
                    
                    db.Subcriptions.Add(data);
                    var success = await db.SaveChangesAsync() > 0;
                    if (success)
                    {
                        scope.Complete();
                        return OperationResult.Success();
                    }
                    return OperationResult.Failed();

                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failed(ex.ToString());
            }
        }
    }
}
