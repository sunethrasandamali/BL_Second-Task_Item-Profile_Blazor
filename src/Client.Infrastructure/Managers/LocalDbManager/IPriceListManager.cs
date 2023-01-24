using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Blazor.IndexedDB.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.LocalDbManager
{
    public interface IPriceListManager:IManager
    {
        Task<IndexedSet<PriceListResponse>> GetAllPriceListAsync();

        Task<PriceListResponse> GetItemByPriceListAsync(string itm_code);

        Task<PriceListResponse[]> FindByItemCode(string searchTerm);

        Task OpenDbAsync(IList<PriceListResponse> price_list_response);
    }
}
