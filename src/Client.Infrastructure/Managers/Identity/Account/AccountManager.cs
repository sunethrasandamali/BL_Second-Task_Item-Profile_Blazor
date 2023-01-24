using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Client.Infrastructure.Extensions;
using BlueLotus360.Com.Shared.Wrapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Identity.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public object Routes { get; private set; }

        public AccountManager(HttpClient httpClient,IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            return null;
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
        {
            return null;
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            return null;
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            return null;
        }
    }
}