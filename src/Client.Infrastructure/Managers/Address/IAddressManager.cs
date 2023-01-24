using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Infrastructure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Address
{
    public interface IAddressManager : IManager
    {

        Task<AddressCreateServerResponse> CreateNewAddress(AddressMaster record);
        Task<AddressMaster> CreateCustomer(AddressMaster customer);
        Task<AddressMaster> CreateCustomerValidation(AddressMaster customer);
        Task<AddressMaster> CheckAdvanceAnalysisAvailability(AddressMaster customer);
        Task<AddressMaster> CreateAdvanceAnalysis(AddressMaster customer);
        Task<AddressResponse> GetAddressByUserKy();
    }

}
