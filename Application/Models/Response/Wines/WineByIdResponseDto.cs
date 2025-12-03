using Application.Models.Response.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.Wines
{
    public class WineByIdResponseDto
    {
        public WineDetailDto Wine { get; set; }
        public List<WineReviewDto> Reviews { get; set; }
    }
}
