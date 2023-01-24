using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.AccountProfile
{
    public interface IProfileManager : IManager
    {
        bool IsExceptionthrown();
        Task<IList<AccountProfileResponse>> GetAccountProfileList(AccountProfileRequest request);

        Task<AccountProfileInsertResponse> InsertAccountProfile(AccountProfileInsertRequest request);

        Task<AccountProfileResponse> UpdatedAccountProfile(AccountProfileResponse request);
    }
}
