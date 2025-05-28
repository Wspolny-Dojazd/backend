using API.Extensions;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

/// <summary>
/// Attribute that ensures the user is an owner of the group before executing the action.
/// </summary>
/// <param name="routeKey">The key used to retrieve the group ID from the route data.</param>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
public class RequireGroupOwnershipAttribute(string routeKey = "id") : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Checks if the user is authorized to access the group.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <param name="context">The context for the action being executed.</param>
    /// <param name="next">The delegate to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var routeData = context.RouteData.Values;

        if (!routeData.TryGetValue(routeKey, out var rawValue) || !int.TryParse(rawValue?.ToString(), out var groupId))
        {
            throw new InvalidOperationException($"Route data does not contain a valid group ID for key {routeKey}.");
        }

        var userId = context.HttpContext.User.GetUserId();
        var authService = context.HttpContext.RequestServices.GetRequiredService<IGroupAuthorizationService>();

        await authService.EnsureOwnershipAsync(groupId, userId);

        _ = await next();
    }
}
