using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Application.Requests.Identity;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Wrapper;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Identity.Authentication
{
    public interface IAuthenticationManager : IManager
    {
        Task<TokenResponse> Login(TokenRequest model);

        Task<IResult> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();

        Task<CompletedUserAuth> GetUserInformation();
    }
}