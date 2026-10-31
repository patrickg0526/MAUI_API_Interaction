using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ItemsWebAPI.Filters
{
    // Validates an "Authorization: Basic <base64(username:password)>" header.
    // Returns 401 Unauthorized when the header is missing, malformed, or the credentials are wrong.
    public class BasicAuthentication : ActionFilterAttribute
    {
        private const string ValidUserName = "gonzalez01";   // compared case-insensitively
        private const string ValidPassword = "Password1";    // compared case-sensitively

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string header = context.HttpContext.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(header))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var headerParts = header.Split(' ');
            if (headerParts.Length != 2 || headerParts[0] != "Basic")
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string credentials;
            try
            {
                credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerParts[1]));
            }
            catch (FormatException)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Split on the first colon only, so passwords may contain ':'
            int separator = credentials.IndexOf(':');
            if (separator < 0)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string userName = credentials[..separator];
            string password = credentials[(separator + 1)..];

            if (!string.Equals(userName, ValidUserName, StringComparison.OrdinalIgnoreCase) || password != ValidPassword)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
