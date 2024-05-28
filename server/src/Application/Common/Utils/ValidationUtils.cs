using Microsoft.AspNetCore.Http;

namespace Application.Common.Utils;

public static class ValidationUtils
{
    public static bool IsValidFullName(string? fullName)
    {
        var parts = fullName?.Split(' ');
        if (parts?.Length is < 2 or > 4)
            return false; 
        foreach (var part in parts?.Where(part => part.Length > 0) ?? Array.Empty<string>())
        {
            if (part.Length is < 2 or > 15)
                return false; 
        }
        return true;
    }
    
    public static string ApplySimpleCapitalization(string input)
    {
        var words = input.Trim().ToLower().Split(' ');
        for (var i = 0; i < words.Length; i++)
        {
            if (!string.IsNullOrEmpty(words[i]))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1);
            }
        }
        return string.Join(" ", words);
    }
    
    public static bool IsAJpegOrPng(IFormFile? file)
    {
        if (file is not null)
        {
            return file.ContentType is "image/jpeg" or "image/png";
        }
        
        return true;
    }
}