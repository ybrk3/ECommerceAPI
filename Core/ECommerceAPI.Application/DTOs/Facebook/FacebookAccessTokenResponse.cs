using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Facebook
{
    public class FacebookAccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }
    }
}

//Return value from facebook
//{ "access_token":"649246677025543|Dpl4QwHzcsSVXa3-7dq2_yCdFgc","token_type":"bearer"}