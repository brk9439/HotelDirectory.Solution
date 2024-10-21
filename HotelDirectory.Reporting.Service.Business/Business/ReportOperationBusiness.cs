using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelDirectory.Reporting.Service.Business.Model;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Context;
using HotelDirectory.Reporting.Service.Infrastructure.Data.Entities;
using Enum = HotelDirectory.Reporting.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Reporting.Service.Business.Business
{
    public interface IReportOperationBusiness
    {
        Task<string> CreateReport(string byLocation);
    }

    public class ReportOperationBusiness : IReportOperationBusiness
    {
        private readonly HotelDbContext _hotelDbContext;
        public async Task<string> CreateReport(string byLocation)
        {
            var location = _hotelDbContext.ContactInfo.Where(x => x.InfoType == Enum.ContactInfoType.Location && x.InfoContent.ToLower() == byLocation.ToLower());
            if (!location.Any())
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

                ReportQueueRequest reportQueueRequest = new ReportQueueRequest()
                {
                    ReportId = reportingInfo.Id,
                    Location = reportingInfo.Location,
                };


                #endregion
                return "Rapor talebi alındı";
            }
            else
            {
                return "Girilen konum bulunamadı";
            }

        }
    }
}
