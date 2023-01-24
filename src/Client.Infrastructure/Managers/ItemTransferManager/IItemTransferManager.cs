using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.ItemTransferManager
{
    public interface IItemTransferManager:IManager
    {
        Task<int> CreateItemTransfer(ItemTransfer itm);
        
        Task<ItemTransferLineItem> GetItemsData(ItemTransferLineItem res);

        Task<ItmtrnsferValidationResponse> TransferValidator(ItemTransfer itm);
        Task<ItmtrnsferValidationResponse> InvoiceTransferValidator(LNDInvoice invoice);
        Task<List<FindItemTransferResponse>> Find(FindItemTransferRequest req);
        Task<ItemTransfer> RefreshForm(TransferOpenRequest req);
        //Task<ItemTransfer> RefreshScanInvoice(TransferOpenRequest req);

        Task<int> UpdateItemTransfer(ItemTransfer req);
        
        Task<List<ItemTransferLineItem>> GetInvoiceData(LNDInvoice res);
        Task<List<string>> GetInvoiceSerialNoList(LNDInvoice res);

        bool IsExceptionthrown();

        #region hold
        //Task<ItemTransfer> SaveTrnHeaderFromLoc(ItemTransfer itm);
        //Task<ItemTransfer> SaveTrnHeaderToLoc(ItemTransfer itm);
        //Task<int> SaveLine(ItemTransferLineItem itm);
        //Task<ItemTransfer> UpdateHeaderForOut(ItemTransfer req);
        //Task<ItemTransfer> UpdateHeaderForIn(ItemTransfer req);
        //Task UpdateLineForOut(ItemTransferLineItem line);
        //Task UpdateLineForIn(ItemTransferLineItem line);
        //Task FIFOPosting(ItemTransfer req);
        #endregion
    }
}
