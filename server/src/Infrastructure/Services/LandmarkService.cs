using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class LandmarkService : ILandmarkService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly JsonSerializerOptions _jsonOptions;
        
        public LandmarkService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _baseUrl = configuration["FlaskApi:BaseUrl"] ?? throw new ArgumentNullException(nameof(configuration));
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        
        public async Task<LandmarkDetectorResponse> GetLandmarkNameAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequestException("File cannot be null or empty");
            }

            using var content = new MultipartFormDataContent();
            await using var fileStream = file.OpenReadStream();
            
            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(fileContent, "image", file.FileName);

            var response = await _httpClient.PostAsync($"{_baseUrl}/predict", content).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new ServerErrorException("Problem with the landmark detection service");
            }
            
            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, _jsonOptions);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new NotFoundException(errorResponse?.Error ?? "Resource not found");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new BadRequestException(errorResponse?.Error ?? "Bad request");
                }
                else
                {
                    throw new ServerErrorException("Problem with the landmark detection service");
                }
            }
            
            LandmarkDetectorResponse landmark = JsonSerializer.Deserialize<LandmarkDetectorResponse>(responseContent, _jsonOptions);

            return landmark ?? throw new ServerErrorException("Problem with the landmark detection service");
        }

        private class ErrorResponse
        {
            public string Error { get; set; }
        }
    }
}
