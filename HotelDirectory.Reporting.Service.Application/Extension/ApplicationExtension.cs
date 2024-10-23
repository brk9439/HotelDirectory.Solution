namespace HotelDirectory.Reporting.Service.Application.Extension
{
    public static class ApplicationExtension
    {
        public static IServiceCollection RegisterService(IServiceCollection services, IConfigurationRoot configuration)
        {

            HotelDirectory.Reporting.Service.Business.Extension.BusinessExtension.RegisterService(services, configuration);
            return services;
        }
    }
}
