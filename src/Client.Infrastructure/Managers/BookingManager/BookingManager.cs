using BL10.CleanArchitecture.Domain.Entities.Booking;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;

namespace BlueLotus360.Com.Infrastructure.Managers.BookingManager
{
    public class BookingManager : IBookingManager
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public BookingManager(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);

        }

        public async Task<IList<CustomerDetailsByVehicle>> GetBookingCustomerDetails(BookingVehicleDetails request)
        {
            IList<CustomerDetailsByVehicle> responses = new List<CustomerDetailsByVehicle>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetBookedCustomerDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<IList<CustomerDetailsByVehicle>>(content);
            }
            catch (Exception exp)
            {
                responses = new List<CustomerDetailsByVehicle>();
            }
            finally
            {

            }

            return responses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<BookingTabDetails> BookingTabDetails(BookingDetails request)
        {
            BookingTabDetails responses = new BookingTabDetails();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.TabDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<BookingTabDetails>(content);
            }
            catch (Exception exp)
            {
                responses = new BookingTabDetails();
            }
            finally
            {

            }

            return responses;
        }

        public async Task<bool> CreateCustomer(AddressMaster customer)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateCustomer, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                JsonConvert.DeserializeObject<bool>(content);
            }
            catch (Exception exp)
            {
                return false;
            }
            finally
            {

            }
            return false;
        }

        public async Task<bool> CreateServiceType(BookingDetails type)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateNewServiceType, type);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                JsonConvert.DeserializeObject<bool>(content);
            }
            catch (Exception exp)
            {
                return false;
            }
            finally
            {

            }
            return false;
        }

        public async Task<IList<BookingDetails>> GetBookingDetailsList(BookingDetails request)
        {
            List<BookingDetails> responses = new List<BookingDetails>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetBookingList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<List<BookingDetails>>(content);
            }
            catch (Exception exp)
            {
                responses = new List<BookingDetails>();
            }
            finally
            {

            }

            return responses;
        }

        public async Task<BookingVehicleDetails> GetBookingItmDetails(BookingVehicleDetails request)
        {
            BookingVehicleDetails responses = new BookingVehicleDetails();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetBookingItmDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<BookingVehicleDetails>(content);
            }
            catch (Exception exp)
            {
                responses = new BookingVehicleDetails();
            }
            finally
            {

            }

            return responses;
        }

        public async  Task<BookingVehicleDetails> GetBookingVehicleDetails(BookingVehicleDetails request)
        {
            BookingVehicleDetails responses = new BookingVehicleDetails();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetVehicleDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<BookingVehicleDetails>(content);
            }
            catch (Exception exp)
            {
                responses = new BookingVehicleDetails();
            }
            finally
            {

            }

            return responses;
        }

        public async Task<bool> InsertServiceType(BookingDetails request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertServiceType, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                JsonConvert.DeserializeObject<bool>(content);
            }
            catch (Exception exp)
            {
                return false;
            }
            finally
            {

            }
            return false;
        }

        public async Task<bool> InsertUpdateBooking(BookingInsertUpdate insertUpdate)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetInsertUpdateBooking, insertUpdate);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                JsonConvert.DeserializeObject<bool>(content);
            }
            catch (Exception exp)
            {
                return false;
            }
            finally
            {

            }
            return false;
        }
    }
}
