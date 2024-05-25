using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class GeoCodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeoCodingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["GEOCODER_API_KEY"] ?? throw new ServerErrorException("Geocoder API key not found");
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
                var data = JsonSerializer.Deserialize<OpenCageResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
                    Country = firstResult.Components.Country,
                    City = firstResult.Components.City ?? firstResult.Components.Town,
                    CountryCode = firstResult.Components.Country_Code
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
        
        public class OpenCageResponse
        {
            public Result[] Results { get; set; }
            public Rate Rate { get; set; }
            public Status Status { get; set; }
            public int TotalResults { get; set; }
        }

        public class Result
        {
            public Annotations Annotations { get; set; }
            public Bounds Bounds { get; set; }
            public Components Components { get; set; }
            public int Confidence { get; set; }
            public string Formatted { get; set; }
            public Geometry Geometry { get; set; }
        }

        public class Annotations
        {
           
        }

        public class Bounds
        {
            public Northeast Northeast { get; set; }
            public Southwest Southwest { get; set; }
        }

        public class Northeast
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Southwest
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Components
        {
            public string ISO_3166_1_alpha_2 { get; set; }
            public string ISO_3166_1_alpha_3 { get; set; }
            public string Category { get; set; }
            public string City { get; set; }
            public string Country { get; set; }
            public string Country_Code { get; set; }
            public string HouseNumber { get; set; }
            public string Neighbourhood { get; set; }
            public string Postcode { get; set; }
            public string Road { get; set; }
            public string State { get; set; }
            public string Suburb { get; set; }
            public string Town { get; set; }
        }

        public class Geometry
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class Rate
        {
            public int Limit { get; set; }
            public int Remaining { get; set; }
            public int Reset { get; set; }
        }

        public class Status
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

    }
}
