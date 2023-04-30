using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    //Table Per Hierarchy
    public class InvoiceFile : FileEntity
    {
        public decimal Price { get; set; }
    }
}
