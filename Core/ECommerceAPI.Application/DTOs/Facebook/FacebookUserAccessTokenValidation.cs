using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Facebook
{
    public class FacebookUserAccessTokenValidation
    {
        [JsonPropertyName("data")]
        public FacebookUserAccessTokenValidationData Data { get; set; }
    }
    public class FacebookUserAccessTokenValidationData
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
//{"data":{"app_id":"649246677025543","type":"USER","application":"ECommerce App","data_access_expires_at":1692189090,"expires_at":1684418400,"is_valid":true,"scopes":["email","public_profile"],"user_id":"10160488612831585"}}