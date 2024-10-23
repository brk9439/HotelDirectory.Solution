using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using HotelDirectory.Reporting.Service.Business.Business;
using HotelDirectory.Reporting.Service.Business.Configuration;
using HotelDirectory.Reporting.Service.Business.Model.Request;
using HotelDirectory.Reporting.Service.Business.Model.Response;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Context;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Entities;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;
using HotelDirectory.Shared.Common;
using HotelDirectory.Shared.ElasticSearch;
using HotelDirectory.Shared.ElasticSearch.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Moq;
using Enum = HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Test
{
    public class ReportOperationBusinessTests
    {
        private readonly Mock<IQueueOperation> _mockQueueOperation;
        private readonly Mock<IElasticSearchLogger<GenericLogModel>> _mockLogger;
        private readonly Mock<IConfiguration> _mockConfiguration; 
        private readonly HotelDbContext _mockDbContext;
        private readonly ReportOperationBusiness _reportOperationBusiness;

        public ReportOperationBusinessTests()
        {
            _mockQueueOperation = new Mock<IQueueOperation>();
            _mockLogger = new Mock<IElasticSearchLogger<GenericLogModel>>();
            _mockConfiguration = new Mock<IConfiguration>(); 
            // InMemory veritabanı kullanarak DbContext'i oluştur
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                 .UseNpgsql("Server=localhost;Port=5435;Database=hoteldb;User Id=hotel_user;Password=hotel_password").Options;

            _mockDbContext = new HotelDbContext(options);
            // Mock IConfiguration değerlerini ayarla
            SetupConfigurationMock();
            var configManager = new ConfigManager(_mockConfiguration.Object);
            _reportOperationBusiness = new ReportOperationBusiness(_mockDbContext, _mockQueueOperation.Object, _mockLogger.Object, configManager);


        }
        private void SetupConfigurationMock()
        {
            _mockConfiguration.Setup(c => c["ReportQueueMessageConfiguration:QueueName"]).Returns("test_queue");
            _mockConfiguration.Setup(c => c["ReportQueueMessageConfiguration:ExchangeName"]).Returns("test_exchange");
            _mockConfiguration.Setup(c => c["ReportQueueMessageConfiguration:RoutingKeyName"]).Returns("test_routing_key");
            _mockConfiguration.Setup(c => c["ReportQueueMessageConfiguration:MessageTtl"]).Returns("30000"); // 30 saniye
        }

        [Fact]
        public async Task CreateReport_InvalidLocation_ReturnsNotFound()
        {
            var location = "Invalid Location";

            var result = await _reportOperationBusiness.CreateReport(location);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task GetListReport_ReturnsSuccess()
        {
            _mockDbContext.ReportingInfo.Add(new ReportingInfo
            {
                Status = Reporting.Service.Infrastructure.Data.Entities.Enum.ReportStatus.Completed,
                HotelCount = 5,
                PhoneCount = 10,
                Location = "Test Location",
                GetDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            });
            await _mockDbContext.SaveChangesAsync();

            var result = await _reportOperationBusiness.GetListReport();

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.True(((IEnumerable<ReportResponse>)result.Data).Any());
        }

        [Fact]
        public async Task GetReport_ExistingReport_ReturnsSuccess()
        {
            var reportInfo = new ReportingInfo
            {
                Status = Reporting.Service.Infrastructure.Data.Entities.Enum.ReportStatus.Completed,
                HotelCount = 5,
                PhoneCount = 10,
                Location = "Test Location",
                GetDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };
            _mockDbContext.ReportingInfo.Add(reportInfo);
            await _mockDbContext.SaveChangesAsync();

            var result = await _reportOperationBusiness.GetReport(reportInfo.Id);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetReport_NonExistingReport_ReturnsNotFound()
        {
            var result = await _reportOperationBusiness.GetReport(Guid.NewGuid());
            
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
