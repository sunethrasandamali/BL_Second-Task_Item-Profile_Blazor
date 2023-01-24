using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.ReportManager
{
    public interface IReportManager:IManager
    {
        Task<ReportConversion> PreparePDFFromHtml(ReportConversion reportInfo);
        Task<ReportCompanyDetailsResponse> GetReportCompanyInformation(ReportCompanyDetailsRequest requestDTO);
    }
}
