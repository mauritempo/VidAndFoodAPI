using Application.Models.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        //public string RefreshToken { get; set; } = string.Empty;
        //public DateTime ExpiresAt { get; set; }

        //// Reutilizamos el UserDto que ya creamos antes
        //public UserDto User { get; set; } = null!;
    }
}
