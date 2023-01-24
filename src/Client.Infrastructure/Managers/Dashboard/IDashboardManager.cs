using BlueLotus360.Com.Shared.Wrapper;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.Dashboards.Queries.GetData;
using System.Collections.Generic;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;


namespace BlueLotus360.Com.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        bool IsExceptionthrown();
        Task<IResult<DashboardDataResponse>> GetDataAsync();

        Task<IList<LocationViseStockResponse>> GetLocationViseStocks(LocationViseStockRequest request);

        Task<IList<SalesDetails>> GetSalesDetails(SalesDetails request);

        Task<IList<SalesByLocationResponse>> GetLocationWiseSalesDetails(SalesDetails request);

        Task<IList<SalesRepDetailsForSalesByLocationResponse>> GetLocationWiseSalesRepDetails(SalesRepDetailsForSalesByLocation request);


        Task<IList<ActualVsBudgetedIncomeResponse>> GetActualVsBudgetedIncome(FinanceRequest request);

        Task<IList<GPft_NetPft_Margin_Response>> GetGPft_NetPft_Margin(FinanceRequest request);

        Task<IList<GPft_NetPft_DT>> Get_Monthly_GPft_NetPft_DT(FinanceRequest request);

        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis_Overdue(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_Overdue_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis_Overdue(FinanceRequest request);
        Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_Overdue_DT(FinanceRequestDT request);
        Task<IList<Debtors_Creditors_Transaction_Details>> Get_Debtor_Creditor_DT_Transaction_Details(FinanceRequestDTDetails filter);

    }
}