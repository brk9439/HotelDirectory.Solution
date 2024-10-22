using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelDirectory.Shared.Common
{
    public static class ResponseMessageConst
    {
        #region Hotel.Service.Business
        public const string HotelCreatedSuccessMessage = "Hotel kayıt işlemi başarılı";
        public const string HotelRemovedSuccessMessage = "Hotel kaldırma işlemi başarılı";
        public const string HotelRemovedContextNullMessage = "Aradığınız hotel bulunamadı";
        public const string ContactCreatedSuccessMessage = "İletişim bilgisi kayıt işlemi başarılı";
        public const string ContactRemovedSuccessMessage = "İletişim bilgisi kaldırma işlemi başarılı";
        public const string ContactRemovedContextNullMessage = "İletişim bilgisi bulunamadı";
        public const string GetHotelInfoSuccessMessage = "Hotel bilgisi iletildi";
        public const string GetHotelInfoNullMessage = "Hotel bilgisi bulunamadı";
        public const string GetDetailInfoSuccessMessage = "İletişim bilgisi iletildi";
        public const string GetDetailInfoNullMessage = "İletişim bilgisi bulunamadı";

        #endregion

        #region Reporting.Service.Business

        public const string CreateReportSuccessMessage = "Rapor oluşturma işlemi başlatıldı";
        public const string CreateReportNullMessage = "Lokasyon bilgisi bulunamadı";
        public const string GetListReportSuccessMessage = "Raporların listelenmesi başarılı";
        public const string GetListReportNullMessage = "Raporların listelenmesi başarısız";

        #endregion

        #region RabbitMQ Message

        public const string SendReportRabbitMQ = "Rapor isteği kuyruğa gönderildi";
        public const string HandleReportRabbitMQ = "Rapor oluşturulmak üzere kuyrukta işlendi";


        #endregion

    }
}
