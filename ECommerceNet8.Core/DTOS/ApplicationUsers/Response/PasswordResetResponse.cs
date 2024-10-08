﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceNet8.Core.DTOS.ApplicationUsers.Response
{
    public class PasswordResetResponse
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; }
        public List<string>  Errors { get; set; } = new List<string>();
    }
}
