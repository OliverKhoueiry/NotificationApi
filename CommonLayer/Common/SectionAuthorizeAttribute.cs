using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class SectionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string _section;
    private readonly string _action;

    public SectionAuthorizeAttribute(string section, string action)
    {
        _section = section;
        _action = action;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userClaims = context.HttpContext.User.Claims;

        var hasSection = userClaims.Any(c => c.Type == "section" && c.Value == _section);
        var hasAction = userClaims.Any(c => c.Type == $"action:{_section}" && c.Value == _action);

        if (!hasSection || !hasAction)
        {
            context.Result = new ForbidResult();
        }
    }
}
