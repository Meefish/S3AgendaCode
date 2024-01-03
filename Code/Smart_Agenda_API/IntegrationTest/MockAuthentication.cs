using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (authHeader == null)
            {
                return await System.Threading.Tasks.Task.FromResult(AuthenticateResult.NoResult());
            }

            var role = Enum.TryParse<UserRole>(authHeader, out var parsedRole) ? parsedRole : UserRole.User;

            var principal = CreatePrincipalForUser(role);

            return await System.Threading.Tasks.Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
             new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
         AuthenticateResult authenticationResult, HttpContext context, object resource)
        {

            if (!authenticationResult.Succeeded)
            {
                return await System.Threading.Tasks.Task.FromResult(PolicyAuthorizationResult.Challenge());
            }

            if (authenticationResult.Succeeded)
            {
                var userPrincipal = authenticationResult.Principal;
                var userRoles = userPrincipal.Claims
                    .Where(c => c.Type == ClaimTypes.Role)
                    .Select(c => c.Value)
                    .ToList();

                var hasRequiredRole = policy.Requirements
                    .OfType<RolesAuthorizationRequirement>()
                    .Any(requirement => requirement.AllowedRoles.Any(role => userRoles.Contains(role)));

                if (hasRequiredRole)
                {
                    return await System.Threading.Tasks.Task.FromResult(PolicyAuthorizationResult.Success());
                }
            }

            return await System.Threading.Tasks.Task.FromResult(PolicyAuthorizationResult.Forbid());
        }

        private ClaimsPrincipal CreatePrincipalForUser(UserRole role)
        {
            int calendarId = 1;
            User user = new User
            {
                UserId = 1,
                Username = "test",
                Email = "user@example.com",
                UserRole = role,
                PasswordHash = "$11$P/JQHRlgEei3UB3DKBeg3OXXVy1lWr1/KS8ISQzhNpldi6Cfc9qQ2"
            };

            var identity = new ClaimsIdentity(new[]
            {
            new Claim("userId", user.UserId.ToString()),
            new Claim("username", user.Username),
            new Claim("email", user.Email),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim("calendarId", calendarId.ToString())
        }, "FakeScheme");

            return new ClaimsPrincipal(identity);
        }
    }
}