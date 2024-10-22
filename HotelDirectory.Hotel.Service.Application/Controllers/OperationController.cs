using HotelDirectory.Hotel.Service.Business.Business;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HotelDirectory.Hotel.Service.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private readonly IOperationBusiness _operationBusiness;

        public OperationController(IOperationBusiness operationBusiness)
        {
            _operationBusiness = operationBusiness;
        }

        [SwaggerResponse(200, "Başarılı olması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [SwaggerResponse(404, "Data bulunamaması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [SwaggerResponse(500, "Herhangi bir hata olması durumunda kullanılan hata mesajı", typeof(HotelDirectory.Shared.Common.BaseResponseModel<string>))]
        [HttpPost("CreateHotel")]
        public async Task<IActionResult> CreateHotel(CreateHotelRequest createHotelRequest)
        {
            return Ok(await _operationBusiness.CreateHotel(createHotelRequest));
        }

        [HttpGet("RemoveHotel/{hotelId}")]
        public async Task<IActionResult> RemoveHotel(Guid hotelId)
        {
            return Ok(await _operationBusiness.RemoveHotel(hotelId));
        }

        [HttpPost("CreateContact")]
        public async Task<IActionResult> CreateContact(CreateContactRequest createContactRequest)
        {
            return Ok(await _operationBusiness.CreateContact(createContactRequest));
        }

        [HttpGet("RemoveContact/{contactId}")]
        public async Task<IActionResult> RemoveContact(Guid contactId)
        {
            return Ok(await _operationBusiness.RemoveContact(contactId));
        }
        [HttpGet("GetHotelInfo/{hotelId}")]
        public async Task<IActionResult> GetHotelInfo(Guid? hotelId)
        {
            return Ok(await _operationBusiness.GetHotelInfo(hotelId.Value));
        }

        [HttpGet("GetDetailInfo/{hotelId}")]
        public async Task<IActionResult> GetDetailInfo(Guid hotelId)
        {
            return Ok(await _operationBusiness.GetDetailInfo(hotelId));
        }
    }
}
