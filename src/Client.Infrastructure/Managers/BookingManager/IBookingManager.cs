using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.BookingManager
{
    public interface IBookingManager : IManager
    {
        Task<IList<CustomerDetailsByVehicle>> GetBookingCustomerDetails(BookingVehicleDetails request);
        //
        Task<IList<BookingDetails>> GetBookingDetailsList(BookingDetails request);
        Task<BookingVehicleDetails> GetBookingVehicleDetails(BookingVehicleDetails request);
        Task<BookingVehicleDetails> GetBookingItmDetails(BookingVehicleDetails request);
        Task<bool> InsertUpdateBooking(BookingInsertUpdate insertUpdate);
        Task<BookingTabDetails> BookingTabDetails(BookingDetails request);
        Task<bool> CreateCustomer(AddressMaster customer);
        Task<bool> CreateServiceType(BookingDetails type);
        Task<bool> InsertServiceType(BookingDetails request);
    }
}
