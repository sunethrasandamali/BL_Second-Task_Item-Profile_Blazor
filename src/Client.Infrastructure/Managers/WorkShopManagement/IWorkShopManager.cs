using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.Entities.MaterData;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;

namespace BlueLotus360.Com.Infrastructure.Managers.WorkShopManagement
{
    public interface IWorkShopManager:IManager
    {
        Task<IList<Vehicle>> GetVehicleDetailsByVehregNo(VehicleSearch request);
        Task<IList<WorkOrder>> GetJobHistory(Vehicle request);
        Task<ProjectResponse> CreateJob(Project request);
        Task<IList<ProjectResponse>> SelectOngoingProjectDetails(Vehicle request);
        Task SaveWorkOrder(Order order);
        Task EditWorkOrder(Order order);
        Task<WorkOrder> OpenWorkOrderV2(OrderOpenRequest request);
        Task<IList<BookingDetails>> GetRecentBookingDetails(Vehicle request);
        Task SaveWorkOrderTransaction(BLTransaction transaction);
        Task<BLTransaction> OpenWorkOrderTransaction(TransactionOpenRequest request);

        // For Insurence 
        Task SaveIRNWorkOrder(Order request);
        Task EditIRNWorkOrder(Order request);
        Task<UserRequestValidation> GetWorkShopValidatoion(WorkOrder request);
        Task<StockAsAtResponse> GetAvailableStock(StockAsAtRequest request);
        Task<IList<WorkOrder>> GetPendingIRNs(WorkOrder request);
    }
}
