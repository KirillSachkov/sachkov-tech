using Microsoft.AspNetCore.Authorization;

namespace SachkovTech.Web.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    public PermissionRequirementHandler()
    {
        
    }
    
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        context.Succeed(permission);
        //success
    }
}