using DiscographyViewerAPI.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection.Metadata;
using DiscographyViewerAPI.Models.Dto;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Runtime.CompilerServices;
using FluentAssertions;
using DiscographyViewerAPI.Models.ViewModels;

namespace MockClientTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task GetCorrectDiscography_GetExpectedResults()
        {       
            // Arrange
            var mockHandler = new Mock<HttpClientHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"album\":[{\"strAlbum\":\"The Fear of Fear\",\"intYearReleased\":\"2023\"},{\"strAlbum\":\"Rotoscope\",\"intYearReleased\":\"2022\"},{\"strAlbum\":\"Eternal Blue\",\"intYearReleased\":\"2021\"}]}")
                });
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            DiscographyService service = new DiscographyService(mockClient);

            // Act           

            var result = await service.GetDiscographyAsync("");

            DiscographyViewModel newResult = new DiscographyViewModel()
            {
                Album = result.Album.Select(a => new AlbumViewModel()
                {
                    Name = a.StrAlbum,
                    YearReleased = a.IntYearReleased,
                }).ToList(),
            };

            // Assert

            Assert.AreEqual(newResult.Album[0].Name, "The Fear of Fear");
            //Assert.AreEqual(3, result.Album.Count);

        }


        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetCorrectDiscography_GetException()
        {
            // Arrange
            var mockHandler = new Mock<HttpClientHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError                
                });
            HttpClient mockClient = new HttpClient(mockHandler.Object);
            DiscographyService service = new DiscographyService(mockClient);

            // Act           

            await service.GetDiscographyAsync("");

           

        }

    }
}