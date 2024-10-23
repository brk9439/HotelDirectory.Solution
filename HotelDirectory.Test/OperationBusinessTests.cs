using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Business.Business;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using HotelDirectory.Shared.Common;
using HotelDirectory.Shared.ElasticSearch.Model;
using HotelDirectory.Shared.ElasticSearch;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using static HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Tests
{
    public class OperationBusinessTests
    {
        private readonly Mock<IElasticSearchLogger<GenericLogModel>> _mockLogger;
        private readonly HotelDbContext _dbContext;
        private readonly OperationBusiness _operationBusiness;

        public OperationBusinessTests()
        {
            _mockLogger = new Mock<IElasticSearchLogger<GenericLogModel>>();

            // InMemory veritabanı oluşturma
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseNpgsql("Server=localhost;Port=5435;Database=hoteldb;User Id=hotel_user;Password=hotel_password")
                .Options;

            _dbContext = new HotelDbContext(options);
            _operationBusiness = new OperationBusiness(_dbContext, _mockLogger.Object);
        }

        [Theory]
        [InlineData("Otel A", "Ali", "Yılmaz")]
        public async Task CreateHotel_ShouldReturnSuccess(string companyName, string personName, string personSurname)
        {
            var request = new CreateHotelRequest
            {
                CompanyName = companyName,
                PersonName = personName,
                PersonSurname = personSurname
            };

            var result = await _operationBusiness.CreateHotel(request);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(ResponseMessageConst.HotelCreatedSuccessMessage, result.Message);
        }

        [Theory]
        [InlineData("70b1d856-9d3b-4dd8-aa51-8a7f6cce3799")] // Geçersiz bir otel ID veriniz
        public async Task RemoveHotel_ShouldReturnNotFound_WhenHotelDoesNotExist(Guid hotelId)
        {
            var result = await _operationBusiness.RemoveHotel(hotelId);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(ResponseMessageConst.HotelRemovedContextNullMessage, result.Message);
        }

        [Theory]
        [InlineData("8de07f59-6720-42e4-a0d0-229151e521bc")]
        public async Task CreateContact_ShouldReturnSuccess(Guid hotelId)
        {

            var contactRequest = new CreateContactRequest
            {
                HotelId = hotelId,
                InfoType = ContactInfoType.MailAddress,
                InfoContent = "otel@a.com"
            };

            var result = await _operationBusiness.CreateContact(contactRequest);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(ResponseMessageConst.ContactCreatedSuccessMessage, result.Message);
        }

        [Theory]
        [InlineData("70b1d856-9d3b-4dd8-aa51-8a7f6cce3784")] 
        public async Task RemoveContact_ShouldReturnNotFound_WhenContactDoesNotExist(Guid contactId)
        {
            var result = await _operationBusiness.RemoveContact(contactId);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(ResponseMessageConst.ContactRemovedContextNullMessage, result.Message);
        }

        [Theory]
        [InlineData("70b1d856-9d3b-4dd8-aa51-8a7f6cce3784")] // Geçerli bir otel ID'si ile değiştirin
        public async Task GetHotelInfo_ShouldReturnHotelInfo_WhenHotelExists(Guid hotelId)
        {
            var result = await _operationBusiness.GetHotelInfo(hotelId);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull(result.Data); 
        }

        [Theory]
        [InlineData("8de07f59-6720-42e4-a0d0-229151e52199")] // Geçersiz bir otel ID veriniz.
        public async Task GetHotelInfo_ShouldReturnNotFound_WhenHotelDoesNotExist(Guid hotelId)
        {
            var result = await _operationBusiness.GetHotelInfo(hotelId);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(ResponseMessageConst.GetHotelInfoNullMessage, result.Message);
        }

        [Theory]
        [InlineData("8de07f59-6720-42e4-a0d0-229151e521bc")] // Geçerli bir otel ID'si ile değiştirin
        public async Task GetDetailInfo_ShouldReturnDetailInfo_WhenHotelExists(Guid hotelId)
        {
            
            var result = await _operationBusiness.GetDetailInfo(hotelId);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.NotNull( result.Data); 
        }

        [Theory]
        [InlineData("70b1d856-9d3b-4dd8-aa51-8a7f6cce3799")]
        public async Task GetDetailInfo_ShouldReturnNotFound_WhenHotelDoesNotExist(Guid hotelId)
        {
            var result = await _operationBusiness.GetDetailInfo(hotelId);

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal(ResponseMessageConst.GetDetailInfoNullMessage, result.Message);
        }
    }
}