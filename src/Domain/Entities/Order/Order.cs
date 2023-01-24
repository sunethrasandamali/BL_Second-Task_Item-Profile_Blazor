﻿using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Domain.Entities.Order
{
    public class Order : BaseEntity
    {
        public long OrderKey { get; set; } = 1;
        public string OrderNumber { get; set; } = "";
        public string OrderDocumentNumber { get; set; } = "";
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime OrderFinishDate { get; set; } = DateTime.Now;
        public DateTime DeliveryDate { get; set; } = DateTime.Now;
        public CodeBaseResponse OrderLocation { get; set; }
        public CodeBaseResponse OrderPaymentTerm { get; set; }
        public AddressResponse OrderCustomer { get; set; }
        public AddressResponse OrderRepAddress { get; set; }
        public AccountResponse OrderAccount { get; set; } = new AccountResponse();
        public CodeBaseResponse OrderType { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse OrderCategory1 { get; set; }
        public CodeBaseResponse OrderCategory2 { get; set; }
        public CodeBaseResponse OrderStatus { get; set; }
        public ProjectResponse OrderProject { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public OrderItem SelectedOrderItem { get; set; }
        public OrderItem EditingLineItem { get; set; }
        public long FormObjectKey { get; set; } = 1;
        public decimal HeaderLevelDisountPrecentage { get; set; }
        public bool IsFromQuotation { get; set; }
        public int Cd1Ky { get; set; } = 1;
        public string Prefix { get; set; } = "";
        public CodeBaseResponse OrderPrefix { get; set; }
        public decimal MeterReading { get; set; }
        public AddressResponse EnteredUser { get; set; }
		public CodeBaseResponse BussinessUnit { get; set; } 
		//
		public ItemResponse Insurance { get; set; }

        public Order()
        {
            OrderLocation = new CodeBaseResponse();
            OrderPaymentTerm = new CodeBaseResponse();
            OrderCustomer = new AddressResponse();
            OrderRepAddress = new AddressResponse();
            OrderDocumentNumber = "";
            OrderItems = new List<OrderItem>();
            OrderType = new CodeBaseResponse();
            OrderAccount = new AccountResponse();
            OrderDocumentNumber = Guid.NewGuid().ToString().Substring(0, 8);
            SelectedOrderItem = new OrderItem();
            OrderItems=new List<OrderItem>();
            EditingLineItem = new OrderItem();
            OrderCategory1 = new CodeBaseResponse();
            OrderCategory2 = new CodeBaseResponse();
            OrderProject = new ProjectResponse();
            OrderStatus = new CodeBaseResponse();
            OrderPrefix=new CodeBaseResponse();
            EnteredUser=new AddressResponse();
            Insurance = new ItemResponse();
            BussinessUnit = new CodeBaseResponse();

		}


        public OrderItem CreateOrderItem(ItemResponse transactionItem, CodeBaseResponse parentLocation)
        {
            OrderItem item = new OrderItem();
            item.OrderLineLocation = parentLocation;
            item.OrderType = this.OrderType;
            item.TransactionQuantity = 1;
            item.TransactionItem = transactionItem;
            item.AvailableStock = 10;
            item.TransactionRate = transactionItem.Rate;
            this.SelectedOrderItem = item;
            return item;
        }



        public void Update(OrderItem itm, int index)
        {
            OrderItems[index].TransactionItem = itm.TransactionItem;
            OrderItems[index].TransactionQuantity = itm.TransactionQuantity;
            OrderItems[index].Rate = itm.Rate;
            OrderItems[index].DiscountPercentage = itm.DiscountPercentage;
            OrderItems[index].OrderType = itm.OrderType;
            OrderItems[index].AvailableStock = itm.AvailableStock;
            OrderItems[index].TransactionUnit = itm.TransactionUnit;

            if (itm.RequestedQuantity > 0)
            {
                OrderItems[index].RequestedQuantity = itm.RequestedQuantity;
                OrderItems[index].OrderLineLocation = itm.OrderLineLocation;
                OrderItems[index].IsTransfer = itm.IsTransfer;
                OrderItems[index].IsTransferConfirmed = itm.IsTransferConfirmed;
            }

        }


        public decimal GetOrderTotalWithDiscounts()
        {
            return OrderItems.Sum(x => x.GetLineTotalWithDiscount());
        }

        public decimal GetOrderTotalWithoutDiscounts()
        {
            return OrderItems.Sum(x => x.GetLineTotalWithoutDiscount());
        }

        public decimal GetOrderTotalWithTaxes()
        {
            return OrderItems.Sum(x => x.GetLineTotalWithTax());
        }


        public decimal GetOrderRateTotal()
        {
            return OrderItems.Sum(x =>(x.IsActive==1)? x.TransactionRate:0);
            
        }

        public decimal GetOrderDiscountTotal()
        {
            return OrderItems.Sum(d => d.GetLineDiscount());
        }

        public decimal GetQuantityTotal()
        {
            return OrderItems.Sum(q => (q.IsActive == 1) ? q.TransactionQuantity:0);
        }

        public decimal GetRequestedQuantityTotal()
        {
            return OrderItems.Sum(q => (q.IsActive == 1) ? q.RequestedQuantity:0);
        }

        public decimal GetTotalTaxType1()
        {
            return OrderItems.Sum(q => q.GetItemTaxType1());
        }

        public decimal GetTotalTaxType4()
        {
            return OrderItems.Sum(q => q.GetItemTaxType4());
        }

        public void AddGridItems(OrderItem item)
        {
            OrderItems.Add(item);
            SelectedOrderItem = new();
        }


        public void CopyFrom(Order source)
        {
            source.CopyProperties(this);

        }


    }


    public class OrderItem : BaseEntity
    {
        public int FromOrderDetailKey { get; set; } = 1;
        public decimal TransactionRate { get; set; }
        public decimal Rate { get; set; }
        public CodeBaseResponse OrderLineLocation { get; set; }
        public int IsTransfer { get; set; }
        public int IsTransferConfirmed { get; set; }
        public decimal TransactionQuantity { get; set; }
        public decimal TransferQuantity { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public ItemResponse TransactionItem { get; set; }
        public CodeBaseResponse OrderType { get; set; }
        public decimal AvailableStock { get; set; }
        public decimal LineNumber { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal LineTotal { get; set; }
        public decimal LineTotalWithoutDiscount { get; set; }
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
        public UnitResponse TransactionUnit { get; set; }    
        public CodeBaseResponse BussinessUnit { get; set; }
        public bool IsInEditMode { get; set; }
        public bool IsJustAdded { get; set; }
        public bool IsLineDiscountChanged { get; set; }
        public int OrderDetailsAccountKey { get; set; }
        public AccountResponse BaringPrinciple { get; set; }
        public decimal PrinciplePrecentage { get; set; }
        public decimal PrincipleAmount { get; set; }
        public AccountResponse BaringCompany { get; set; }
        public decimal CompanyPrecentage { get; set; }
        public decimal CompanyAmount { get; set; }
        public decimal CustomerAmount { get; set; }
        public int IsSelected { get; set; }
        public AccountResponse Supplier { get; set; }//where to map
        public User EnteredUser { get; set; }   
        public AddressResponse ResourceAddress { get; set; }//technician ,where to map
        public string Description { get; set; } = "";
        public DateTime EnteredDate { get; set; }//where to map
        public bool IsServiceItem { get; set; }
        public bool IsMaterialItem { get; set; } 
        public bool IsNoteItem { get; set; }
        public decimal Time { get; set; }
        public decimal SubTotal { get; set; }
        public ItemResponse Packages { get; set; } // FOR INSURENCE
        public decimal VAT { get; set; }
        public decimal VATAmount { get; set; }
        public string Catergory { get; set; } = "";
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public OrderItem()
        {
            OrderLineLocation = new CodeBaseResponse();
            TransactionItem = new ItemResponse();
            TransactionUnit = new UnitResponse();
            OrderType = new CodeBaseResponse();
            BussinessUnit = new CodeBaseResponse();
            Supplier = new AccountResponse();
            ResourceAddress = new AddressResponse();
            EnteredUser = new User();
            BaringCompany = new AccountResponse();
            BaringPrinciple = new AccountResponse();
            Packages = new ItemResponse();
        }

        public decimal GetLineDiscount()
        {
            if (this.IsActive == 1)
            {
                DiscountAmount = (this.TransactionRate * DiscountPercentage / 100) * TransactionQuantity;
            }
            else
            {
                DiscountAmount = 0;
            }

            return DiscountAmount;

        }

        public decimal GetLineTotalWithDiscount()
        {
            return GetLineTotalWithoutDiscount() - GetLineDiscount();
        }


        public decimal GetLineTotalWithTax()
        {
            return GetLineTotalWithDiscount() + GetItemTaxType1() + GetItemTaxType4();
        }

        public decimal GetLineTotalWithoutDiscount()
        {
            if (this.IsActive == 1)
            {
                return TransactionQuantity * TransactionRate;
            }
            else
            {
                return 0;
            }
                
            
        }


        public bool NeedToRequestFromAnotherLocation()
        {
            return this.AvailableStock < this.TransactionQuantity;
        }



        public decimal GetItemTaxType1()
        {
            if (this.IsActive == 1)
            {
                ItemTaxType1 = (GetLineTotalWithDiscount() * this.ItemTaxType1Per) / 100;
            }
            else
            {
                ItemTaxType1 = 0;
            }

            return ItemTaxType1;
        }

        public decimal GetItemTaxType4()
        {
            if (this.IsActive == 1)
            {
                ItemTaxType4 = (GetLineTotalWithDiscount() * this.ItemTaxType4Per) / 100;
            }
            else
            {
                ItemTaxType4 = 0;
            }   
            

            return ItemTaxType4;
        }

        public decimal GetLineCompanyAmount()
        {
            CompanyAmount = (this.GetLineTotalWithTax() * CompanyPrecentage / 100);

            return CompanyAmount;
        }

        public decimal GetLinePrincipleAmount()
        {
            PrincipleAmount = (this.GetLineTotalWithTax() * PrinciplePrecentage / 100);

            return PrincipleAmount;
        }

        public decimal GetLineCustomerAmount()
        {
            CustomerAmount = GetLineTotalWithTax() - GetLineCompanyAmount() - GetLinePrincipleAmount();
            return CustomerAmount;
        }

        
        public void CalculateLineBalance()
        {
            this.GetLineTotalWithDiscount();
            this.GetLineCompanyAmount();
            this.GetLinePrincipleAmount();
            this.GetLineCustomerAmount();

            SubTotal = GetLineTotalWithTax();

            
        }

        public void CopyFrom(OrderItem source)
        {
            source.CopyProperties(this);

        }
    }

    public class ItemRateRequest
    {
        public long ObjectKey { get; set; } = 1;

        public long ItemKey { get; set; } = 1;
        public DateTime EffectiveDate { get; set; }
        public long LocationKey { get; set; } = 1;
        public long TransactionTypeKey { get; set; } = 1;
        public long BussienssUnitKey { get; set; } = 1;
        public long ProjectKey { get; set; } = 1;
        public long AddressKey { get; set; } = 1;

        public long AccountKey { get; set; } = 1;
        public long PayementTermKey { get; set; } = 1;
        public long Code1Key { get; set; }
        public long Code2Key { get; set; }
        public decimal Rate { get; set; }

        public string ConditionCode { get; set; } = "";




    }

    public class ItemRequestModel
    {
        public long TransactionTypeKey { get; set; } = 1;
        public long ItemKey { get; set; } = 1;
        public long LocationKey { get; set; } = 1;

        public long ProjectKey { get; set; } = 1;

        public long AddressKey { get; set; } = 1;
        public string ItemCode { get; set; } = "";
        public DateTime RequestDate { get; set; }

    }


    public class OrderSaveResponse
    {
        public string OrderNumber { get; set; } = "";
        public string Prefix { get; set; } = "";

        public long OrderKey { get; set; } = 1;
    }

    //public class GenericOrder : BaseEntity
    //{
    //    public int OrderKey { get; set; } = 1;
    //    public string OrderNumber { get; set; } = "";
    //    public string OrderDocumentNumber { get; set; } = "";
    //    public DateTime OrderDate { get; set; }
    //    public CodeBaseResponse OrderLocation { get; set; } = new CodeBaseResponse();
    //    public CodeBaseResponse OrderPaymentTerm { get; set; } = new CodeBaseResponse();
    //    public AddressResponse OrderCustomer { get; set; } = new AddressResponse();
    //    public AddressResponse OrderRepAddress { get; set; } = new AddressResponse();
    //    public AccountResponse OrderAccount { get; set; }
    //    public CodeBaseResponse OrderType { get; set; } = new CodeBaseResponse();
    //    public IList<GenericOrderItem> OrderItems { get; set; }
    //    public OrderItem SelectedOrderItem { get; set; }
    //    public int FormObjectKey { get; set; } = 1;
    //    public int OrderDetKy { get; set; } = 1;


    //    public GenericOrder()
    //    {
    //        OrderLocation = new CodeBaseResponse();
    //        OrderPaymentTerm = new CodeBaseResponse();
    //        OrderCustomer = new AddressResponse();
    //        OrderRepAddress=new AddressResponse();
    //        OrderAccount=new AccountResponse();
    //        OrderType=new CodeBaseResponse();
    //        SelectedOrderItem=new OrderItem();
    //        OrderDocumentNumber = "";
    //        OrderItems = new List<GenericOrderItem>();
    //    }

    //    public void CopyFrom(Order source)
    //    {
    //        source.CopyProperties(this);

    //    }

    //}


    //public class GenericOrderItem
    //{
    //    public decimal TransactionRate { get; set; }
    //    public decimal Rate { get; set; }
    //    public CodeBaseResponse OrderLineLocation { get; set; }
    //    public int IsTransfer { get; set; }
    //    public int IsTransferConfirmed { get; set; }
    //    public decimal TransactionQuantity { get; set; }
    //    public decimal TransferQuantity { get; set; }
    //    public decimal DiscountPercentage { get; set; }
    //    public decimal DiscountAmount { get; set; }
    //    public ItemResponse TransactionItem { get; set; }
    //    public CodeBaseResponse OrderType { get; set; }
    //    public decimal AvailableStock { get; set; }
    //    public decimal LineNumber { get; set; }
    //    public decimal RequestedQuantity { get; set; }
    //    public decimal LineTotal { get; set; }
    //    public decimal LineTotalWithoutDiscount { get; set; }

    //    public decimal ItemTaxType1 { get; set; }
    //    public decimal ItemTaxType2 { get; set; }
    //    public decimal ItemTaxType3 { get; set; }
    //    public decimal ItemTaxType4 { get; set; }
    //    public decimal ItemTaxType5 { get; set; }

    //    public decimal ItemTaxType1Per { get; set; }
    //    public decimal ItemTaxType2Per { get; set; }
    //    public decimal ItemTaxType3Per { get; set; }
    //    public decimal ItemTaxType4Per { get; set; }
    //    public decimal ItemTaxType5Per { get; set; }
    //    //
    //    public decimal AvailableQuantity { get; set; } = 0;
    //    public UnitResponse Unit { get; set; } = new UnitResponse();
    //    public decimal VAT { get; set; } = 0;
    //    public decimal SVAT { get; set; } = 0;
    //    public decimal AvlStk { get; set; } = 0;

    //    public decimal OriginalQuantity { get; set; }


    //    public GenericOrderItem()
    //    {
    //        OrderLineLocation = new CodeBaseResponse();
    //        TransactionItem = new ItemResponse();
    //        OrderType = new CodeBaseResponse();
    //    }



    //}

    public class OrderOpenRequest
    {
        public long OrderKey { get; set; } = 1;//get quotation order ky
        public long BaseTypKy { get; set; } = 1;//get sales order ordertypky
        public long PrjKy { get; set; } = 1;// selected projected ky or else 1
        public long ObjKy { get; set; } = 1;
        public long TransactionKey { get; set; } = 1;

    }

    public class OrderFindDto
    {
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
        public int OrderTypeKey { get; set; }
        public string OrderNo { get; set; } = "";
        public int FromOrderNumber { get; set; } = 1;
        public int ToOrderNumber { get; set; } = int.MaxValue;

        public string DocumentNumber { get; set; } = "";
        public string YourReference { get; set; } = "";

        public int LocationKey { get; set; } = 1;

        public int ProjectKey { get; set; } = 1;
        public int AccountKey { get; set; } = 1;

        public int AddressKey { get; set; } = 1;

        public int ApproveStatusKey { get; set; } = 1;
        public bool IsPrinterd { get; set; }

        public bool IsRecurence { get; set; }

        public int PrefixKey { get; set; } = 1;

        public int ObjectKey { get; set; } = 1;
        public CodeBaseResponse Location { get; set; }
        public CodeBaseResponse Prefix { get; set; }

        public OrderFindDto()
        {
            Location= new CodeBaseResponse();
            Prefix = new CodeBaseResponse();
        }
    }

    public class OrderFindResults
    {
        public int OrderKey { get; set; } = 1;

        public DateTime OrderDate { get; set; }

        public string Prefix { get; set; } = "";

        public string OrderNumber { get; set; } = "";

        public string DocumentNumber { get; set; } = "";

        public string YourReference { get; set; } = "";

        public string Description { get; set; } = "";
        public string CusSupId { get; set; } = "";
        public string CusSupName { get; set; } = "";
        public int ProjectKey { get; set; }
        public string ProjectName { get; set; } = "";
        public CodeBaseResponse ApproveState { get; set; }
        public AccountResponse Account { get; set; }
        public int RequestingObjectKey { get; set; }
        public string PreviewURL { get; set; } = "";
        public int EntUsrKy { get; set; }
        public int IsActive { get; set; }
        public CodeBaseResponse ApproveReason { get; set; }
        public OrderFindResults()
        {
            ApproveState = new CodeBaseResponse();
            Account = new AccountResponse();
            ApproveReason = new CodeBaseResponse(); 
        }
    }

    public class FindQuoteDTO
    {
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now;
        public int OrderTypeKey { get; set; }
        public int FromOrderNumber { get; set; } = 0;
        public int ToOrderNumber { get; set; } = int.MaxValue;

        public string DocumentNumber { get; set; } = "";
        public string YourReference { get; set; } = "";

        public int LocationKey { get; set; } = 1;

        public int ProjectKey { get; set; } = 1;
        public int AccountKey { get; set; } = 1;

        public int AddressKey { get; set; } = 1;

        public int ApproveStatusKey { get; set; } = 1;
        public bool IsPrinterd { get; set; }

        public bool IsRecurence { get; set; }

        public int PrefixKey { get; set; } = 1;

        public int ObjectKey { get; set; } = 1;
    }

    public class GetFromQuoatationDTO
    {
        public int OrdTypKy { get; set; }
        public int PreOrdTypKy { get; set; }
        public AddressResponse Supplier { get; set; }
        public AddressResponse AdvAnalysis { get; set; }
        public CodeBaseResponse Location { get; set; }
        public DateTime FromDate { get; set; } = new DateTime(1990, 1, 1);
        public DateTime ToDate { get; set; } = DateTime.Now;
        public CodeBaseResponse Project { get; set; }
        public string SoNo { get; set; } = "";
        public int OrdNo { get; set; }
        public CodeBaseResponse PreFix { get; set; }
        public long ObjKy { get; set; }

        public GetFromQuoatationDTO()
        {
            Supplier = new();
            AdvAnalysis = new();
            Location=new CodeBaseResponse();
            Project=new CodeBaseResponse();
            PreFix=new CodeBaseResponse();
        }

    }
    public class GetFromQuotResults
    {
        public int OrdKy { get; set; }
        public string OrdNo { get; set; } = "";
        public DateTime OrdDt { get; set; }
        public string DocNo { get; set; } = "";
        public int PrjId { get; set; }
        public string PrjNm { get; set; } = "";
        public string SupAccCd { get; set; } = "";
        public string SupAccNm { get; set; } = "";
        public string Prefix { get; set; } = "";
        public string LocCd { get; set; } = "";
    }

}
