#nullable disable
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Master.API.Controllers
{
    public class GenericController : ControllerBase
    {
        protected string CurrentUserName;
        protected int currentUserId;

        protected void GetCurrentUserNameFromClaim()
        {
            CurrentUserName = User?.FindFirstValue(ClaimTypes.Email)
                ?? User?.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
        }

        protected void GetCurrentUserIdFromClaim()
        {
            var idStr = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(idStr, out currentUserId);
        }
    }
}
