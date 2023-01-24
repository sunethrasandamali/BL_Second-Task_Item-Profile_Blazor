using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using Blazor.IndexedDB.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;

namespace BlueLotus360.Com.Infrastructure.Managers.LocalDbManager
{
    public class PriceListManager : IPriceListManager
    {
        private readonly HttpClient _httpClient;
        private readonly IIndexedDbFactory _dbFactory;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<PriceListManager> _localizer;
        private IJSRuntime _runtime;

        public PriceListManager(HttpClient httpClient, 
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<PriceListManager> localizer)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
            _dbFactory = new IndexedDbFactory(_runtime);
        }

        public Task<PriceListResponse[]> FindByItemCode(string searchTerm)
        {
            throw new System.NotImplementedException();
        }

        public Task<IndexedSet<PriceListResponse>> GetAllPriceListAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<PriceListResponse> GetItemByPriceListAsync(string itm_code)
        {
            throw new System.NotImplementedException();
        }

        public async Task OpenDbAsync(IList<PriceListResponse> price_list_response)
        {
            var db = await _dbFactory.Create<PriceListDB>();

            foreach (var prt in price_list_response)
            {
                db.PriceListTb.Add(prt);
            }

            

            
        }
    }

    public class PriceListDB : IndexedDb
    {
        public PriceListDB(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version)
        {

        }


        public IndexedSet<PriceListResponse> PriceListTb { get; set; }
    }
}
