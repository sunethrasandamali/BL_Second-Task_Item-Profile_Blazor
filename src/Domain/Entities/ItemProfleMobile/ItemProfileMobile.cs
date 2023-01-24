using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile
{
    public class ItemSelectListRequest
    {
        public long ElementKey { get; set; }

        public int ItmTypKy { get; set; } = 1;

        //public int Dept { get; set; }

        //public int Cat { get; set; }

        //public int FrmRow { get; set; }

        //public int ToRow { get; set; }

        //public byte OnlyisAct { get; set; }

        //public string ItmCd { get; set; }

        //public string ItmNm { get; set; }

        public ItemSelectListRequest()
        {
            //Dept = 1;
            //Cat = 1;
            //FrmRow = 0;
            //ToRow = 999999;
            //OnlyisAct = 1;
            //ItmCd = string.Empty;
            //ItmNm = string.Empty;
        }
    }

    public class ItemSelectList: BaseEntity
    {
        public int ItmKy { get; set; }

        public string ItemName { get; set; }

        public string ItemCode { get; set; }
        public CodeBaseResponse ItemType { get; set; } = new CodeBaseResponse();
        public UnitResponse ItemUnit { get; set; } = new UnitResponse();
        public ItemResponse ParentItem { get; set; } = new ItemResponse();
        public bool IsAct { get; set; }
        public bool IsApprove { get; set; }
        public bool IsInEditMode { get; set; }

        public ItemSelectList()
        {
            this.ItmKy = 0;
            this.ItemCode = string.Empty;
            this.ItemName = string.Empty;
            this.ItemType = new CodeBaseResponse();
            this.ItemUnit = new UnitResponse();
            this.ParentItem = new ItemResponse();
            this.IsAct = true;
            this.IsApprove = true;
        }

        public void CopyFrom(ItemSelectList source)
        {
            source.CopyProperties(this);
        }
    }
}
