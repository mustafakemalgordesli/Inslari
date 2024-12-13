﻿using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class ValidationException : BadRequestException
{
        public ValidationException(Dictionary<string, string[]> errors)
            : base("Validation errors occurred") =>
            Errors = errors;

    public Dictionary<string, string[]> Errors { get; }
}

