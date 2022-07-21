using System.Collections.Generic;
using System.Linq;

namespace MovieStreamer.Models
{
    public class LoginOperation
    {
        public bool Succeeded { get; set; }
        public string? errors { get; set; }
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public static LoginOperation Success(string? id, string? email, string? phone)
        {
            var result = new LoginOperation
            { 
                Succeeded = true,
                Id = id,
                Email = email,
                Phone = phone
            };
            return result;
        }
        public static LoginOperation Failed(string err = null)
        {
            var result = new LoginOperation { Succeeded = false };
            if (err != null)
            {
                result.errors = err;
            }
            return result;
        }

    }
    public class OperationResult
    {
        public bool Succeeded { get; set; }
        public string? errors { get; set; }

        public static OperationResult Success()
        {
            var result = new OperationResult { Succeeded = true };
            return result;
        }
        public static OperationResult Failed(string err = null)
        {
            var result = new OperationResult { Succeeded = false };
            if (err != null)
            {
                result.errors = err;
            }
            return result;
        }

    }
}
