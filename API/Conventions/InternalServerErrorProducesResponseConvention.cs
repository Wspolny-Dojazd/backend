using API.Models.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace API.Conventions;

/// <summary>
/// Applies a <c>501 Internal Server Error</c> Swagger response type to all controller actions
/// unless they already define the response.
/// </summary>
/// <remarks>
/// This convention ensures consistent documentation of internal server errors across protected endpoints
/// by injecting a <see cref="ProducesResponseTypeAttribute"/> for <see cref="StatusCodes.Status500InternalServerError"/>.
/// </remarks>
public class InternalServerErrorProducesResponseConvention : IApplicationModelConvention
{
    /// <inheritdoc/>
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            foreach (var action in controller.Actions)
            {
                var alreadyDeclared = action.Filters.OfType<ProducesResponseTypeAttribute>()
                    .Any(attr => attr.StatusCode == StatusCodes.Status500InternalServerError);

                if (!alreadyDeclared)
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute(
                        typeof(ErrorResponse<InternalErrorCode>),
                        StatusCodes.Status500InternalServerError));
                }
            }
        }
    }
}
