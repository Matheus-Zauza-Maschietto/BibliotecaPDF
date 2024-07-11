using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace bibliotecaPDF.Services.Interfaces;

public interface IJsonWebTokenService{
    string GerarToken(ClaimsIdentity ClaimsParaToken);
    string GerarRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}
