using System;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Application.Controllers;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using HotelDirectory.Hotel.Service.Business.Business;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using static HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Tests
{
    public class OperationControllerTests
    {
        private readonly Mock<IOperationBusiness> _operationBusinessMock;
        private readonly OperationController _controller;

        public OperationControllerTests()
        {
            _operationBusinessMock = new Mock<IOperationBusiness>();
            _controller = new OperationController(_operationBusinessMock.Object);
        }

        [Theory]
        [InlineData("ONAL HOTEL", "Burak", "Önal", "Kayıt başarılı")]
        public async Task CreateHotel_ShouldReturnOk_WhenSuccessful(string companyName, string personName, string personSurname, string expectedMessage)
        {
            var request = new CreateHotelRequest
            {
                CompanyName = companyName,
                PersonName = personName,
                PersonSurname = personSurname
            };

            _operationBusinessMock
            .Setup(x => x.CreateHotel(request))
            .ReturnsAsync(expectedMessage);

            var result = await _controller.CreateHotel(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedMessage, okResult.Value);
        }


        [Theory]
        [InlineData("a1d5d279-b164-4ecb-8f21-3107c6e97994")]
        public async Task RemoveHotel_ShouldReturnOk_WhenSuccessful(Guid guid)
        {
            _operationBusinessMock
            .Setup(x => x.RemoveHotel(guid))
            .ReturnsAsync("Hotel kaldırıldı.");

            var result = await _controller.RemoveHotel(guid);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Hotel kaldırıldı.", okResult.Value);
        }

        [Fact]
        public async Task RemoveHotel_ShouldReturnOk_WhenNotFound()
        {
            var hotelId = Guid.NewGuid();
            _operationBusinessMock
            .Setup(x => x.RemoveHotel(hotelId))
            .ReturnsAsync("İlgili hotel bulunamadı");

            var result = await _controller.RemoveHotel(hotelId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("İlgili hotel bulunamadı", okResult.Value);
        }

        [Theory]
        [InlineData(ContactInfoType.MailAddress, "onalhotel@test.com", "Kayıt başarılı")]
        public async Task CreateContact_ShouldReturnOk_WhenSuccessful(ContactInfoType infoType, string infoContent, string expectedMessage)
        {
            var request = new CreateContactRequest
            {
                InfoType = infoType,
                InfoContent = infoContent
            };

            _operationBusinessMock
            .Setup(x => x.CreateContact(request))
            .ReturnsAsync(expectedMessage);

            var result = await _controller.CreateContact(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedMessage, okResult.Value);
        }

        [Theory]
        [InlineData("2d3c786b-6883-4c5b-8db9-02366761fb3f")]
        public async Task RemoveContact_ShouldReturnOk_WhenSuccessful(Guid guid)
        {
            _operationBusinessMock
            .Setup(x => x.RemoveContact(guid))
            .ReturnsAsync("Başarılı");

            var result = await _controller.RemoveContact(guid);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Başarılı", okResult.Value);
        }

        [Fact]
        public async Task RemoveContact_ShouldReturnOk_WhenNotFound()
        {
            var contactId = Guid.NewGuid();
            _operationBusinessMock
            .Setup(x => x.RemoveContact(contactId))
            .ReturnsAsync("İlgili iletişim bilgisi bulunamadı");

            var result = await _controller.RemoveContact(contactId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("İlgili iletişim bilgisi bulunamadı", okResult.Value);
        }

        [Theory]
        [InlineData("9682d0e9-5e82-4be3-9803-33edfb775bcb")]
        public async Task GetDetailInfo_ShouldReturnOkWhenSuccessful(Guid guid)
        {
            var hotelId = Guid.NewGuid();
            _operationBusinessMock
            .Setup(x => x.GetDetailInfo(guid))
            .ReturnsAsync(new { HotelId = hotelId, CompanyName = "Test Company" });

            var result = await _controller.GetDetailInfo(guid);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

    }
}