using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.API.Security;

public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if(context.User == null)
            return Task.CompletedTask;
        
        bool hasPermission = context.User.Claims.Any(c =>
            c.Type == "Permission" && c.Value == requirement.Permission);

        if(hasPermission)
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
}