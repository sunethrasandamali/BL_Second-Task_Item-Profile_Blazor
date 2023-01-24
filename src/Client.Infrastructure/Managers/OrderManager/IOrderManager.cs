using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.OrderManager
{
    public interface IOrderManager : IManager
    {
        Task SaveOrder(Order order);
        Task<IList<OrderFindResults>> FindOrders(OrderFindDto request, URLDefinitions uRL);
        Task<Order> OpenOrder(OrderOpenRequest request);
        Task EditOrder(Order order);
        Task<IList<GetFromQuotResults>> FindFromQuotation(GetFromQuoatationDTO request, URLDefinitions uRL);
        Task<Order> OpenQuotation(OrderOpenRequest request);
        Task<IList<OrderFindResults>> LoadOrderApprovals(OrderFindDto request);
        Task UpadteOrderApprovals(OrderFindResults request);
        Task<IList<CodeBaseResponse>> GetOrderStatus();
        Task<int> PartnerOrderCount(RequestParameters partnerOrder);
        Task<IList<PartnerOrder>> GetAllPartnerOrder(RequestParameters partnerOrder);
        Task<PartnerOrder> GetLastSyncTime(APIRequestParameters request);
        Task<CodeBaseResponse> GetOrderStatusByPartnerStatus(CodeBaseResponse request);
        Task<PartnerOrder> SavePartnerOrders(PartnerOrder request);
        Task<ItemResponse> GetItemByItemCode(ItemResponse request);
        Task<PartnerOrder> GetPartnerOrdersByOrderKy(RequestParameters request);
        Task<bool> InsertLastOrderSync(RequestParameters request);
    }
}
