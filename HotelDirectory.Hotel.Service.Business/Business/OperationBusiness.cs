using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using HotelDirectory.Hotel.Service.Business.Model.Response;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using HotelDirectory.Shared.ElasticSearch;
using HotelDirectory.Shared.ElasticSearch.Model;
using Microsoft.EntityFrameworkCore;
using Enum = HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Business.Business
{
    public interface IOperationBusiness
    {
        Task<string> CreateHotel(CreateHotelRequest createHotelRequest);
        Task<string> RemoveHotel(Guid hotelId);
        Task<string> CreateContact(CreateContactRequest createContactRequest);
        Task<string> RemoveContact(Guid contactId);
        Task<object> GetHotelInfo(Guid hotelId);
        Task<object> GetDetailInfo(Guid hotelId);
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

        public async Task<string> CreateHotel(CreateHotelRequest createHotelRequest)
        {
            HotelInfo hotelInfo = new HotelInfo
            {
                CompanyName = createHotelRequest.CompanyName,
                PersonName = createHotelRequest.PersonName,
                PersonSurname = createHotelRequest.PersonSurname,
                Status = Enum.Status.Active
            };
            await _hotelDbContext.HotelInfo.AddAsync(hotelInfo);
            await _hotelDbContext.SaveChangesAsync();


            return "Kayıt başarılı";
        }

        public async Task<string> RemoveHotel(Guid hotelId)
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
                return "İlgili hotel bulunamadı";

            return "Hotel kaldırıldı.";
        }

        public async Task<string> CreateContact(CreateContactRequest createContactRequest)
        {
            ContactInfo contactInfo = new ContactInfo
            {
                //FK_HotelInfo = createContactRequest.HotelId,
                InfoType = createContactRequest.InfoType,
                InfoContent = createContactRequest.InfoContent,
                Status = Enum.Status.Active
            };
            await _hotelDbContext.ContactInfo.AddAsync(contactInfo);
            await _hotelDbContext.SaveChangesAsync();

            return "Kayıt başarılı";
        }

        public async Task<string> RemoveContact(Guid contactId)
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
                return "İlgili iletişim bilgisi bulunamadı";

            return "Başarılı";
        }

        public async Task<object> GetHotelInfo(Guid hotelId)
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
                    return hotelInfo;
                }
                else
                {
                    return "İlgili Hotel bilgisi bulunamadı";
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
                    return hotelInfoList;
                }
                else
                {
                    return "Hotel bilgisi bulunamadı";
                }
            }

        }

        public async Task<object> GetDetailInfo(Guid hotelId)
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
                return detailInfos;
            }
            else
            {
                var hotelInfos = _hotelDbContext.HotelInfo.Where(x => x.Status == Enum.Status.Active).ToList();
                List<DetailHotelResponse> detailInfos = new List<DetailHotelResponse>();
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

                return detailInfos;
            }



        }
    }
}
