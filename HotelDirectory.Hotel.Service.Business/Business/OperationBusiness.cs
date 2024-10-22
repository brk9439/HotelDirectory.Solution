using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using HotelDirectory.Hotel.Service.Business.Model.Response;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using HotelDirectory.Shared.Common;
using HotelDirectory.Shared.ElasticSearch;
using HotelDirectory.Shared.ElasticSearch.Model;
using Microsoft.EntityFrameworkCore;
using Enum = HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;
using Type = HotelDirectory.Shared.ElasticSearch.Model.Type;

namespace HotelDirectory.Hotel.Service.Business.Business
{
    public interface IOperationBusiness
    {
        Task<BaseResponseModel<object>> CreateHotel(CreateHotelRequest createHotelRequest);
        Task<BaseResponseModel<object>> RemoveHotel(Guid hotelId);
        Task<BaseResponseModel<object>> CreateContact(CreateContactRequest createContactRequest);
        Task<BaseResponseModel<object>> RemoveContact(Guid contactId);
        Task<BaseResponseModel<object>> GetHotelInfo(Guid hotelId);
        Task<BaseResponseModel<object>> GetDetailInfo(Guid hotelId);
    }

    public class OperationBusiness : IOperationBusiness
    {
        private readonly HotelDbContext _hotelDbContext;
        private readonly IElasticSearchLogger<GenericLogModel> _logger;

        public OperationBusiness(HotelDbContext hotelDbContext, IElasticSearchLogger<GenericLogModel> logger)
        {
            _hotelDbContext = hotelDbContext;
            _logger = logger;
        }

        public async Task<BaseResponseModel<object>> CreateHotel(CreateHotelRequest createHotelRequest)
        {
            var hotelInfo = new HotelInfo
            {
                CompanyName = createHotelRequest.CompanyName,
                PersonName = createHotelRequest.PersonName,
                PersonSurname = createHotelRequest.PersonSurname,
                Status = Enum.Status.Active
            };

            await _hotelDbContext.HotelInfo.AddAsync(hotelInfo);
            await _hotelDbContext.SaveChangesAsync();

            _logger.AddLog(new GenericLogModel
            {
                Controller = "HotelOperation",
                Method = "CreateHotel",
                Message = ResponseMessageConst.HotelCreatedSuccessMessage,
                Type = Type.Success
            });

            return new BaseResponseModel<object>
            {
                Message = ResponseMessageConst.HotelCreatedSuccessMessage,
                StatusCode = HttpStatusCode.OK
            };

        }

        public async Task<BaseResponseModel<object>> RemoveHotel(Guid hotelId)
        {

            var hotelItem = _hotelDbContext.HotelInfo.SingleOrDefault(x => x.Id == hotelId);
            if (hotelItem != null)
            {
                hotelItem.Status = Enum.Status.Passive;
                hotelItem.DeletedDate = DateTime.Now;

                _hotelDbContext.HotelInfo.Update(hotelItem);
                _hotelDbContext.SaveChanges();
            }
            else
            {
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "HotelOperation",
                    Method = "RemoveHotel",
                    Message = ResponseMessageConst.HotelRemovedContextNullMessage,
                    Type = Type.NotFound
                });

                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.HotelRemovedContextNullMessage,
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            _logger.AddLog(new GenericLogModel
            {
                Controller = "HotelOperation",
                Method = "RemoveHotel",
                Message = ResponseMessageConst.HotelRemovedSuccessMessage,
                Type = Type.Success
            });

            return new BaseResponseModel<object>
            {
                Message = ResponseMessageConst.HotelRemovedSuccessMessage,
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<BaseResponseModel<object>> CreateContact(CreateContactRequest createContactRequest)
        {
            ContactInfo contactInfo = new ContactInfo
            {
                FK_HotelInfo = createContactRequest.HotelId,
                InfoType = createContactRequest.InfoType,
                InfoContent = createContactRequest.InfoContent,
                Status = Enum.Status.Active
            };
            await _hotelDbContext.ContactInfo.AddAsync(contactInfo);
            await _hotelDbContext.SaveChangesAsync();

            _logger.AddLog(new GenericLogModel
            {
                Controller = "HotelOperation",
                Method = "CreateContact",
                Message = ResponseMessageConst.ContactCreatedSuccessMessage,
                Type = Type.Success
            });

            return new BaseResponseModel<object>
            {
                Message = ResponseMessageConst.ContactCreatedSuccessMessage,
                StatusCode = HttpStatusCode.OK

            };
        }

        public async Task<BaseResponseModel<object>> RemoveContact(Guid contactId)
        {
            var contactItem = _hotelDbContext.ContactInfo.SingleOrDefault(x => x.Id == contactId);
            if (contactItem != null)
            {
                contactItem.DeletedDate = DateTime.Now;
                contactItem.Status = Enum.Status.Passive;
                _hotelDbContext.ContactInfo.Update(contactItem);
                _hotelDbContext.SaveChanges();
            }
            else
            {
                _logger.AddLog(new GenericLogModel
                {
                    Controller = "HotelOperation",
                    Method = "RemoveContact",
                    Message = ResponseMessageConst.ContactRemovedContextNullMessage,
                    Type = Type.NotFound
                });
                return new BaseResponseModel<object>
                {
                    Message = ResponseMessageConst.ContactRemovedContextNullMessage,
                    StatusCode = HttpStatusCode.NotFound

                };
            }

            _logger.AddLog(new GenericLogModel
            {
                Controller = "HotelOperation",
                Method = "RemoveContact",
                Message = ResponseMessageConst.ContactRemovedSuccessMessage,
                Type = Type.Success
            });

            return new BaseResponseModel<object>
            {
                Message = ResponseMessageConst.ContactRemovedSuccessMessage,
                StatusCode = HttpStatusCode.OK

            };
        }

        public async Task<BaseResponseModel<object>> GetHotelInfo(Guid hotelId)
        {
            if (hotelId != Guid.Empty)
            {
                var hotelItem = _hotelDbContext.HotelInfo.SingleOrDefault(x => x.Id == hotelId);
                if (hotelItem != null)
                {
                    HotelInfoResponse hotelInfo = new HotelInfoResponse
                    {
                        HotelId = hotelItem.Id,
                        PersonName = hotelItem.PersonName,
                        PersonSurname = hotelItem.PersonSurname,
                        CompanyName = hotelItem.CompanyName,
                    };

                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetHotelInfo",
                        Message = ResponseMessageConst.GetHotelInfoSuccessMessage,
                        Type = Type.Success
                    });
                    return new BaseResponseModel<object>
                    {
                        Data = hotelInfo,
                        StatusCode = HttpStatusCode.OK,
                        Message = ResponseMessageConst.GetHotelInfoSuccessMessage
                    };
                }
                else
                {
                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetHotelInfo",
                        Message = ResponseMessageConst.GetHotelInfoNullMessage,
                        Type = Type.NotFound
                    });
                    return new BaseResponseModel<object>
                    {
                        Message = ResponseMessageConst.GetHotelInfoNullMessage,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        Success = false

                    };
                }


            }
            else
            {
                var hotelItems = _hotelDbContext.HotelInfo.Where(x => x.Status == Enum.Status.Active);
                if (hotelItems.Any())
                {
                    List<HotelInfoResponse> hotelInfoList = hotelItems.Select(x => new HotelInfoResponse
                    {
                        PersonName = x.PersonName,
                        PersonSurname = x.PersonSurname,
                        CompanyName = x.CompanyName,
                        HotelId = x.Id
                    }).ToList();

                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetHotelInfo",
                        Message = ResponseMessageConst.GetHotelInfoSuccessMessage,
                        Type = Type.Success
                    });
                    return new BaseResponseModel<object>
                    {
                        Data = hotelInfoList,
                        StatusCode = HttpStatusCode.OK,
                        Message = ResponseMessageConst.GetHotelInfoSuccessMessage
                    };
                }
                else
                {
                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetHotelInfo",
                        Message = ResponseMessageConst.GetHotelInfoNullMessage,
                        Type = Type.NotFound
                    });
                    return new BaseResponseModel<object>
                    {
                        Message = ResponseMessageConst.GetHotelInfoNullMessage,
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        Success = false

                    };
                }
            }

        }

        public async Task<BaseResponseModel<object>> GetDetailInfo(Guid hotelId)
        {

            if (hotelId != Guid.Empty)
            {
                var contactInfo = _hotelDbContext.ContactInfo.Where(x => x.FK_HotelInfo == hotelId && x.Status == Enum.Status.Active)
                .Select(x => new DetailContactInfo()
                {
                    InfoContent = x.InfoContent,
                    InfoType = x.InfoType,
                    ContactId = x.Id
                })
                .ToList();

                var hotelInfos = _hotelDbContext.HotelInfo.Where(x => x.Id == hotelId && x.Status == Enum.Status.Active).ToList();
                var detailInfos = hotelInfos.Select(x => new DetailHotelResponse()
                {
                    HotelId = x.Id,
                    PersonSurname = x.PersonSurname,
                    PersonName = x.PersonName,
                    CompanyName = x.CompanyName,
                    Contacts = contactInfo
                }).FirstOrDefault();
                if (detailInfos == null)
                {
                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetDetailInfo",
                        Message = ResponseMessageConst.GetDetailInfoNullMessage,
                        Type = Type.NotFound

                    });
                    return new BaseResponseModel<object>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        Message = ResponseMessageConst.GetDetailInfoNullMessage,
                        Success = false
                    };
                }

                _logger.AddLog(new GenericLogModel
                {
                    Controller = "HotelOperation",
                    Method = "GetDetailInfo",
                    Message = ResponseMessageConst.GetDetailInfoSuccessMessage,
                    Type = Type.Success
                });
                return new BaseResponseModel<object>
                {
                    Data = detailInfos,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = ResponseMessageConst.GetDetailInfoSuccessMessage

                };
            }
            else
            {
                var hotelInfos = _hotelDbContext.HotelInfo.Where(x => x.Status == Enum.Status.Active).ToList();
                List<DetailHotelResponse> detailInfos = new List<DetailHotelResponse>();
                if (hotelInfos.Any())
                {
                    hotelInfos.ForEach(x => detailInfos.Add(new DetailHotelResponse
                    {
                        HotelId = x.Id,
                        PersonName = x.PersonName,
                        PersonSurname = x.PersonSurname,
                        CompanyName = x.CompanyName,
                        Contacts = _hotelDbContext.ContactInfo.Where(x => x.FK_HotelInfo == x.Id && x.Status == Enum.Status.Active)
                            .Select(y => new DetailContactInfo()
                            {
                                InfoContent = y.InfoContent,
                                ContactId = y.Id,
                                InfoType = y.InfoType
                            }).ToList()
                    }));

                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetDetailInfo",
                        Message = ResponseMessageConst.GetDetailInfoSuccessMessage,
                        Type = Type.Success

                    });
                    return new BaseResponseModel<object>
                    {
                        Data = detailInfos,
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Message = ResponseMessageConst.GetDetailInfoSuccessMessage
                    };
                }
                else
                {
                    _logger.AddLog(new GenericLogModel
                    {
                        Controller = "HotelOperation",
                        Method = "GetDetailInfo",
                        Message = ResponseMessageConst.GetDetailInfoNullMessage,
                        Type = Type.NotFound

                    });
                    return new BaseResponseModel<object>
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        Message = ResponseMessageConst.GetDetailInfoNullMessage,
                        Success = false
                    };
                }

            }


        }
    }
}
