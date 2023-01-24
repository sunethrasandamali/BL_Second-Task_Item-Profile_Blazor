
using BlueLotus360.CleanArchitecture.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;

namespace BlueLotus360.Com.Infrastructure.Managers.Company
{
    public interface ICompanyManager :IManager
    {
        Task<IList<CompanyResponse>> GetUserCompanies();

        Task UpdateSelectedCompany(CompanyResponse response);

        Task<ReportCompanyDetailsResponse> GetCompanyDetailsResponse();

    }
}
