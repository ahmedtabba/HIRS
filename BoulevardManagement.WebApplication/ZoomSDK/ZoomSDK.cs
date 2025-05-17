using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Packages:
/// System.IdentityModel.Tokens.Jwt
/// </summary>
namespace ZoomAPI
{
    public class ZoomSDK
    {
        public static string ZoomToken(string ApiKey, string ApiSecret)
        {
            // Token will be good for 20 minutes
            DateTime Expiry = DateTime.UtcNow.AddMinutes(20);
            int ts = (int)(Expiry - new DateTime(1970, 1, 1)).TotalSeconds;

            // Create Security key  using private key above:
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApiSecret));

            // length should be >256b
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            var header = new JwtHeader(credentials);

            //Zoom Required Payload
            var payload = new JwtPayload
                {
                    { "iss", ApiKey},
                    { "exp", ts },
                };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();

            // Token to String so you can use it in your client
            var tokenString = handler.WriteToken(secToken);

            return tokenString;
        }

        public static MeetingResponse CreateMeeting(string token, CreateMeeting meeting)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.zoom.us/v2/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(meeting), Encoding.UTF8, "application/json");
                var result = client.PostAsync(@"users/me/meetings", content).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;
                    var meetingResponse = JsonConvert.DeserializeObject<MeetingResponse>(resultContent);
                    return meetingResponse;
                }
            }

            return new MeetingResponse();
        }
    }
}
