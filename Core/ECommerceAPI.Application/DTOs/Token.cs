﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs
{
    //props for created token
    public class Token
    {
        public string? AccessToken { get; set; }
        public DateTime Expiration { get; set; }
    }
}
