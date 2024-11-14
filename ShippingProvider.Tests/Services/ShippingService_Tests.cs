using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using ShippingProvider.DTO.ServicePointDTOs;
using ShippingProvider.DTO.TransitTimeDTOs;
using ShippingProvider.Services;
using System.Net;
using System.Text.Json;

namespace ShippingProvider.Tests.Services;

public class ShippingService_Tests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;

    public ShippingService_Tests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
    }

    [Fact]
    public async Task GetNearestServicePointsAsync_ReturnsServicePoints_WhenSuccessful()
    {
        // Arrange
        var postalCode = "12345";
        var numberOfServicePoints = 3;
        var expectedResponse = new ServicePointInformationResponseWrapper
        {
            ServicePointInformationResponse = new ServicePointInformationResponse
            {
                ServicePoints = new List<ServicePoints>
                {
                    new ServicePoints
                    {
                        Name = "Test ServicePoint",
                        ServicePointId = "SP1",
                        VisitingAddress = new VisitingAddress
                        {
                            CountryCode = "SE",
                            City = "Stockholm",
                            StreetName = "Main St",
                            StreetNumber = "10",
                            PostalCode = "12345"
                        }
                    }
                }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse);
        var baseUrl = "https://example.com/";

        _mockConfiguration.Setup(c => c["PostNord:BaseUrl"]).Returns(baseUrl);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri!.ToString().StartsWith(baseUrl)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var shippingService = new ShippingService(_httpClient, "secretKey", _mockConfiguration.Object);

        // Act
        var result = await shippingService.GetNearestServicePointsAsync(postalCode, numberOfServicePoints);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.ServicePointInformationResponse.ServicePoints);
        Assert.Equal("Test ServicePoint", result.ServicePointInformationResponse.ServicePoints.First().Name);
        Assert.Equal("Stockholm", result.ServicePointInformationResponse.ServicePoints.First().VisitingAddress.City);
    }

    [Fact]
    public async Task GetTransitTimesAsync_ReturnsTransitTimes_WhenSuccessful()
    {
        // Arrange
        var request = new TransitTimeRequest { DestinationPostalCode = "14142" };
        var expectedResponse = new List<TransitTimeResponse>
        {
            new TransitTimeResponse
            {
                timeOfDeparture = "08:00",
                timeOfArrival = "12:00",
                serviceInformation = new ServiceInformation { name = "Express" }
            }
        };

        var jsonResponse = JsonSerializer.Serialize(expectedResponse);
        var baseUrl = "https://example.com/";

        _mockConfiguration.Setup(c => c["PostNord:BaseUrl"]).Returns(baseUrl);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(r => r.Method == HttpMethod.Get && r.RequestUri!.ToString().StartsWith(baseUrl)),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            });

        var shippingService = new ShippingService(_httpClient, "secretKey", _mockConfiguration.Object);

        // Act
        var result = await shippingService.GetTransitTimesAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("08:00", result.First().timeOfDeparture);
        Assert.Equal("Express", result.First().serviceInformation.name);
    }

    [Fact]
    public async Task GetNearestServicePointsAsync_ThrowsException_WhenRequestFails()
    {
        // Arrange
        var postalCode = "12345";
        var numberOfServicePoints = 3;
        var baseUrl = "https://example.com/";

        _mockConfiguration.Setup(c => c["PostNord:BaseUrl"]).Returns(baseUrl);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Bad Request")
            });

        var shippingService = new ShippingService(_httpClient, "secretKey", _mockConfiguration.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            shippingService.GetNearestServicePointsAsync(postalCode, numberOfServicePoints));
        Assert.Contains("Failed to fetch service points", exception.Message);
    }

    [Fact]
    public async Task GetTransitTimesAsync_ThrowsException_WhenRequestFails()
    {
        // Arrange
        var request = new TransitTimeRequest { DestinationPostalCode = "14142" };
        var baseUrl = "https://example.com/";

        _mockConfiguration.Setup(c => c["PostNord:BaseUrl"]).Returns(baseUrl);

        _mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Server Error")
            });

        var shippingService = new ShippingService(_httpClient, "secretKey", _mockConfiguration.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            shippingService.GetTransitTimesAsync(request));
        Assert.Contains("Failed to fetch service points", exception.Message);
    }
}
