using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WebServer.Data;
using WebServer.Models.Entities;

public class CheckUserStatusAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var db = context.HttpContext.RequestServices
            .GetService<ApplicationDBContext>();

        var email = context.HttpContext.User.Identity?.Name;

        if (email == null)
        {
            context.Result = new RedirectToActionResult("Add", "User", null);
            return;
        }

        var user = db.Users.FirstOrDefault(u => u.Email == email);

        if (user == null || user.Status == UserStatus.Blocked)
        {
            context.Result = new RedirectToActionResult("Add", "User", null);
        }
    }
}