# HotelDirectory.Solution
-----------------------------------------------------------------------------------------------------------------------------------------------
# ReadME
# C:\ altında, HotelService adında bir klasör oluşturup GitHub'taki klon linki üzerinden proje dosyalarını indirip oluşturunuz.
# Veri bağlantısı için postgresql veritabanını aşağıdaki bilgiler ile oluşturmalısınız. Tablo yapısı uygulama ayağa kalktığında oluşacaktır.
# Terminal üzerinden projenin bulunduğu dizine gidip  "C:\HotelService"  "docker-compose up" komutu ile Projeye ait tüm conteinerları ayağa kaldırıyoruz.

# Containerlar başarıyla ayağa kalktıktan sonra ulaşmak için aşağıdaki URL'leri kullanabilirsiniz.


RabbitMQUI : http://localhost:15673/
RabbitMQ   : http://localhost:5672/
  Username: user
  Password: password

HotelPostgres   : localhost:5435
  Username : hotel_user
  Password : hotel_password
  Database : hoteldb

ElasticSearch : http://localhost:9201/
ElasticSearchKibana :http://localhost:5602/  

HotelDirectory.Hotel.Service.Application : http://localhost:5003/
HotelDirectory.Hotel.Service.Application.Swagger : http://localhost:5003/swagger/index.html

HotelDirectory.Reporting.Service.Application : http://localhost:5004/
HotelDirectory.Reporting.Service.Application.Swagger : http://localhost:5004/swagger/index.html

HotelDirectory.Reporting.Service.Consumer : http://localhost:5002/
HotelDirectory.Reporting.Service.Consumer.Swagger : http://localhost:5002/swagger/index.html


-----------------------------------------------------------------------------------------------------------------------------------------------

Api Endpointleri Ve RabbitMQ Queue bilgileri aşağıdaki gibidir.

#Api ---- HotelDirectory.Hotel.Service.Application ----

Url : api/Operation/CreateHotel
Http Method : Post
Description : Yeni Hotel kaydı oluşturur.

Request Body : 
{
  "personName": "isim",
  "personSurname": "soyisim",
  "companyName": "hotel adı"
}

-----

Url : api/Operation/RemoveHotel/{hotelId}
Http Method : Get
Description : Varolan hotel listeden silinir/kaldırılır. 

Request Body : {4dd3663f-3d66-451d-8d02-968f050e4001}

----

Url : api/Operation/CreateContact
Http Method : Post
Description : Yeni iletişim bilgileri oluşturur. infoType = 1-Telefon, 2-Mail, 3-Lokasyon

Request Body : 
{
  "hotelId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "infoType": 1,
  "infoContent": "Lokasyon"
}

----

Url : api/Operation/RemoveContact/{contactId}
Http Method : Get
Description : Varolan iletişim bilgisi silinir/kaldırılır.

Request Body : {3fa85f64-5717-4562-b3fc-2c963f66afa6}

---- 

Url : api/Operation/GetHotelInfo/{hotelId}
Http Method : Get
Description : Hotel bilgisini getirir.

Request Body : {3fa85f64-5717-4562-b3fc-2c963f66afa6}

-----

Url : api/Operation/GetDetailInfo/
Http Method : Get
Description : Detaylı iletişim bilgisini getirir. 

Request Body : {3fa85f64-5717-4562-b3fc-2c963f66afa6}

-----

#Api ----- HotelDirectory.Reporting.Service.Application

Url : api/ReportOperation/CreateReport/{byLocation}
Http Method : Get
Description : Şehir bilgisi girilerek ilgili şehirdeki hotel kayıtlarına göre rapor oluşturma adımı başlatılır. RabbitMQ kuyruğuna veri gönderilir. ReportInfo tablosunda veri kaydı oluşturulur. Rapor sonuçları bu adımda oluşturulmaz.

Request Body : {Ankara}

---- 

Url : api/ReportOperation/GetListReport
Http Method : Get
Description : Tüm rapor sonuçlarını getirir.

Request Body : 

---- 

Url : api/ReportOperation/GetReport/{reportId}
Http Method : Get
Description : Guid'si verilen raporun sonuçlarını getirir.

Request Body : {111cf32a-1a57-4e33-8b2f-1e7b2814ecbc}


-----------------------------------------------------------------------------------------------------------------------------------------------
#ElasticSearch

Proje içerisinde bulunan tüm api uçlarının sonuçları elasticSearch'e loglanır. Kibana üzerinden "elastic-log" indexine yazılır. Ayrıca kuyruğa gönderilen ve kuyrukta işlenen işlemler içinde elastic search üzerinde log tutulmaktadır. 

-----------------------------------------------------------------------------------------------------------------------------------------------

#RabbitMQ

Rapor oluşturma isteklerinde verilerin Report Application katmanından RabbitMQ kuyruğuna gitmektedir. Consumer olarak Background Service tarafından kuyruktaki istekler dinlenmektedir. Bu yapı sayesinden kuyruğa gelen işlemler asenkron olarak işlenir.  Kuyruktan alınan veriler sayesinde rapor oluşturulur ve ilgili veri tabanı tablosuna kayıt edilir. 
