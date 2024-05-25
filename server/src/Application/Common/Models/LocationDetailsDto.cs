﻿namespace Application.Common.Models;

public class LocationDetailsDto
{
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? CountryCode { get; set; }
    public string? Errors { get; set; } 
}