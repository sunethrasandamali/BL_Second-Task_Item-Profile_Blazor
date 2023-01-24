﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlueLotus360.Data
{
    public partial class User
    {

        private string _password;
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get { 
            return _password;
            }
            set
            {
                _password = value;
            }
        }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }        
        public int PubId { get; set; }
        public DateTime? HireDate { get; set; }

        public Role Role { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
