using Shared.Enums.ErrorCodes;

namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when the provided credentials are invalid.
/// </summary>
public class InternalServerErrorException()
    : AppException(500, InternalErrorCode.INTERNAL_ERROR);
