using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using ExpenseApproval.Utility;
using System.Threading;
using System.Threading.Tasks;
using ExpenseApproval.Model;

namespace ExpenseApproval.Api.AuthenticationFilter
{
    public class AuthenticateUser : ActionFilterAttribute, IAuthenticationFilter
    {
        #region Private Members

        private ITraceWriter _tracer = GlobalConfiguration.Configuration.Services.GetTraceWriter();

        #endregion

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request.Headers.Authorization;
            if (req != null && req.Scheme.Equals(
                        "Bearer", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    TokenValidationParameters validationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                RequireExpirationTime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constant.EncryptionKey))
                            };
                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    SecurityToken validatedToken;
                    context.Principal = handler.ValidateToken(req.Parameter, validationParameters, out validatedToken);
                    ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                    IList<Claim> claims = claimsIdentity.Claims.ToList();
                    int employeeId = int.Parse(claims[1].Value);
                    string role = claims[0].Value;
                    context.Request.Properties.Add(Constant.EmployeeId, employeeId);
                    context.Request.Properties.Add(Constant.Role, role);
                }
                catch
                {
                    throw new ExpenseException(Constant.ErrorMessageInvalidToken);
                }
            }
            else
            {
                throw new ExpenseException(Constant.ErrorMessageMissingToken);
            }
            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}