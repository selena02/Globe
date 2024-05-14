using Application.Common.Interfaces;
using Application.Common.Models;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Infrastructure.Services
{
    public class OpenCageGeocodingService : IGeocodingService
    {
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.opencagedata.com/geocode/v1/json";

        public OpenCageGeocodingService(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
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

            var client = new RestClient(_baseUrl);
            var request = new RestRequest();
            request.AddParameter("q", location);
            request.AddParameter("key", _apiKey);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                return new LocationDetailsDto
                {
                    Errors = "Failed to get coordinates"
                };
            }

            try
            {
                var data = JObject.Parse(response.Content);
                var firstResult = data["results"]?.FirstOrDefault();
                if (firstResult == null)
                {
                    return new LocationDetailsDto
                    {
                        Errors = "No results found for the specified location"
                    };
                }

                var latitude = (double?)firstResult["geometry"]?["lat"];
                var longitude = (double?)firstResult["geometry"]?["lng"];
                var formattedAddress = (string)firstResult["formatted"];
                var country = (string)firstResult["components"]?["country"];
                var city = (string)firstResult["components"]["city"] ?? (string)firstResult["components"]["town"];

                if (latitude == null || longitude == null)
                {
                    return new LocationDetailsDto
                    {
                        Errors = "Invalid coordinates returned"
                    };
                }

                return new LocationDetailsDto
                {
                    Latitude = latitude.Value,
                    Longitude = longitude.Value,
                    FormattedAddress = formattedAddress,
                    Country = country,
                    City = city,
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
    }
}
