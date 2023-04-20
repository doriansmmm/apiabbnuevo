using System.IdentityModel.Tokens.Jwt;

namespace ABBAPI.Services
{
    public class GetTokenValues
    {
        public bool validToken(string token)
        {
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(stream);
            var mail = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;

            DateTime expired = jwtSecurityToken.ValidTo;

            if(DateTime.Now > expired)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public tokenValues getTokenValues(string token)
        {
            var stream = token;
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(stream);
            var mail = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress").Value;
            var role = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
            DateTime expired = jwtSecurityToken.ValidTo;

            tokenValues tvalues = new tokenValues();
            tvalues.Mail = mail;
            tvalues.Role = role;
            tvalues.Expire = expired;

            return tvalues;
        }

        public class tokenValues
        {
            public string Mail { get; set; } = null!;
            public string Role { get; set; } = null!;
            public DateTime Expire { get; set; } 
            
        }
    }
}
