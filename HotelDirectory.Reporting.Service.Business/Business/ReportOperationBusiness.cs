using HotelDirectory.Reporting.Service.Infrastructure.Data.Context;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Entities;
using HotelDirectory.Reporting.Service.Infrastructure.RabbitMQClient.Interface;
using HotelDirectory.Shared.Common;
using HotelDirectory.Shared.ElasticSearch.Model;
using HotelDirectory.Shared.ElasticSearch;
using Enum = HotelDirectory.Reporting.Service.Infrastructure.Data.Entities.Enum;
using System.Net;
using Type = HotelDirectory.Shared.ElasticSearch.Model.Type;
using HotelDirectory.Reporting.Service.Business.Model.Request;
using HotelDirectory.Reporting.Service.Business.Model.Response;
using Nest;

namespace HotelDirectory.Reporting.Service.Business.Business
{
    public interface IReportOperationBusiness
    {
        Task<BaseResponseModel<object>> CreateReport(string byLocation);
        Task<BaseResponseModel<object>> GetListReport();
    }

    public class ReportOperationBusiness : IReportOperationBusiness
    {
        private readonly HotelDbContext _hotelDbContext;
        private readonly IQueueOperation _queueOperation;
        private readonly IElasticSearchLogger<GenericLogModel> _logger;
        public ReportOperationBusiness(HotelDbContext hotelDbContext, IQueueOperation queueOperation, IElasticSearchLogger<GenericLogModel> logger)
        {
            _hotelDbContext = hotelDbContext;
            _queueOperation = queueOperation;
            _logger = logger;
        }

        public async Task<BaseResponseModel<object>> CreateReport(string byLocation)
        {
            var location = _hotelDbContext.ContactInfo.Where(x => x.InfoType == Enum.ContactInfoType.Location && x.InfoContent.ToLower() == byLocation.ToLower());
            if (location.Any())
            {
                ReportingInfo reportingInfo = new ReportingInfo
                {
                    HotelCount = 0,
                    PhoneCount = 0,
                    GetDate = DateTime.Now,
                    CreatedDate = DateTime.Now,
                    Location = location.FirstOrDefault().InfoContent.ToLower(),
                    Status = Enum.ReportStatus.Waiting
                };
                await _hotelDbContext.ReportingInfo.AddAsync(reportingInfo);
                await _hotelDbContext.SaveChangesAsync();

                #region Rabbit mq

                _logger.AddLog(new GenericLogModel()
                {
                    Controller = "ReportOperation",
                    Method = "ReportSendRabbitMQ",
                    Message = ResponseMessageConst.SendReportRabbitMQ,
                    Type = Type.Success
                });

                ReportQueueRequest reportQueueRequest = new ReportQueueRequest()
                {
                    ReportId = reportingInfo.Id,
                    Location = reportingInfo.Location,
                };
                _queueOperation.PublishMessage(reportQueueRequest, "report_queue", "report_direct", "report_key", 0);

                #endregion

                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "CreateReport",
                    Message = ResponseMessageConst.HotelCreatedSuccessMessage,
                    Type = Type.Success
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.CreateReportSuccessMessage,
                    StatusCode = HttpStatusCode.OK,
                    Data = reportQueueRequest
                };
            }
            else
            {
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "CreateReport",
                    Message = ResponseMessageConst.CreateReportNullMessage,
                    Type = Type.NotFound
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.CreateReportNullMessage,
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }

        }
        /// <summary>
        /// Tüm Raporların bilgisini getirir.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponseModel<object>> GetListReport()
        {
            var getList = _hotelDbContext.ReportingInfo.ToList();
            if (getList.Any())
            {
                var result = getList.Select(x => new ReportResponse()
                {
                    Status = x.Status == Enum.ReportStatus.Completed ? "Tamamlandı" : x.Status == Enum.ReportStatus.Waiting ? "Hazırlanıyor" : string.Empty,
                    HotelCount = x.HotelCount,
                    PhoneCount = x.PhoneCount,
                    Location = x.Location,
                    ReportStartDate = x.GetDate,
                    ReportEndDate = x.UpdatedDate

                });

                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "GetListReport",
                    Message = ResponseMessageConst.GetListReportSuccessMessage,
                    Type = Type.Success
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.GetListReportSuccessMessage,
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };
            }
            else
            {
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "GetListReport",
                    Message = ResponseMessageConst.GetListReportNullMessage,
                    Type = Type.NotFound
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.GetListReportNullMessage,
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
        }

        public async Task<BaseResponseModel<object>> GetReport(Guid reportId)
        {
            var reportInfo = _hotelDbContext.ReportingInfo.SingleOrDefault(x => x.Id == reportId);
            if (reportInfo == null)
            {
                var result = new ReportResponse()
                {
                    Status = reportInfo.Status == Enum.ReportStatus.Completed ? "Tamamlandı" :
                        reportInfo.Status == Enum.ReportStatus.Waiting ? "Hazırlanıyor" : string.Empty,
                    HotelCount = reportInfo.HotelCount,
                    PhoneCount = reportInfo.PhoneCount,
                    Location = reportInfo.Location,
                    ReportStartDate = reportInfo.GetDate,
                    ReportEndDate = reportInfo.UpdatedDate,
                };
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "GetReport",
                    Message = ResponseMessageConst.GetListReportSuccessMessage,
                    Type = Type.Success
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.GetListReportSuccessMessage,
                    StatusCode = HttpStatusCode.OK,
                    Data = result
                };
            }
            else
            {
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "ReportOperation",
                    Method = "GetReport",
                    Message = ResponseMessageConst.GetListReportNullMessage,
                    Type = Type.NotFound
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.GetListReportNullMessage,
                    StatusCode = HttpStatusCode.NotFound,
                    Success = false
                };
            }
        }
    }
}
