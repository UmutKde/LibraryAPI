using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.API.Security;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission {get;}
    public PermissionRequirement(string permission) => Permission = permission;
}