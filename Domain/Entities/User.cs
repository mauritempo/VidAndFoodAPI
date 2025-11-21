using Domain.common;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string? FullName { get; set; }
        public  Role RoleUser { get; set; } = Role.User;  

        public virtual ICollection<WineUser> WineUsers { get; set; } = new List<WineUser>();
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<WineFavorite> Favorites { get; set; } = new List<WineFavorite>();
    
}
}
