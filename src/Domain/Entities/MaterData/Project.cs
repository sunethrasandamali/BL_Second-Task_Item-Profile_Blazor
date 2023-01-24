﻿using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL10.CleanArchitecture.Domain.Entities.MaterData
{
    public class Project
    {
        public long ObjectKey { get; set; }
        public long ProjectKey { get; set; }
        public string? ProjectNumber { get; set; }
        public string? ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public CodeBaseResponse ProjectType { get; set; } = new CodeBaseResponse();
        public int ParentKey { get; set; }
        public string? ParentProjectID { get; set; }
        public string? ParentProjectName { get; set; }
        public CodeBaseResponse ProjectStatus { get; set; } = new CodeBaseResponse();
        public int IsActive { get; set; }
        public int IsApproved { get; set; }
        public int IsAllowTransaction { get; set; }
        public int IsParent { get; set; }
        public int IsDefault { get; set; }
        public DateTime ProjectStartDate { get; set; } = DateTime.Now;
        public DateTime ProjectEndDate { get; set; } = DateTime.Now;
        public ItemResponse Item { get; set; } = new ItemResponse();
        public string? Alias { get; set; }
        public AddressResponse Address { get; set; } = new AddressResponse();
        public AccountResponse Account { get; set; } = new AccountResponse();
        public CodeBaseResponse BusinessUnit { get; set; } = new CodeBaseResponse();
        public CodeBaseResponse Location { get; set; } = new CodeBaseResponse();

        public string? YourReference { get; set; }


    }
}
