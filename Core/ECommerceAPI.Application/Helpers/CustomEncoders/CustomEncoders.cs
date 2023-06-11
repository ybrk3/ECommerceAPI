using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Helpers.CustomEncoders
{
    public static class CustomEncoders
    {
        public static string UrlEncode(this string value)
        {
            //however this token can include non-proper chars for link url, so we are encoding it
            byte[] tokenBytes = Encoding.UTF8.GetBytes(value); //convert it to byte
            return WebEncoders.Base64UrlEncode(tokenBytes); //encoding it

        }
        public static string UrlDecode(this string value)
        {
            //convert resetToken to byte array and decode it
            byte[] resetTokenByte = WebEncoders.Base64UrlDecode(value);
            //convert byte resetToken to string
            return Encoding.UTF8.GetString(resetTokenByte);

        }
    }
}
