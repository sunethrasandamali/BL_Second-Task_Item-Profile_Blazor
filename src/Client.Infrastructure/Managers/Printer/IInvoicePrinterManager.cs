using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Printer
{
    public interface IInvoicePrinterManager:IManager
    {
        Task PrintTransactionBillLocalAsync(TransactionReportLocal report, URLDefinitions definitions);

    }
}
