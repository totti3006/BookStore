using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _allowRole;

        public CustomAuthorizeAttribute(string allowRole)
        {
            _allowRole = allowRole;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isInRole = context.HttpContext.User.IsInRole(_allowRole);

            if (isInRole == false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }
    }
}
