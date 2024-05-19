using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class GeoCodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeoCodingService(HttpClient httpClient, IConfiguration configuration, ILogger<GeoCodingService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["GeocoderApiKey"] ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<LocationDetailsDto> GetLocationDetailsAsync(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
            {
                return new LocationDetailsDto
                {
                    Errors = "Location cannot be empty"
                };
            }

            var requestUrl = $"https://api.opencagedata.com/geocode/v1/json?q={Uri.EscapeDataString(location)}&key={_apiKey}";
            
            try
            {
                var response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return new LocationDetailsDto
                    {
                        Errors = $"Failed to get coordinates, status code: {response.StatusCode}"
                    };
                }

                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var data = JsonSerializer.Deserialize<OpenCageResponse>(json);

                if (data == null || data.Results.Length == 0)
                {
                    return new LocationDetailsDto
                    {
                        Errors = "No results found for the specified location"
                    };
                }

                var firstResult = data.Results[0];
                return new LocationDetailsDto
                {
                    Latitude = firstResult.Geometry.Lat,
                    Longitude = firstResult.Geometry.Lng,
                    FormattedAddress = firstResult.Formatted,
                    Country = firstResult.Components.Country,
                    City = firstResult.Components.City ?? firstResult.Components.Town
                };
            }
            catch (Exception ex)
            {
                return new LocationDetailsDto
                {
                    Errors = $"Error parsing response: {ex.Message}"
                };
            }
        }

        private class OpenCageResponse
        {
            public Result[] Results { get; set; }
        }

        private class Result
        {
            public Geometry Geometry { get; set; }
            public string Formatted { get; set; }
            public Components Components { get; set; }
        }

        private class Geometry
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        private class Components
        {
            public string Country { get; set; }
            public string City { get; set; }
            public string Town { get; set; }
        }
    }
}
