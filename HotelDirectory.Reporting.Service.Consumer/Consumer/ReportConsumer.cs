using HotelDirectory.Reporting.Service.Infrastructure.Data.Context;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;
using System.Text;
using System.Text.Json;
using HotelDirectory.Reporting.Service.Consumer.Configuration;
using HotelDirectory.Reporting.Service.Consumer.Model;
using HotelDirectory.Shared.ElasticSearch;
using HotelDirectory.Shared.ElasticSearch.Model;
using RabbitMQ.Client.Events;
using Enum = HotelDirectory.Reporting.Service.Infrastructure.Data.Entities.Enum;
using HotelDirectory.Shared.Common;
using Type = HotelDirectory.Shared.ElasticSearch.Model.Type;

namespace HotelDirectory.Reporting.Service.Consumer.Consumer
{
    public class ReportConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IQueueOperation _queueOperation;
        private readonly ConfigManager _config;

        public ReportConsumer(IServiceScopeFactory serviceScopeFactory, IQueueOperation queueOperation, ConfigManager config)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _queueOperation = queueOperation;
            _config = config;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _queueOperation.ConsumeQueue(_config.ConsumeQueueConfiguration.QueueName, _config.ConsumeQueueConfiguration.ExchangeName, _config.ConsumeQueueConfiguration.ExchangeType,_config.ConsumeQueueConfiguration.RoutingKeyName, _config.ConsumeQueueConfiguration.PrefetchCount,
                receivedEventHandler: (model, ea) =>
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var _hotelDBContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
                        var _logger = scope.ServiceProvider.GetRequiredService<IElasticSearchLogger<GenericLogModel>>();

                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var reportResponse = JsonSerializer.Deserialize<ReportDTO>(message);

                        #region DB_Operation

                        var reportingItem = _hotelDBContext.ReportingInfo.SingleOrDefault(x => x.Id == reportResponse.ReportId);
                        if (reportingItem != null)
                        {
                            var registeredHotels = _hotelDBContext.ContactInfo.Where(x =>
                                x.InfoType == Enum.ContactInfoType.Location &&
                                x.InfoContent.ToLower() == reportResponse.Location.ToLower());
                            var hotelList = registeredHotels.Select(x => x.FK_HotelInfo).Distinct().ToList();

                            if (registeredHotels.Any())
                            {
                                reportingItem.HotelCount = hotelList.Count();
                                reportingItem.PhoneCount = _hotelDBContext.ContactInfo.Where(x =>
                                    x.InfoType == Enum.ContactInfoType.PhoneNumber && hotelList.Contains(x.FK_HotelInfo)).ToList().Count();
                                reportingItem.Status = Enum.ReportStatus.Completed;
                                reportingItem.UpdatedDate = DateTime.Now;

                                _hotelDBContext.ReportingInfo.Update(reportingItem);
                                _hotelDBContext.SaveChanges();
                            }

                            _logger.AddLog(new GenericLogModel()
                            {
                                Controller = "ReportConsumer",
                                Method = "ExecuteAsync",
                                Message = ResponseMessageConst.HandleReportRabbitMQ,
                                Type = Type.Success
                            });
                            ((EventingBasicConsumer)model).Model.BasicAck(ea.DeliveryTag, false);
                        }
                        #endregion
                    }

                });
            return Task.CompletedTask;
        }
    }
}
