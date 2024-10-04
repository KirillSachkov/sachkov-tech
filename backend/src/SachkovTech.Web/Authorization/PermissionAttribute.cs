using Microsoft.AspNetCore.Authorization;

namespace SachkovTech.Web.Authorization;

public class PermissionAttribute : AuthorizeAttribute, IAuthorizationRequirement
{
    public string Code { get; set; }

    public PermissionAttribute(string code)
        : base(policy: code)
    {
        Code = code;
    }
}