using API.Extensions;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

/// <summary>
/// Attribute that ensures the user is a member of the group before executing the action.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class RequireGroupMembershipAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Checks if the user is authorized to access the group.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <param name="context">The context for the action being executed.</param>
    /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var groupId = int.Parse(context.RouteData.Values[nameof(Group.Id)]!.ToString()!);
        var userId = context.HttpContext.User.GetUserId();
        var authService = context.HttpContext.RequestServices.GetRequiredService<IGroupAuthorizationService>();

        await authService.EnsureMembershipAsync(groupId, userId);

        _ = await next();
    }
}
