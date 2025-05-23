﻿using Shared.Enums.ErrorCodes.Auth;

namespace Application.Exceptions;

/// <summary>
/// Represents an exception thrown when the provided credentials are invalid.
/// </summary>
public class InvalidCredentialsException()
    : AppException(400, LoginErrorCode.INVALID_CREDENTIALS);
