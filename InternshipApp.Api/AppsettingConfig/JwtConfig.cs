using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipApp.Api.AppsettingConfig
{
    public class JwtConfig
    {
        public string JWT_Secret { get; set; } = string.Empty;
	    public string Issuer { get; set; } = string.Empty;
	    public string Audience { get; set; } = string.Empty;
    }
}
