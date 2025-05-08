using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Shared.Enums.ErrorCodes;
using Shared.Enums.ErrorCodes.Auth;

namespace API.Conventions;

/// <summary>
/// Applies a <c>401 Unauthorized</c> Swagger response type to all controller actions
/// that require authorization, unless they explicitly allow anonymous access or already define the response.
/// </summary>
/// <remarks>
/// This convention ensures consistent documentation of authorization errors across protected endpoints
/// by injecting a <see cref="ProducesResponseTypeAttribute"/> for <see cref="StatusCodes.Status401Unauthorized"/>.
/// </remarks>
public class UnauthorizedProducesResponseConvention : IApplicationModelConvention
{
    /// <inheritdoc/>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                var allowsAnonymous = controller.Attributes.OfType<AllowAnonymousAttribute>().Any()
                                     || action.Attributes.OfType<AllowAnonymousAttribute>().Any();

                var alreadyDeclared = action.Filters.OfType<ProducesResponseTypeAttribute>()
                    .Any(attr => attr.StatusCode == StatusCodes.Status401Unauthorized);

                if (!allowsAnonymous && !alreadyDeclared)
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute(
                        typeof(ErrorResponse<AuthErrorCode>),
                        StatusCodes.Status401Unauthorized));
                }
            }
        }
    }
}
