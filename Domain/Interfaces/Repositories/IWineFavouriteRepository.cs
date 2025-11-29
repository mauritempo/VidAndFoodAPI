using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface IWineFavouriteRepository : IBaseRepository<WineFavorite>
    {
        Task<(List<WineFavorite> Items, int TotalCount)> GetPagedFavouriteAsync(Guid userId, int page, int pageSize);
        Task<WineFavorite?> GetByPairAsync(Guid userId, Guid wineId);
        Task<bool> IsFavoriteAsync(Guid userId, Guid wineId);
        Task DeleteAsync(WineFavorite favorite);
    }
}
