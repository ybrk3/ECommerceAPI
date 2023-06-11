using ECommerceAPI.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.CustomAttributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public string Menu { get; set; } //Which represents Controller
        public string Definition { get; set; } //Definition of end-point. We have created Consts for name of end-points
        public ActionType ActionType { get; set; } //Type of the end-point (deleting,reading etc.) Enum class.
    }
}
