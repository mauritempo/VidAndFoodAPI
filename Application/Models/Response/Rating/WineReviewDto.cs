using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Rating
{
    public class WineReviewDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty; 
        public int Score { get; set; }
        public string? Review { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId { get; set; }
        public bool IsSommelierReview { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
