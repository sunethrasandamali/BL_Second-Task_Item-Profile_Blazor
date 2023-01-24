using BlueLotus360.CleanArchitecture.Client.Infrastructure.Managers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using System;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared
{
    public  class ItemService
    {
     private   IComboDataManager _comboManager;

        public IComboDataManager ComboManager { get => _comboManager; set => _comboManager = value; }

        public async Task<ItemRateResponse> RequestItemRateForTransaction(BLTransaction transaction,TransactionLineItem lineItem)
        {


            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = BaseResponse.GetKeyValue(transaction.Location);
            request.AddressKey = BaseResponse.GetKeyValue(transaction.Address);
            request.AccountKey = BaseResponse.GetKeyValue(transaction.Account);

            request.ItemKey = lineItem.TransactionItem.ItemKey;
            request.EffectiveDate = DateTime.Now.Date;
            request.ConditionCode = "TrnTyp";
            request.ObjectKey = transaction.ElementKey;
            request.Code1Key = BaseResponse.GetKeyValue(transaction.Code1);
            return (await ComboManager.GetRate(request));
        }



    }
}
