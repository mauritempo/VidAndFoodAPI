using Application.Models.Response.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.User
{
        public class AuthResponseDto
        {
            public string Token { get; set; }
            //public string RefreshToken { get; set; }
            //public DateTime ExpiresAt { get; set; }
            //public UserDto User { get; set; } = null!;
        }
}

