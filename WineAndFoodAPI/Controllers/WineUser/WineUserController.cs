using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WineAndFoodAPI.Controllers.WineUser
{
    [Route("api/[controller]")]
    [ApiController]
    public class WineUserController : ControllerBase
    {
        private readonly IWineUserService _wineUserService;
        private readonly IRatingService _ratingService;

        public WineUserController (IWineUserService wineUserService, IRatingService ratingService)
        {
            _wineUserService = wineUserService;
            _ratingService = ratingService;
        }
    }
}
