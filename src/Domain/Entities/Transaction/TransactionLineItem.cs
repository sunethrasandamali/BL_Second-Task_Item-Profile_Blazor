﻿using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Utility;
namespace BlueLotus360.CleanArchitecture.Domain.Entities.Transaction
{
    public class TransactionLineItem : BaseEntity
    {
        public long TransactionKey { get; set; } = 1;
        public long ElementKey { get; set; } = 1;   
        public long ItemTransactionKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; } = DateTime.Now; 
        public int LineNumber { get; set; }
        public long ItemTransferLinkKey = 1;
        public ItemResponse TransactionItem { get; set; } = new ItemResponse();
        public CodeBaseResponse TransactionLocation { get; set; } = new CodeBaseResponse();
        public decimal Quantity { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransactionRate { get; set; }
        public decimal TransactionPrice { get; set; }
        public decimal Rate { get; set; }
        public UnitResponse TransactionUnit { get; set; } = new UnitResponse();
        public CodeBaseResponse BussinessUnit { get; set; } = new CodeBaseResponse();
        public decimal DiscountAmount { get; set; }
        public decimal TransactionDiscountAmount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public ProjectResponse TransactionProject { get; set; } = new ProjectResponse();
        public AddressResponse Address { get; set; } = new AddressResponse();
        public CodeBaseResponse ItemProperty { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ConditionsState { get; set; } = new CodeBaseResponse();
        public int IsInventory { get; set; } = 1;
        public int IsCosting { get; set; } = 1;
        public int IsSetOff { get; set; } = 0;
        public int OrderDetailKey { get; set; } = 1;
        public long ReferenceItemTransactionKey { get; set; } = 1;
        public CodeBaseResponse Code1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Code2 { get; set; } = new CodeBaseResponse();
        public string Description { get; set; } = "";
        public string Remarks { get; set; } = "";
        public long OrderKey { get; set; } = 1;
        public long Skey { get; set; } = 1;
        public decimal QuantityPercentage { get; set; }
        public decimal HeaderDiscountAmount { get; set; }
        public ProjectResponse Project2 { get; set; }= new ProjectResponse();
        public decimal Quantity2 { get; set; }
        public decimal TaskQuantity { get; set; }
        public UnitResponse TaskUnit { get; set; } = new UnitResponse();
        public decimal FromNo { get; set; }
        public decimal ToNo { get; set; }
        public decimal NextActionNo { get; set; }
        public DateTime NextActionDate { get; set; } = DateTime.Now;
        public CodeBaseResponse NextActionType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemPack { get; set; } = new CodeBaseResponse();
        public decimal CommisionPercentage { get; set; }
        public decimal ItemTaxType1 { get; set; }
        public decimal ItemTaxType2 { get; set; }
        public decimal ItemTaxType3 { get; set; }
        public decimal ItemTaxType4 { get; set; }
        public decimal ItemTaxType5 { get; set; }
        public decimal ItemTaxType1Per { get; set; }
        public decimal ItemTaxType2Per { get; set; }
        public decimal ItemTaxType3Per { get; set; }
        public decimal ItemTaxType4Per { get; set; }
        public decimal ItemTaxType5Per { get; set; }
        public long LCKey { get; set; } = 1;
        public long LoanKey { get; set; } = 1;
        public long ProcessDetailKey { get; set; } = 1;
        public long LCDetailKey { get; set; } = 1;
        public long LoanDetailKey { get; set; } = 1;
        public CodeBaseResponse Analysis1 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis3 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis4 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis5 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Analysis6 { get; set; } = new CodeBaseResponse();
        public decimal SalesPrice2 { get; set; }
        public AddressResponse ReservationAddress { get; set; } = new AddressResponse();
        public long ItemBudgetKey { get; } = 1;
        public bool IsQuantiy { get; set; } = false;

        public decimal BalanceQuantity { get; set; }

        /*@isPri tinyint = 0 ,
     @isVal bit = 0 ,
     @isDisplay bit = 1 ,
     @isNoPrnPri bit = 0 ,
     @isQlity bit = 0 ,
     @MADrAccKy bigint = 1 ,
     @MACrAccKy bigint = 1 ,
     @FADrAccKy bigint = 1 ,
     @FACrAccKy bigint = 1 ,
     @QtyMul float = 1 ,
     @isRateInclTT1 bit = 0 ,
     @DisAmt2 money = 0 ,
     @PreItmTrnKy bigint = 1,
     @DepItmTrnKy bigint = 1,
     @AcsLvlKy int = 1 ,
     @ItmEANKy bigint =1,
     @DlvAdrKy int =1,
         */
        public decimal Amount1 { get; set; }
        public decimal Amount2 { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Amount4 { get; set; }
        public decimal Amount5 { get; set; }
        public decimal Amount6 { get; set; }
        public decimal Amount7 { get; set; }
        public decimal Amount8 { get; set; }
        public decimal Amount9 { get; set; }
        public decimal Amount10 { get; set; }
        public decimal LooseQuantity { get; set; }
        public DateTime DateTime1 { get; set; } = DateTime.Now;
        public DateTime DateTime2 { get; set; } = DateTime.Now;
        public DateTime DateTime3 { get; set; } = DateTime.Now;
        public CodeBaseResponse TransactionType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ProjectTaskLocation { get; set; } = new CodeBaseResponse();
        public long ObjectKey = 1;
        public long FromItemTransactionKey { get; set; } = 1;
        public long OfferItemTransactionKey { get; set; } = 1;
        public decimal LineAmount { get; set; }
        public ItemResponse ProcessItem { get; set; } = new ItemResponse();
        public CodeBaseResponse ItemCategory2 { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse ItemCategory1 { get; set; } = new CodeBaseResponse();
        public decimal LineNetRate { get; set; }


        public decimal AvailableQuantity { get; set; } = 0;

        public bool IsInEditMode { get; set; }

        public string SerialNumber { get; set; } = "";

        //
        public decimal MarkupPercentage { get; set; }
        public decimal MarkupAmount { get; set; }
        public decimal TotalMarkupAmount { get; set; }
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public IList<ItemSerialNumber> SerialNumbers { get; set; }= new List<ItemSerialNumber>();

        // CarMart
        public AccountResponse BaringPrinciple { get; set; }
        public decimal PrinciplePrecentage { get; set; }
        public decimal PrincipleAmount { get; set; }     
        public AccountResponse BaringCompany { get; set; }
        public decimal CompanyPrecentage { get; set; }
        public decimal CompanyAmount { get; set; }
        public decimal CustomerAmount { get; set; }
        public int IsSelected { get; set; }
        public decimal Time { get; set; }
        public int IsMaterialItem { get; set; }
        public int IsServiceItem { get; set; }
        public decimal SubTotal { get; set; }

        public TransactionLineItem()
        {
            TransactionItem = new();
            TransactionUnit = new UnitResponse();
            TransactionQuantity = 1;
            Quantity = 1;
            Quantity2 = 1;
            TransactionItem = new ItemResponse();
            LineNetRate = 0;
            ItemCategory2 = new CodeBaseResponse();
            ItemCategory1 = new CodeBaseResponse();
            Analysis1 = new CodeBaseResponse();
            Analysis2 = new CodeBaseResponse();
            Analysis3 = new CodeBaseResponse();
            Analysis5 = new CodeBaseResponse();
            Description = String.Empty;
            TransactionRate = 0;

            SerialNumbers = new List<ItemSerialNumber>();
        }

        public decimal GetLineDiscount()
        {
            return Math.Round((this.TransactionRate * DiscountPercentage / 100) * TransactionQuantity, 2);

        }

        public decimal GetLineTotalWithDiscount()
        {
            decimal lineTotal = GetLineTotalWithoutDiscount();
            decimal dis = GetLineDiscount();

            return (lineTotal - dis);
        }

        public decimal GetNetLineTotal()
        {

            MarkupAmount = MarkupPercentage * TransactionRate;
            TotalMarkupAmount = MarkupAmount * TransactionQuantity;
            return GetLineTotalWithDiscount() + TotalMarkupAmount;
        }


        public void CopyFrom(TransactionLineItem source)
        {
            source.CopyProperties(this);

        }

        public decimal GetTotalMarkupAmount()
        {
            MarkupAmount = MarkupPercentage * TransactionRate;
            TotalMarkupAmount = MarkupAmount * TransactionQuantity;
            return TotalMarkupAmount;
        }



        public decimal GetLineTotalWithoutDiscount()
        {
            return Math.Round(TransactionQuantity * TransactionRate, 2);
        }

        public bool IsKiloWashItem()
        {
            return Convert.ToBoolean(ItemCategory2.AddtionalData["IsKiloWash"]);
        }
        public bool IsCommonItem()
        {
            return Convert.ToBoolean(ItemCategory2.AddtionalData["IsCommon"]);
        }


        #region hold
        //public decimal GetLineDiscount()
        //{
        //    return Math.Round((this.TransactionRate * DiscountPercentage / 100) * TransactionQuantity, 2);

        //}

        //public decimal GetLineTotalWithDiscount()
        //{
        //    decimal lineTotal = GetLineTotalWithoutDiscount();
        //    decimal dis = GetLineDiscount();

        //    return (lineTotal - dis);
        //}

        //public decimal GetNetLineTotal()
        //{

        //    MarkupAmount = MarkupPercentage * TransactionRate;
        //    TotalMarkupAmount = MarkupAmount * TransactionQuantity;
        //    return GetLineTotalWithDiscount() + TotalMarkupAmount;
        //}


        //public void CopyFrom(TransactionLineItem source)
        //{
        //    source.CopyProperties(this);

        //}





        //public decimal GetTotalMarkupAmount()
        //{
        //    MarkupAmount = MarkupPercentage * TransactionRate;
        //    TotalMarkupAmount = MarkupAmount * TransactionQuantity;
        //    return TotalMarkupAmount;
        //}



        //public decimal GetLineTotalWithoutDiscount()
        //{
        //    return Math.Round(TransactionQuantity * TransactionRate, 2);
        //}
        #endregion

    }
}