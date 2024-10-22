using HotelDirectory.Reporting.Service.Business.Business;
using HotelDirectory.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerResponse(200, "Başarılı olması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [SwaggerResponse(404, "Data bulunamaması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [SwaggerResponse(500, "Herhangi bir hata olması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [HttpGet("CreateReport/{byLocation}")]
        public async Task<IActionResult> CreateReport(string byLocation)
        {
            return Ok(await _reportOperationBusiness.CreateReport(byLocation));
        }

        [HttpGet("GetListReport")]
        public async Task<IActionResult> GetListReport()
        {
            return Ok(await _reportOperationBusiness.GetListReport());
        }

    }
}
