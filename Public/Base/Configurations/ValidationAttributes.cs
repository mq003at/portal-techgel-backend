using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace portal.DTOs.Validations;

[AttributeUsage(AttributeTargets.Property)]
public class MaxFileSizeAttribute : ValidationAttribute
{
    private readonly int _maxBytes;

    public MaxFileSizeAttribute(int maxBytes) => _maxBytes = maxBytes;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is IFormFile file && file.Length > _maxBytes)
            return new ValidationResult(
                ErrorMessage ?? $"Maximum allowed file size is {_maxBytes / (1024 * 1024)} MB."
            );
        return ValidationResult.Success;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class AllowedExtensionsAttribute : ValidationAttribute
{
    private readonly string[] _extensions;

    public AllowedExtensionsAttribute(string[] extensions) => _extensions = extensions;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        if (value is IFormFile file)
        {
            var ext = System.IO.Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || Array.IndexOf(_extensions, ext.ToLower()) < 0)
                return new ValidationResult(ErrorMessage ?? $"This file extension is not allowed.");
        }
        return ValidationResult.Success;
    }
}
