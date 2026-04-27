using LibrarySystem.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace LibrarySystem.Presentation.HasPermissionAttribute;

public class HasPermissionAttribute<TEntity> : AuthorizeAttribute where TEntity : class
{
    public HasPermissionAttribute(ActionType actionType)
    {
        Policy = $"{typeof(TEntity).Name}.{actionType}";
    }
}

// bu class t entity önderliğinde modellerimiz için önceden belirlediğimiz temel kuralları arkada otomatik olarak üretecek olan o sistem
// bu sistemde amaç bu kısımda hepsini ayarlamak ve kullancıya bu rolleri atama yapmak
