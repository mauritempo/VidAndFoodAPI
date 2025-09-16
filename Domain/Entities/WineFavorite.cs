﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WineFavorite
    {
        public int UserId { get; set; }
        public int WineId { get; set; }

        public User User { get; set; } = null!;
        public Wine Wine { get; set; } = null!;
    }
}
