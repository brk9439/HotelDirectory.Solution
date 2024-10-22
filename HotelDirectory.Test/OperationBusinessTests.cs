using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelDirectory.Hotel.Service.Business.Business;
using HotelDirectory.Hotel.Service.Business.Model.Request;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Context;
using HotelDirectory.Hotel.Service.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using static HotelDirectory.Hotel.Service.Infrastructure.Data.Entities.Enum;

namespace HotelDirectory.Hotel.Service.Tests
{
    public class OperationBusinessTests
    {
        private readonly HotelDbContext _context;
        private readonly OperationBusiness _operationBusiness;

        public OperationBusinessTests()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
            .UseNpgsql("Server=localhost;Port=5435;Database=hoteldb;User Id=hotel_user;Password=hotel_password")
            .Options;

            _context = new HotelDbContext(options);
            _operationBusiness = new OperationBusiness(_context);
        }

        [Theory]
        [InlineData("TEST HOTEL", "Can", "Önal")]
        public async Task CreateHotel_ShouldAddHotel(string companyName, string personName, string personSurname)
        {
            var request = new CreateHotelRequest
            {
                CompanyName = companyName,
                PersonName = personName,
                PersonSurname = personSurname
            };

            var result = await _operationBusiness.CreateHotel(request);

            Assert.Equal("Kayıt başarılı", result);
            Assert.Single(_context.HotelInfo);
        }

        [Theory]
        [InlineData("a1d5d279-b164-4ecb-8f21-3107c6e97994")]
        public async Task RemoveHotel_ShouldReturnNotFound(string hotelIdStr)
        {
            var hotelId = Guid.Parse(hotelIdStr);

            var result = await _operationBusiness.RemoveHotel(hotelId);

            Assert.Equal("İlgili hotel bulunamadı", result);
        }

        [Fact]
        public async Task RemoveHotel_ShouldSoftDeleteHotel()
        {
            var hotel = new HotelInfo
            {
                Id = Guid.NewGuid(),
                CompanyName = "TEST HOTEL",
                PersonName = "Ahmet",
                PersonSurname = "Önal",
                Status = Status.Active
            };

            _context.HotelInfo.Add(hotel);
            await _context.SaveChangesAsync();

            var result = await _operationBusiness.RemoveHotel(hotel.Id);

            Assert.Equal("Hotel kaldırıldı.", result);
            var removedHotel = await _context.HotelInfo.FindAsync(hotel.Id);
            Assert.Equal(Status.Passive, removedHotel.Status);
        }

        [Theory]
        [InlineData(ContactInfoType.MailAddress, "test@testhotel.com")]
        public async Task CreateContact_ShouldAddContact(ContactInfoType infoType, string infoContent)
        {
            var request = new CreateContactRequest
            {
                InfoType = infoType,
                InfoContent = infoContent,
            };

            var result = await _operationBusiness.CreateContact(request);

            Assert.Equal("Kayıt başarılı", result);
            Assert.Single(_context.ContactInfo);
        }

        [Theory]
        [InlineData("fcf7a57e-bd9d-4c51-8c45-982895af4a29")]
        public async Task RemoveContact_ShouldReturnNotFound(string contactIdStr)
        {
            var contactId = Guid.Parse(contactIdStr);

            var result = await _operationBusiness.RemoveContact(contactId);

            Assert.Equal("İlgili iletişim bilgisi bulunamadı", result);
        }

        [Fact]
        public async Task RemoveContact_ShouldSoftDeleteContact()
        {
            var contact = new ContactInfo
            {
                Id = Guid.NewGuid(),
                InfoType = ContactInfoType.Location,
                InfoContent = "Ankara",
                Status = Status.Active
            };

            _context.ContactInfo.Add(contact);
            await _context.SaveChangesAsync();

            var result = await _operationBusiness.RemoveContact(contact.Id);

            Assert.Equal("Başarılı", result);
            var removedContact = await _context.ContactInfo.FindAsync(contact.Id);
            Assert.Equal(Status.Passive, removedContact.Status);
        }
    }
}