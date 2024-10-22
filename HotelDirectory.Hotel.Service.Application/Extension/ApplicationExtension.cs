namespace HotelDirectory.Hotel.Service.Application.Extension
{
    public static class ApplicationExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {
            HotelDirectory.Hotel.Service.Business.Extension.BusinessExtension.RegisterService(services, configuration);
            HotelDirectory.Shared.ElasticSearch.Extension.ElasticSearchExtension.RegisterService(services, configuration);

            return services;
        }
    }
}
