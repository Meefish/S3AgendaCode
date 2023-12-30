using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Smart_Agenda_Logic.Domain;
using System.Security.Claims;

namespace IntegrationTest
{
    public class MockAuthentication : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();
            int calendarId = 1;
            User user = new User
            {
                UserId = 1,
                Username = "test",
                Email = "user@example.com",
                UserRole = UserRole.User,
                PasswordHash = "$11$P/JQHRlgEei3UB3DKBeg3OXXVy1lWr1/KS8ISQzhNpldi6Cfc9qQ2"
            };

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim("userId", user.UserId.ToString()),
                new Claim("username", user.Username),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim("calendarId", calendarId.ToString())
            }, "FakeScheme"));

            return await System.Threading.Tasks.Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
             new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
         AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await System.Threading.Tasks.Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }
}