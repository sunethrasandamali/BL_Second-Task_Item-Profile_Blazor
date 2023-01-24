using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder
{
    public class SalesOrderValidator : IOrderValidator
    {

        private Order _order;
        public UserMessageManager UserMessages { get; set; }

        public bool CanAddItemToGrid()
        {
            UserMessages.UserMessages.Clear();
            if (_order.SelectedOrderItem.NeedToRequestFromAnotherLocation())
            {
                if (_order.SelectedOrderItem.OrderLineLocation.CodeKey == 1)
                {
                    UserMessages.AddErrorMessage("Please Select a  location for Item Requisition");
                }

                if (_order.OrderLocation.CodeKey == _order.SelectedOrderItem.OrderLineLocation.CodeKey)
                {
                    UserMessages.AddErrorMessage("Please Select a different location for Item Requisition");
                }

            }

            if (_order.SelectedOrderItem.TransactionQuantity <= 0)
            {
                UserMessages.AddErrorMessage("Transaction Quantity Cannot be Zero or less");
            }

            if (_order.SelectedOrderItem.TransactionUnit.UnitKey < 11)
            {
                UserMessages.AddErrorMessage("Transaction Unit Cannot be empty");
            }



            return UserMessages.UserMessages.Count == 0;
        }

        public bool CanRequestToAddItem()
        {
            throw new NotImplementedException();
        }

        public bool CanChangeHeaderInformatiom()
        {
            return _order.OrderItems.Count==0;
        }

        public SalesOrderValidator(Order order)
        {
            _order = order;
            UserMessages = new UserMessageManager();


        }
    }
}

