using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Enums.Wines.Criteria
{

        public class WineSearchCriteria
        {
            public string? Winery { get; set; }
            public Guid? GrapeId { get; set; }
            public decimal? MinPrice { get; set; }
            public decimal? MaxPrice { get; set; }
            public string? Region { get; set; }

            public bool OnlyInStock { get; set; } = true;

            public bool HasPriceRange => MinPrice.HasValue || MaxPrice.HasValue;
        }
}
