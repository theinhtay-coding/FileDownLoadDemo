using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FileDownLoadDemo.Services
{
    public class ValidateHeadersAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            // Check for Content-Type header
            if (!request.Headers.TryGetValue("Content-Type", out var contentType) || contentType != "application/x-www-form-urlencoded")
            {
                context.Result = new BadRequestObjectResult(new { Message = "Missing or invalid Content-Type header." });
                return;
            }

            // Check for Authorization header
            if (!request.Headers.TryGetValue("Authorization", out var authorization) || !authorization.ToString().StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Missing or invalid Authorization header." });
                return;
            }

            // Check for X-NOM-Mobile header
            if (!request.Headers.TryGetValue("X-NOM-Mobile", out var xNomMobile) || xNomMobile != "true")
            {
                context.Result = new BadRequestObjectResult(new { Message = "Missing or invalid X-NOM-Mobile header." });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
