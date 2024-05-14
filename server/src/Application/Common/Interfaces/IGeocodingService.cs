using Application.Common.Models;

namespace Application.Common.Interfaces;

public interface IGeocodingService
{
    Task<LocationDetailsDto> GetLocationDetailsAsync(string location);
}