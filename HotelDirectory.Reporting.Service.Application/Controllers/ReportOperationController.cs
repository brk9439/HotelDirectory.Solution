using HotelDirectory.Reporting.Service.Business.Business;
using Microsoft.AspNetCore.Mvc;

namespace HotelDirectory.Reporting.Service.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportOperationController : ControllerBase
    {
        private readonly IReportOperationBusiness _reportOperationBusiness;

        public ReportOperationController(IReportOperationBusiness reportOperationBusiness)
        {
            _reportOperationBusiness = reportOperationBusiness;
        }

        [HttpGet("CreateReport/{byLocation}")]
        public async Task<string> CreateReport(string byLocation)
        {
            return await _reportOperationBusiness.CreateReport(byLocation);
        }

    }
}
