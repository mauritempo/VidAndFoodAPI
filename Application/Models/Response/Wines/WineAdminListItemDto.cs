using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Wines
{
    public class WineAdminListItemDto : WineListItemDto
    {
        public bool IsActive { get; set; }
    }
}
